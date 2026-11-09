using Microsoft.AspNetCore.Mvc;
using RandomTaskManager.DTOs.Tasks;
using RandomTaskManager.Services.Interfaces;

namespace RandomTaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шаблон URL маршрута
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IWebHostEnvironment _env;

        public TasksController(ITaskService taskService, IWebHostEnvironment env)
        {
            _taskService = taskService; // Внедряем сервис задач через конструктор
            _env = env;
        }

        /// <summary>
        /// Вытянуть случайную задачу на предпросмотр
        /// </summary>
        [HttpPost("generate")] // Метод обрабатывает post запросы по адресу /api/tasks/generate
        public async Task<IActionResult> GenerateTask([FromBody] GenerateTaskRequestDto request, [FromQuery] int userId = 1)
        {
            try
            {
                var task = await _taskService.GetRandomTaskAsync(userId, request.Period, request.CategoryId);

                if (task == null)
                {
                    return NotFound(new { message = "Нет доступных задач в каталоге для выбранного периода или категории." });
                }

                // Преобразование сущности БД в DTO
                var response = new TaskResponseDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Period = task.Period.ToString(),
                    Status = task.Status.ToString(),
                    DurationMinutes = task.DurationMinutes,
                    CategoryName = task.Category?.Name
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                // Если исчерпан лимит 3/3 попыток
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Подтвердить и принять предпросмотренную задачу в свой активный слот
        /// </summary>
        [HttpPost("accept/{taskId}")]
        public async Task<IActionResult> AcceptTask(int taskId, [FromQuery] int userId = 1)
        {
            var success = await _taskService.AcceptTaskAsync(taskId, userId);

            if (!success)
            {
                return BadRequest(new { message = "Не удалось принять задачу. Убедитесь, что она находится у вас на предпросмотре." });
            }

            return Ok(new { message = "Задача успешно принята к выполнению!" });
        }

        /// <summary>
        /// Отметить активную задачу как выполненную
        /// </summary>
        [HttpPost("complete/{taskId}")]
        public async Task<IActionResult> CompleteTask(int taskId, [FromQuery] int userId = 1)
        {
            var success = await _taskService.CompleteTaskAsync(taskId, userId);

            if (!success)
            {
                return BadRequest(new { message = "Не удалось завершить задачу. Задача не найдена." });
            }

            return Ok(new { message = "Поздравляем! Задача выполнена." });
        }

        /// <summary>
        /// Получить текущие активные задачи пользователя (до 3 штук: Day, Week, Month)
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTasks([FromQuery] int userId = 1)
        {
            var tasks = await _taskService.GetUserActiveTasksAsync(userId);

            var response = tasks.Select(t => new TaskResponseDto // превращает список задач в один ответ
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Period = t.Period.ToString(),
                Status = t.Status.ToString(),
                DurationMinutes = t.DurationMinutes,
                CategoryName = t.Category?.Name
            });

            return Ok(response);
        }

        /// <summary>
        /// [DEV ONLY] Сброс состояния всех задач и лимитов
        /// </summary>
        [HttpPost("reset")]
        public async Task<IActionResult> ResetData()
        {
            // эндпоинт работает только при локальной разработке
            if (!_env.IsDevelopment())
            {
                return Forbid("Данный метод доступен только в режиме разработки.");
            }

            await _taskService.ResetDatabaseStateAsync();
            return Ok(new { message = "Состояние базы данных и лимиты успешно сброшены!" });
        }
    }
}