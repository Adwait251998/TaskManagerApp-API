using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration) {
            _configuration = configuration;
        }


        public string GenerateToken(UserDTO user)
        {

            // Claims = info about the user that will be stored in token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.Name),
                      new Claim("UserName",user.Name),
                 new Claim("Email",user.Email),
                 new Claim("userId",user.Id.ToString()),
                 new Claim("PhoneNumber",user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) // This is unique random GUID for token as a token ID
            };


            //Security key (must match appsettings.json)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); // Convert the the unique jwt key from configuration using cryptographic
         
            var creds =  new SigningCredentials(key,SecurityAlgorithms.HmacSha256); //SymmetricSecurityKey means the same key is used to both sign and validate the token.   
                                                                                    // This is the typical approach with HMAC(HS256) algorithms.(Hash based Authentiocation code)


            var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]);
            //Create Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
