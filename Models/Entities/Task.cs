using RandomTaskManager.Models.Enums;
using TaskStatus = RandomTaskManager.Models.Enums.TaskStatus; // чтобы не было конфликта с System.Threading.Tasks.TaskStatus

namespace RandomTaskManager.Models.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int DurationMinutes { get; set; } // Время на выполнение

        public Difficulty Difficulty { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Внешний ключ и связь с категорией
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Внешний ключ и связь с пользователем
        public int? UserId { get; set; }
        public User? User { get; set; }

        public List<Tag> Tags { get; set; } = new();
    }
}