using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Interface;
using ServiceLayer.User.Dto;

namespace YourApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult<JwtTokenResponseDto> UserLogin([FromBody] UserLoginDto userLoginDto)
        {
            var token = _userService.GetAccessToken(userLoginDto);

            return Ok(token);
        }
        

          // 📝 Register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto dto)
        {
            _userService.RegisterUser(dto);
            return Ok(new { message = "User registered successfully." });
        }

        // 👤 Get current user
        [HttpGet("me")]
        public ActionResult<UserDto> GetCurrentUser()
        {
            var user = _userService.GetCurrentUser(); // You may need UserId from token/claims
            return user == null ? NotFound() : Ok(user);
        }

        // ✏️ Update user by id
        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            var updated = _userService.UpdateUser(id, dto);
            return updated ? Ok(new { message = "User updated." }) : NotFound(new { message = "User not found." });
        }

        // 🧑‍💼 Get all users (Admin)
        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        // ❌ Deactivate user (Admin)
        [HttpDelete("{id}")]
        public IActionResult DeactivateUser(Guid id)
        {
            var result = _userService.DeactivateUser(id);
            return result ? Ok(new { message = "User deactivated." }) : NotFound(new { message = "User not found." });
        }
    }
}
