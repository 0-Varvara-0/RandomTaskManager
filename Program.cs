using Microsoft.EntityFrameworkCore;
using RandomTaskManager.Data;
using RandomTaskManager.Services.Implementations;
using RandomTaskManager.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args); //Конфигурация приложения

// Подключение к PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options => // говорит использовать драйвер постгреса
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов бизнес-логики
builder.Services.AddScoped<ILimitService, LimitService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Добавление поддержки MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов
if (!app.Environment.IsDevelopment()) // Если не на пк разработчика
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Перенаправление HTTP-запросов на HTTPS
app.UseStaticFiles(); // Поддержка статических файлов

app.UseRouting(); // Маршрутизация запросов

app.UseAuthorization();

app.MapControllerRoute( // эндпоинт маршрутизации
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();