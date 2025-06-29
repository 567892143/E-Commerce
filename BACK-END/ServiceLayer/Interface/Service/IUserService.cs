using ServiceLayer.User.Dto;

namespace ServiceLayer.Interface;
public interface IUserService
{
  
    JwtTokenResponseDto GetAccessToken(UserLoginDto userLoginDto);

}