using Microsoft.EntityFrameworkCore;
using RandomTaskManager.Data;
using RandomTaskManager.Models.Entities;
using RandomTaskManager.Models.Enums;
using RandomTaskManager.Services.Interfaces;

using Task = System.Threading.Tasks.Task; // Task — это системный асинхронный Task

namespace RandomTaskManager.Services.Implementations
{
    public class LimitService : ILimitService
    {
        private readonly AppDbContext _context; // ссылку на контекст можно присвоить только в конструкторе
        private const int DEFAULT_MAX_ATTEMPTS = 3;

        public LimitService(AppDbContext context)
        {
            _context = context;
        }

        // Проверяет, можно ли пользователю выдать новую задачу в этом периоде
        public async Task<bool> CanGenerateTaskAsync(int userId, Period period = Period.Day)
        {
            var limit = await GetOrCreateLimitAsync(userId, period); // приостанавливает выполнение метода на время обращения к postgres

            if (DateTime.UtcNow >= limit.ResetAt) // проверка времени сброса лимита
            {
                limit.Used = 0;
                limit.ResetAt = CalculateNextResetDate(period);
                await _context.SaveChangesAsync();
            }

            return limit.Used < limit.Max;
        }

        // Увеличивает счетчик использованных генераций на 1
        public async Task IncrementUsageAsync(int userId, Period period = Period.Day)
        {
            var limit = await GetOrCreateLimitAsync(userId, period);
            limit.Used++;
            await _context.SaveChangesAsync();
        }

        // Ищет запись счетчика лимитов пользователя в PostgreSQL для указанного периода
        private async Task<UserLimit> GetOrCreateLimitAsync(int userId, Period period)
        {
            // Выполняет SQL-запрос SELECT ... LIMIT 1, где ищет совпадение по UserId и Period
            var limit = await _context.UserLimits.FirstOrDefaultAsync(l => l.UserId == userId && l.Period == period);

            if (limit == null) // если запись не найдена - создаётся новый
            {
                limit = new UserLimit
                {
                    UserId = userId,
                    Period = period,
                    Used = 0,
                    Max = DEFAULT_MAX_ATTEMPTS,
                    ResetAt = CalculateNextResetDate(period)
                };
                _context.UserLimits.Add(limit);
                await _context.SaveChangesAsync();
            }

            return limit;
        }

        // Вычисляет дату и время следующего сброса лимита
        private static DateTime CalculateNextResetDate(Period period)
        {
            return period switch
            {
                Period.Day => DateTime.UtcNow.AddDays(1),
                Period.Week => DateTime.UtcNow.AddDays(7),
                Period.Month => DateTime.UtcNow.AddMonths(1),
                _ => DateTime.UtcNow.AddDays(1)
            };
        }
    }
}