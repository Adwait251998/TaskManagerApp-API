using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.DTOs
{
    public class CreateTaskItemDTO
    {
        public int TaskId { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int StatusId { get; set; }       
        public int UserId { get; set; }
       
        public DateTime? DueDate { get; set; }  
    }
}
