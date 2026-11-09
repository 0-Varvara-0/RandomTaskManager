using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.Models.Entities
{
    public class UserLimit
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public Period Period { get; set; }
        public int Used { get; set; } = 0; // использовано попыток
        public int Max { get; set; } = 3; // максимальное количество генераций задачи за период
        public DateTime ResetAt { get; set; } // время сброса попыток генерации
    }
}