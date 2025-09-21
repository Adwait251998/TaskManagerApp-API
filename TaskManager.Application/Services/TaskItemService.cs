using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Services
{
    public class TaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository; 
        public TaskItemService(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        public async Task<CreateTaskItemDTO> CreateTask(CreateTaskItemDTO taskItemDTO)
        {
            var taskItem = new TaskItem
            {
                Title = taskItemDTO.Title,
                Description = taskItemDTO.Description,
                StatusId =  taskItemDTO.StatusId,
                UserId= taskItemDTO.UserId,
                CreatedAt = DateTime.UtcNow,
                DueDate = taskItemDTO.DueDate


            };

            var createdTask = await _taskItemRepository.CreateTaskItemAsync(taskItem);

            return new CreateTaskItemDTO
            {

                Title = createdTask.Title,
                Description = createdTask.Description,
                StatusId = createdTask.StatusId,
                UserId = createdTask.UserId,
                DueDate = createdTask.DueDate
            };
        }


        public async Task<List<TaskItemViewModel>> GetTaskItemsById(int userId)
        {
            var taskList = new List<TaskItemViewModel>();
            taskList = await _taskItemRepository.GetTaskListByUserId(userId);
            
            return taskList;
        }

        public async Task<CreateTaskItemDTO> UpdateTaskDetails(CreateTaskItemDTO taskItemDTO)
        {
            var taskItem = new TaskItem
            {
                Id = taskItemDTO.TaskId,
                Title = taskItemDTO.Title,
                Description = taskItemDTO.Description,
                StatusId = taskItemDTO.StatusId,
                UserId = taskItemDTO.UserId,
                CreatedAt = DateTime.UtcNow,
                DueDate = taskItemDTO.DueDate


            };

            var createdTask = await _taskItemRepository.UpdateTaskDetailsAsync(taskItem);

            return new CreateTaskItemDTO
            {
                TaskId = taskItemDTO.TaskId,
                Title = createdTask.Title,
                Description = createdTask.Description,
                StatusId = createdTask.StatusId,
                UserId = createdTask.UserId,
                DueDate = createdTask.DueDate
            };
        }
    }
}
