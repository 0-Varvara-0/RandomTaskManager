namespace RandomTaskManager.DTOs.Tasks
{
    // Отправка данных задачи клиенту
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string? CategoryName { get; set; }
    }
}