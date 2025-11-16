using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace TaskManager.Application.DTOs
{
    public class ResetPasswordDTO
    {
        public string emailId { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;

    }
}
