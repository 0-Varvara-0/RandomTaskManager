using Microsoft.EntityFrameworkCore;
using RandomTaskManager.Models.Entities;
using Task = RandomTaskManager.Models.Entities.Task; // чтобы не было конфликта с System.Threading.Tasks.Task

namespace RandomTaskManager.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор: получает настройки (строку подключения к Postgres) и передает их базовому классу DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

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
        }
    }
}