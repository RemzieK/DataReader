using DataReader.Domain.Services.AbstractionServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{

    public class JwtService : IJwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserServices _userServices;
    

        public JwtService(IConfiguration configuration, IUserServices userServices)
        {
            _configuration = configuration;
            _userServices = userServices;
            
        }

        public string GenerateToken(int UserID, string UserName)
        {
            var user = _userServices.GetByIdAsync(UserID).Result;

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: new List<Claim>
                {
            new Claim(ClaimTypes.NameIdentifier, user.ToString()),
            new Claim(ClaimTypes.Name, UserName),
            new Claim(ClaimTypes.Role, user.Role),
                },
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}
