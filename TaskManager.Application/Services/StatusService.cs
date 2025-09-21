using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Services
{
    public class StatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }


        public async Task<List<StatusDTO>> GetStatusList()
        {
            List<Status> statuses = new List<Status>();

            List<StatusDTO> statusDTOList = new List<StatusDTO>();
            statuses = await _statusRepository.GetStatuses();

            foreach(var status in statuses)
            {
                statusDTOList.Add(new StatusDTO
                {
                    Id = status.Id,
                    Name = status.Name,
                    Description = status.Description,
                });
            }
            return statusDTOList;
        }
    }
}
