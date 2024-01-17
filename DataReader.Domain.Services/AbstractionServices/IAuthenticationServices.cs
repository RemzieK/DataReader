using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services.AbstractionServices
{
    public interface IAuthenticationServices
    {
        Task<string?> AuthenticateAsync(string username, string password);
    }
}
