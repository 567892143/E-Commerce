using ServiceLayer.User.Dto;

namespace ServiceLayer.Interface;

public interface IUserService
{

    JwtTokenResponseDto GetAccessToken(UserLoginDto userLoginDto);

    void RegisterUser(UserRegisterDto dto);
    UserDto? GetCurrentUser(); // Normally from claims
    bool UpdateUser(Guid id, UpdateUserDto dto);
    List<UserDto> GetAllUsers();
    bool DeactivateUser(Guid id);

}