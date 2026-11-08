using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.Models.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        
        public string Conditions { get; set; } = string.Empty; // Условие разблокировки

        public AchievementType Type { get; set; } = AchievementType.Permanent;
        public List<User> Users { get; set; } = new();
    }
}