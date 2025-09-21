using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Infastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerDBContext _taskManager;
        public UserRepository(TaskManagerDBContext taskManager)
        {
            _taskManager = taskManager;
        }
        public async Task<User> CreateUserAsync(User user)
        {

            _taskManager.Users.Add(user);
            await _taskManager.SaveChangesAsync();
            return user; // EF will now populate Id
        }

        public async Task<List<User>> GetUserListAsync()
        {
            List<User> userList = new List<User>();
            userList = await _taskManager.Users.ToListAsync();
            return userList;
        }

        public async Task<bool> UserExists(string UserName)
        {
            var userExits = await _taskManager.Users.FirstOrDefaultAsync(u => u.Email == UserName);
            return userExits != null;
        }

        public async Task<bool> ValidateUserCreds(LoginAuth login)
        {
            var validateUserCreds = false;
            var userExists = await _taskManager.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            var user = await _taskManager.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            var storedPassword = user.HashPassword;
            if (userExists != null)
            {
                validateUserCreds = BCrypt.Net.BCrypt.Verify(login.Password, storedPassword);
            }
            if (userExists != null && validateUserCreds)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<int> GetUserIdByEmailId(string email)
        {

            var userId = await _taskManager.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefaultAsync(); // returns 0 if not found
            return userId;
        }

        public async Task<UserDTO?> GetUserByEmailId(string email)
        {
            var user = new User();
            user = await _taskManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if(user != null)
            {
                var userDTO = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email= user.Email,
                    PhoneNumber=user.PhoneNumber,

                };
                return userDTO;
            }
            else
            {
                return null;
            }
            
        }
    }
}
