using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.ViewModels;
using TaskManager.Core.Entities;

namespace TaskManager.Infastructure.Repositories
{
    public class TaskItemRepository:ITaskItemRepository
    {
        private readonly TaskManagerDBContext _taskManagerDBContext;
        public TaskItemRepository(TaskManagerDBContext taskManagerDBContext)
        {
            _taskManagerDBContext = taskManagerDBContext;
        }
        public async Task<TaskItem> CreateTaskItemAsync(TaskItem taskItem)
        {
            _taskManagerDBContext.Add(taskItem);
            await _taskManagerDBContext.SaveChangesAsync();
            return taskItem;
        }

        public async Task<List<TaskItemViewModel>> GetTaskListByUserId(int userId)
        {
            var parameter = new SqlParameter("@userId", userId);

            var result = await _taskManagerDBContext
                .Set<TaskItemViewModel>()        
                .FromSqlRaw("EXEC GET_TASKS_BY_USERID @userId", parameter)
                .ToListAsync();

            return result;
        }

        public async Task <TaskItem> UpdateTaskDetailsAsync(TaskItem item)
        {
            var existingTask = await _taskManagerDBContext.TaskItems.FirstOrDefaultAsync(i => i.Id == item.Id);
            if (existingTask != null)
            {
                existingTask.Title = item.Title;
                existingTask.Description = item.Description;
                existingTask.DueDate = item.DueDate;
                existingTask.StatusId = item.StatusId;
                existingTask.UserId = item.UserId;

                _taskManagerDBContext.Update(existingTask);
                await _taskManagerDBContext.SaveChangesAsync();
                return item;
            }
            else
            {
                // Optionally: handle case when task not found
                throw new Exception($"Task with ID {item.Id} not found");
            }
         
        }
    }
}
