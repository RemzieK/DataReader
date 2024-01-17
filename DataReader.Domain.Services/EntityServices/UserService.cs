using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using DataReader.Domain.Services.AbstractionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader.Domain.Services.EntityServices
{

    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashing _passwordHasher;
        private readonly IAuthorizationServices _authorizationServices;

        public UserService(IUserRepository userRepository, IPasswordHashing passwordHasher, IAuthorizationServices authorizationServices)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authorizationServices = authorizationServices;
        }

        public async Task RegisterUser(string UserName, string UserPassword)
        {
            string hashedPassword = _passwordHasher.HashPassword(UserPassword);

            User User = new User { UserName = UserName, UserPassword = hashedPassword };

            await _userRepository.CreateAsync(User);
        }

        public async Task<bool> AuthenticateUser(string UserName, string UserPassword)
        {
            User user = await _userRepository.GetByUsername(UserName);

            bool passwordIsValid = _passwordHasher.VerifyPassword(UserPassword, user.UserPassword);

            return passwordIsValid;
        }
        public async Task AddUser(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _userRepository.GetByUsername(username);
        }
        public async Task<User> GetByIdAsync(int UserID)
        {
            return await _userRepository.GetByIdAsync(UserID);
        }

    }
}
