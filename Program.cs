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

// Добавление поддержки контроллеров
builder.Services.AddControllers();

// Добавление генераторов спецификации Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включение веб-интерфейса Swagger UI в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Перенаправление HTTP-запросов на HTTPS
app.UseRouting(); // Маршрутизация запросов
app.UseAuthorization();

app.MapControllers(); // эндпоинт маршрутизации

app.Run();