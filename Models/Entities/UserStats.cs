namespace RandomTaskManager.Models.Entities
{
    public class UserStats
    {
        public int CompletedTasksCount { get; set; }
        public int TotalHoursSpent { get; set; }
        public List<Category> TopCategories { get; set; } = new();
        public List<Achievement> UnlockedAchievements { get; set; } = new();
    }
}