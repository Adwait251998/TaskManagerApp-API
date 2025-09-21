using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController:ControllerBase
    {
        private readonly StatusService _statusService;
        public StatusController(StatusService statusService) {
            _statusService = statusService;
        }

        [Authorize]

        [HttpGet("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
           var statusList = await _statusService.GetStatusList();
            return new JsonResult(statusList);
        }
    }
}
