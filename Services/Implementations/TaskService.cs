using Microsoft.EntityFrameworkCore;
using RandomTaskManager.Data;
using RandomTaskManager.Models.Enums;
using RandomTaskManager.Services.Interfaces;

using TaskEntity = RandomTaskManager.Models.Entities.Task;
using TaskStatus = RandomTaskManager.Models.Enums.TaskStatus;

namespace RandomTaskManager.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly ILimitService _limitService;

        // Внедряем контекст базы и сервис лимитов
        public TaskService(AppDbContext context, ILimitService limitService)
        {
            _context = context;
            _limitService = limitService;
        }

        // Получение случайной задачи
        public async Task<TaskEntity?> GetRandomTaskAsync(int userId, Period period, int? categoryId = null)
        {
            // Проверяем, есть ли неиспользованные попытки для выбранного периода (Day/Week/Month)
            if (!await _limitService.CanGenerateTaskAsync(userId, period))
            {
                throw new InvalidOperationException($"Вы исчерпали 3 попытки генерации задач на ({period}).");
            }

            var previousPendingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Period == period && t.Status == TaskStatus.Pending);

            if (previousPendingTask != null) // Если у пользователя уже висела предпросмотренная задача
            {
                previousPendingTask.UserId = null;
                previousPendingTask.Status = TaskStatus.Available;
            }

            // Ищем доступные задачи из каталога
            var query = _context.Tasks.Include(t => t.Category).Where(t => t.Status == TaskStatus.Available && t.UserId == null && t.Period == period);

            if (categoryId.HasValue) // если юзер выбирал категорию
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            var availableTasks = await query.ToListAsync(); // выполнение запроса

            if (!availableTasks.Any()) // если не нашлось подходящей задачи
            {
                return null;
            }

            // Случайный выбор задачи из найденного списка доступных задач
            var random = new Random();
            var selectedTask = availableTasks[random.Next(availableTasks.Count)];

            // Переводим задачу во временный статус Pending
            selectedTask.UserId = userId;
            selectedTask.Status = TaskStatus.Pending;

            // Списываем 1 попытку реролла/генерации в LimitService
            await _limitService.IncrementUsageAsync(userId, period);

            await _context.SaveChangesAsync();
            return selectedTask;
        }

        // Принятие задачу
        public async Task<bool> AcceptTaskAsync(int taskId, int userId)
        {
            // Ищет задачу Pending у этого пользователя
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && t.Status == TaskStatus.Pending);

            if (task == null) // Задача не найдена
            {
                return false;
            }

            var existingActiveTask = await _context.Tasks.FirstOrDefaultAsync(t => t.UserId == userId && t.Period == task.Period && t.Status == TaskStatus.Active);

            if (existingActiveTask != null) // если уже есть активная задача в этом периоде
            {
                existingActiveTask.UserId = null;
                existingActiveTask.Status = TaskStatus.Available;
            }

            // Переводим новую задачу в статус Active и привязываем к пользователю
            task.Status = TaskStatus.Active;
            task.CreatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // Выполнение задачи
        public async Task<bool> CompleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId && t.Status == TaskStatus.Active);

            if (task == null)
            {
                return false;
            }

            task.Status = TaskStatus.Completed;
            await _context.SaveChangesAsync();
            return true;
        }

        // Получение активных задач
        public async Task<List<TaskEntity>> GetUserActiveTasksAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Status == TaskStatus.Active)
                .AsNoTracking() // для оптимизации запроса на чтение
                .ToListAsync();
        }

        // Сброс базы данных
        public async Task ResetDatabaseStateAsync()
        {
            // Возвращаем все задачи в исходный доступный статус
            var tasks = await _context.Tasks.ToListAsync();
            foreach (var task in tasks)
            {
                task.Status = TaskStatus.Available;
                task.UserId = null;
            }

            // Очищаем историю суточных лимитов пользователей
            _context.UserLimits.RemoveRange(_context.UserLimits);

            await _context.SaveChangesAsync();
        }
    }
}