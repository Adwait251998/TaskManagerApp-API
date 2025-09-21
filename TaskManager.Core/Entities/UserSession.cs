using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskManager.Core.Entities
{
    public class UserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ISession Session => _httpContextAccessor.HttpContext!.Session;
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

      

        public void SetUser(int userId, string userName, string email)
        {

            Session.SetInt32("Id", userId);
            Session.SetString("Name", userName);
            Session.SetString("Email", email);
            Id = userId;
            Name = userName;
            Email = email;
        }
        public bool IsAuthenticated => GetUserId().HasValue || Id > 0;
        public void Clear()
        {
            Id = 0;
            Name = "";
            Email = "";
        }

        public int? GetUserId() => Session.GetInt32("Id");
        public string? GetUserName() => Session.GetString("Name");
        public string? GetUserEmail() => Session.GetString("Email");

       
    }
}
