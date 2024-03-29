﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services.AbstractionServices
{
    public interface IAuthorizationServices
    {
        public bool Authorize(string token, string requiredRole);
        bool IsAdmin(string token);
    }
}
