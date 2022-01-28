using API.Data;
using API.Dtos;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        public UsersController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet("current-user/profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var currentUserId = User.FindFirst("id").Value;
            return Ok(await _userRepository.GetProfileAsync(currentUserId));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            return Ok(new { Token = await _userService.LoginAsync(userLoginDto) });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
        {
            await _userService.RegisterAsync(userRegistrationDto);
            return NoContent();
        }
    }
}