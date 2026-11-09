using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Связанные данные
        public List<Task> Tasks { get; set; } = new();
        public List<UserLimit> Limits { get; set; } = new();
        public List<Achievement> Achievements { get; set; } = new();
    }
}