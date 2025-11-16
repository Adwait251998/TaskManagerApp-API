using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Core.Entities;
using Twilio.TwiML.Voice;

namespace TaskManager.Application.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        public UserServices(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            var hashpassword = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PhoneNumber = userDTO.PhoneNumber,
                HashPassword = hashpassword,

            };
            var createdUser = await _userRepository.CreateUserAsync(user);
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
            if (userExist)
            {
                isValidUserCreds = await _userRepository.ValidateUserCreds(loginDetails);
            }
            if (userExist && isValidUserCreds)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<int> GetUserIdByEmail(string email)
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

        public async Task<bool> ForgotPassword(string emailId)
        {
            // 1. Get user by email (Identity handles this)
            var user = await _userRepository.GetUserByEmailId(emailId);

            if (user == null)
                return false;

            // 2. Generate reset token
          

           var token=   await _userRepository.SaveForgetPasswordToken(emailId);




            // 3. Create reset password deep link
            var resetLink = $"http://localhost:3000/reset-password?email={emailId}&token={token}";

            // 4. Email content
            var emailBody = $@"
               <div style='font-size:20px;font-weight:bold;color:black;margin-bottom:20px'>
                   Click the link below to reset your password.
               </div>
               <a href='{resetLink}' style='font-size:14px;color:blue;'>Reset Password</a>";

            // 5. Send email
            return await _emailService.SendEmailAsync(emailId, "Reset Password", emailBody);
        }

        public async Task<bool> ResetPassword (ResetPasswordDTO resetDTO)
        {
            var isTokenValid =  await _userRepository.CheckTokenValidityForResetPassword(resetDTO.token, resetDTO.emailId);
            if(isTokenValid)
            {
                var hashpassword = BCrypt.Net.BCrypt.HashPassword(resetDTO.newPassword);
                await  _userRepository.MarkTokenAsExpired(resetDTO.emailId, resetDTO.token);
                return await _userRepository.UpdatePassword(resetDTO.emailId, hashpassword);
            }
            else
            {
                return false;
            }

        }
      
    }
}
