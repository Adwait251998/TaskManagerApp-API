using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;

namespace TaskManager.Application.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;
        public UserServices(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

       public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            var hashpassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PhoneNumber= userDTO.PhoneNumber,
                HashPassword = hashpassword,
                
            };
            var createdUser =  await _userRepository.CreateUserAsync(user);
            return new UserDTO
            {

                Name = createdUser.Name,
                Email = createdUser.Email,
                PhoneNumber = userDTO.PhoneNumber
            };
           
        } 
        public async Task<List<UserDTO>> GetUserList()
        {
          
            List<UserDTO> userDTOList = new List<UserDTO>();
            List<User> userList = new List<User>();
            userList = await _userRepository.GetUserListAsync();

            foreach (var user in userList)
            {
                userDTOList.Add(new UserDTO
                {
                     Id = user.Id,
                     Name = user.Name,
                     Email = user.Email,
                     PhoneNumber = user.PhoneNumber
                });
            }
            return userDTOList;
        }


        public async Task<bool> UserExistsAndIsValid(LoginAuth loginDetails)
        {
           
            var userExist = await _userRepository.UserExists(loginDetails.Email);
            var isValidUserCreds = false;
            if(userExist)
            {
                isValidUserCreds = await _userRepository.ValidateUserCreds(loginDetails);
            }
            if(userExist && isValidUserCreds)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
        public async Task <int> GetUserIdByEmail(string email)
       {
            var userId = await _userRepository.GetUserIdByEmailId(email);
            return userId;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = new UserDTO();
            user = await _userRepository.GetUserByEmailId(email);
            return user;
        }
    }
}
