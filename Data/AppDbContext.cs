using Microsoft.EntityFrameworkCore;
using RandomTaskManager.Models.Entities;
using RandomTaskManager.Models.Enums;
using Task = RandomTaskManager.Models.Entities.Task; // чтобы не было конфликта с System.Threading.Tasks.Task
using TaskStatus = RandomTaskManager.Models.Enums.TaskStatus;

namespace RandomTaskManager.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор: получает настройки (строку подключения к Postgres) и передает их базовому классу DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        // Таблицы базы данных
        public DbSet<Task> Tasks => Set<Task>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Achievement> Achievements => Set<Achievement>();
        public DbSet<UserLimit> UserLimits => Set<UserLimit>();

        // Создание структуры БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Вызов стандартных настроек

            // Настройка связи "многие-ко-многим" между Task и Tag
            modelBuilder.Entity<Task>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Tasks);

            // Настройка связи "многие-ко-многим" между User и Achievement
            modelBuilder.Entity<User>()
                .HasMany(u => u.Achievements)
                .WithMany(a => a.Users);

            // Создаем тестового пользователя
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "TestUser",
                    Email = "student@nsu.ru",
                    PasswordHash = "test_hash_123",
                    Role = Role.User,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            // Добавляем базовые категории задач
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Спорт и Здоровье", Description = "Физические упражнения и активность" },
                new Category { Id = 2, Name = "Обучение и Развитие", Description = "Чтение, языки, программирование" },
                new Category { Id = 3, Name = "Быт и Органайзер", Description = "Уборка, планирование, порядок" }
            );

            // Заполняем каталог доступных задач
            modelBuilder.Entity<Task>().HasData(
                new Task { Id = 1, Title = "Сделать 20 отжиманий", Description = "Быстрый разгон крови и бодрость на день", Difficulty = Difficulty.Easy, Status = TaskStatus.Available, CategoryId = 1, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Task { Id = 2, Title = "Прочитать 15 страниц книги", Description = "Любая художественная или техническая литература", Difficulty = Difficulty.Medium, Status = TaskStatus.Available, CategoryId = 2, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Task { Id = 3, Title = "Разобрать рабочий стол", Description = "Удалить ненужные файлы и рассортировать загрузки", Difficulty = Difficulty.Easy, Status = TaskStatus.Available, CategoryId = 3, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}