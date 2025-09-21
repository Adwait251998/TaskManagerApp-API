using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Infastructure.Repositories
{
    public class StatusRepository:IStatusRepository
    {

        private readonly TaskManagerDBContext _taskManagerDBContext;
        public  StatusRepository(TaskManagerDBContext taskManagerDBContext)
        {
            _taskManagerDBContext = taskManagerDBContext;
        }

        public async Task<List<Status>> GetStatuses()
        {
            List<Status> statuses = new List<Status>();
            statuses = await _taskManagerDBContext.Statuses.ToListAsync();
            return statuses;
        }
    }
}
