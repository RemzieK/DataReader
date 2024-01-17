using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using DataReader.Domain.Services.AbstractionServices;


namespace DataReader.Domain.Services
{

    public class AuthenticationService : IAuthenticationServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtServices _jwtService;
        private readonly IPasswordHashing _passwordHashing;
        public AuthenticationService(IUserRepository userRepository, IJwtServices jwtService, IPasswordHashing passwordHashing)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHashing = passwordHashing;
        }
        public async Task<string?> AuthenticateAsync(string UserName, string UserPassword)
        {
            var user = await _userRepository.GetUserByUsernameAndPasswordAsync(UserName, UserPassword);

            if (user == null)
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user.UserId, user.Username);

            return token;
        }
        public async Task<bool> RegisterAsync(string userName, string userPassword)
        {
            var existingUser = await _userRepository.GetByUsername(userName);
            if (existingUser != null)
            {
                return false;
            }
            string hashedPassword = _passwordHashing.HashPassword(userPassword);

            var newUser = new User { Username = userName, Password = hashedPassword };
            await _userRepository.CreateAsync(newUser);

            return true;
        }
        public async Task<string?> LoginAsync(string userName, string userPassword)
        {
            var user = await _userRepository.GetUserByUsernameAndPasswordAsync(userName, _passwordHashing.HashPassword(userPassword));

            if (user == null)
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user.UserId, user.Username);
            return token;
        }
    }
}
