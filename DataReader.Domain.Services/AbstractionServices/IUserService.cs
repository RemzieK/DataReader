using DataReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services.AbstractionServices
{
    public interface IUserServices
    {

        Task RegisterUser(string username, string password);
        Task<bool> AuthenticateUser(string username, string password);
        Task<User> GetUserByUsername(string username);
        Task<User> GetByIdAsync(int userId);


    }
}
