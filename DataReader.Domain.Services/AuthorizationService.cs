using DataReader.Domain.Services.AbstractionServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services
{
    public class AuthorizationService : IAuthorizationServices
    {
        public bool Authorize(string token, string requiredRole)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                throw new Exception("Role not found.");
            }

            return roleClaim.Value == requiredRole;
        }
    }
}
