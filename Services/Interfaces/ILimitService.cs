using RandomTaskManager.Models.Enums;

namespace RandomTaskManager.Services.Interfaces
{
    public interface ILimitService
    {
        // Проверяет, можно ли пользователю выдать новую задачу в этом периоде
        Task<bool> CanGenerateTaskAsync(int userId, Period period = Period.Day);

        // Увеличивает счетчик использованных генераций на 1
        Task IncrementUsageAsync(int userId, Period period = Period.Day);
    }
}