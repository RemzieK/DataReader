using DataReader.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {

        Task<User> GetByUsername(string username);
        Task CreateAsync(User users);
        Task UpdateAsync(User users);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int ID);
        Task<User> GetUserByUsernameAndPasswordAsync(string username, string password);

    }
}
