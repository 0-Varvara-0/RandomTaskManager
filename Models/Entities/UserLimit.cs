using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.Models.Entities
{
    public class UserLimit
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public Period Period { get; set; }
        public int Used { get; set; }
        public int Max { get; set; }
    }
}