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
            var token=_userService.GetAccessToken(userLoginDto);

            return Ok(token);
        }
    }
}
