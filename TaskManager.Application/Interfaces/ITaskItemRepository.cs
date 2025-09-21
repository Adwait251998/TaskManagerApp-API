using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskItemRepository
    {
        public  Task<TaskItem> CreateTaskItemAsync(TaskItem taskItem);

        public  Task<List<TaskItemViewModel>> GetTaskListByUserId(int userId);
        public Task<TaskItem> UpdateTaskDetailsAsync(TaskItem taskItem);

    }
}
