using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IStatusRepository
    {
        public Task<List<Status>> GetStatuses();
    }
}
