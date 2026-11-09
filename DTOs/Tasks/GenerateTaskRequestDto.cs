using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.DTOs.Tasks
{
    // Запрос на выпадание задачи
    public class GenerateTaskRequestDto
    {
        public Period Period { get; set; } = Period.Day;
        public int? CategoryId { get; set; }
    }
}