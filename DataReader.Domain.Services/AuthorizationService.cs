﻿using DataReader.Domain.Services.AbstractionServices;
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
        private const string RoleClaimType = "role";
        private const string AdminRole = "Admin";

        public bool IsAdmin(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
            {
                // Token is not a valid JWT
                return false;
            }

            // Check if the role claim exists in the token
            var roleClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == RoleClaimType);

            if (roleClaim == null)
            {
                // Role claim not found
                return false;
            }

            // Check if the user has the Admin role
            return roleClaim.Value == AdminRole;
        }
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
