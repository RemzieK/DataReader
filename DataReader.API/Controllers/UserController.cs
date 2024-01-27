using DataReader.Domain.Entities;
using DataReader.Domain.Interfaces;
using DataReader.Domain.Services.AbstractionServices;
using Microsoft.AspNetCore.Mvc;

namespace DataReader.API.Controllers
{
    [Route("apiUserController")]
    [ApiController]
    public class UserController : BaseController<User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationServices _authenticationService;

        public UserController(IUserRepository userRepository, IAuthenticationServices authenticationService, IRepository<User> baseRepository) : base(baseRepository)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult> CreateUser(User user)
        {
            await _userRepository.CreateAsync(user);
            return Ok(user.UserId);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUser(User user)
        {
            await _userRepository.UpdateAsync(user);
            return Ok(user.UserId);
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("GetUserByNameAndPassword")]
        public async Task<ActionResult<User>> GetUserByNameAndPassword(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAndPasswordAsync(username, password);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("User not found");
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<bool>> Register(string username, string password)
        {
            var result = await _authenticationService.RegisterAsync(username, password);
            return Ok(result);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string?>> Login(string username, string password)
        {
            var token = await _authenticationService.LoginAsync(username, password);
            if (token != null)
            {
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
