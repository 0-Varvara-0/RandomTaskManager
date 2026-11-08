namespace RandomTaskManager.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        // Список заданий в этой категории
        public List<Task> Tasks { get; set; } = new();
    }
}