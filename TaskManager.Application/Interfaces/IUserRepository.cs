using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user);

        public Task<List<User>> GetUserListAsync();

        public Task<bool> UserExists(string UserName);

        public Task<bool> ValidateUserCreds(LoginAuth login);

        public Task<int> GetUserIdByEmailId(string email);

        public Task<UserDTO> GetUserByEmailId(string email);

        public Task<string> SaveForgetPasswordToken(string email);

        public Task<bool> CheckTokenValidityForResetPassword(string token, string emailId);

        public Task<bool> UpdatePassword(string emailId, string newPassword);
        public Task MarkTokenAsExpired(string emailId, string token);

    }
}
