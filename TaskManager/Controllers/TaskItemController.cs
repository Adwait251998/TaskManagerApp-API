using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;

namespace TaskManager.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TaskItemController:ControllerBase
    {
        private readonly ILogger<TaskItemController> _logger;
        public TaskItemService _taskItemService;
        private readonly StatusService _statusService;

        public TaskItemController(TaskItemService taskItemService, StatusService statusService, ILogger<TaskItemController> logger)
        {
            _taskItemService = taskItemService;
            _statusService = statusService;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskItemDTO taskItemDTO)
        {
            _logger.LogInformation("GetCreateTaskDTO called at {Time}", DateTime.UtcNow);
            _logger.LogInformation("GetCreateTaskDTO {data}:", JsonSerializer.Serialize(taskItemDTO));
            if (taskItemDTO == null)
                return Ok(new { Message = "Task not created successfully." });

            // Call the service to create the user
            var createdUser = await _taskItemService.CreateTask(taskItemDTO);

            return Ok(new { Message = "Task created successfully." });
        }

        [Authorize]
        [HttpGet("GetCreateTaskDTO")]
        public async Task<IActionResult> GetCreateTaskDTO()
        {
            var taskItemDTO = new CreateTaskFormDTO();
            var userDto = new UserDTO
            {
                Name = User.FindFirst("UserName")?.Value,
                Email = User.FindFirst("Email")?.Value,
                Id = int.Parse(User.FindFirst("userId")?.Value ?? "0"),
                PhoneNumber = User.FindFirst("PhoneNumber")?.Value,
            };
            var statuses = await _statusService.GetStatusList();
            taskItemDTO.Statuses = statuses;
            taskItemDTO.User = userDto;

            return new JsonResult(taskItemDTO);

        }
        [Authorize]
        [HttpGet("GetTaskByUserID")]
        public async Task<IActionResult> GetTaskByUserID( int userId)
        {
            List<TaskItemViewModel> itemList =   new List<TaskItemViewModel>();
            itemList =  await _taskItemService.GetTaskItemsById(userId);
            return new JsonResult(itemList);

        }

        [Authorize]
        [HttpPost("UpdateTaskDetails")]
        public async Task<IActionResult> UpdateTaskDetails([FromBody] CreateTaskItemDTO task)
        {
            _logger.LogInformation("GetCreateTaskDTO called at {Time}", DateTime.UtcNow);
            _logger.LogInformation("GetCreateTaskDTO {data}:", JsonSerializer.Serialize(task));
            if (task == null)
                return Ok(new { Message = "Task not created successfully." });

            // Call the service to create the user
            var createdUser = await _taskItemService.UpdateTaskDetails(task);

            return Ok(new { Message = "Task updated successfully." });
        }
    }
}
