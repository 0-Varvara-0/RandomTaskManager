using RandomTaskManager.Models.Enums;
using TaskEntity = RandomTaskManager.Models.Entities.Task;

namespace RandomTaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        // Вытягивает случайную задачу в указанный слот (Day, Week или Month)
        Task<TaskEntity?> GetRandomTaskAsync(int userId, Period period, int? categoryId = null);

        // Закрепляет задачу за пользователем при нажатии кнопки «Принять»
        Task<bool> AcceptTaskAsync(int taskId, int userId);

        // Отмечает задачу как выполненную
        Task<bool> CompleteTaskAsync(int taskId, int userId);

        // Возвращает список всех активных задач пользователя (максимум 3 — по одной на каждый слот)
        Task<List<TaskEntity>> GetUserActiveTasksAsync(int userId);
    }
}