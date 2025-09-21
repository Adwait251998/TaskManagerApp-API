using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.ViewModels
{
    public class TaskItemViewModel
    {
        public int TaskId {  get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public string? Description { get; set; }     = string.Empty;
        public int StatusId { get; set; }
        public string StatusDescription { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
