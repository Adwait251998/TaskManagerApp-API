using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserServices _userService;
        public UserController(UserServices userService)
        {
            _userService = userService;
        }
        [HttpPost("CreaterUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("User data is required.");

            // Call the service to create the user
            var createdUser = await _userService.CreateUser(userDTO);

            return Ok(new { Message = "User created successfully." });
        }
        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            List<UserDTO> userList = new List<UserDTO>();
             userList = await _userService.GetUserList();
            return new JsonResult(userList);
        }

        [HttpGet("GetUserIdByEmail")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            var userId = await _userService.GetUserIdByEmail(email);
            return new JsonResult(userId);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]  ResetPasswordDTO resetDTO)
        {
            var passwordUpdated = await _userService.ResetPassword(resetDTO);
            if (passwordUpdated)
            {
                return new JsonResult("Password updated successfully!");
            }
            else
            {
                return new JsonResult("We couldn't update your password. Reset link may be invalid or expired.");
            }
               
        }
    }
}
