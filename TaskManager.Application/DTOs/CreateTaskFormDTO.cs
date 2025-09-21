using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.DTOs
{
    public class CreateTaskFormDTO
    {
        public List<StatusDTO> Statuses { get; set; } = new();
        public UserDTO User { get; set; } = new();
    }
}
