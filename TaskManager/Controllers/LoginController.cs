using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;
using TaskManager.Filters;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController:ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserServices _userServices;

        private readonly ILogger<LoginController> _logger;
        public LoginController(JwtTokenService jwtTokenService, UserServices userServices, ILogger<LoginController> logger)
        {
            _jwtTokenService = jwtTokenService;
            _userServices = userServices;
            _logger = logger;
        }

        [HttpPost("Login")]
        //[ServiceFilter(typeof(UserSessionFilter))]
        public async Task<IActionResult> Login([FromBody] LoginAuth loginDetails)
        {
            _logger.LogInformation("GetCreateTaskDTO {data}:", JsonSerializer.Serialize(loginDetails));
            if (loginDetails == null) return BadRequest("Invalid request");
            var userExists = await _userServices.UserExistsAndIsValid(loginDetails);
            if (userExists)
            {

             
                var loggedInUser = await _userServices.GetUserByEmail(loginDetails.Email);
                var token = _jwtTokenService.GenerateToken(loggedInUser);
                var response = new LoginResponseDto
                {
                    Token = token,
                    User = new UserDTO
                    {
                        Id = loggedInUser.Id,
                        Email = loggedInUser.Email,
                        Name = loggedInUser.Name,
                        PhoneNumber =loggedInUser.PhoneNumber,

                    }
                };
                return Ok(new
                {
                    response
                });
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string emailId)
        {
            var isEmailSent = await _userServices.ForgotPassword(emailId);
             var response = isEmailSent ? "To reset your password an email has been sent to registered Email Id!" : "Invalid EmailId";
            return Ok(new {
                response
            });
        }
    }
}
