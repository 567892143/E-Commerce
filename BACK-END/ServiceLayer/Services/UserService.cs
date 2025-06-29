
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Interface;
using ServiceLayer.Interface.Reposiory;
using ServiceLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ServiceLayer.User.Dto;
using Microsoft.Extensions.Logging;

namespace ServiceLayer.Services;

public class UserService : IUserService
{

    private IUserRepository _userRepository;

    private IConfiguration _config;

    private ILogger<UserService> _logger;
    public UserService(IUserRepository userRepository, IConfiguration config, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _config = config;
        _logger = logger;
    }

    public JwtTokenResponseDto GetAccessToken(UserLoginDto userLoginDto)
    {
        _logger.LogInformation("Fetching access token for user with email:{email}",userLoginDto.email);
        IQueryable<Models.User> user = _userRepository.GetAllUsers();
        //  User validUser=user.Where(u=> u.ContactId==Contact.Id)


        string accessToken = GenerateJwtToken(user.FirstOrDefault());
        return new JwtTokenResponseDto{AccessToken=accessToken,RefreshToken=null};
    }
    
    private string GenerateJwtToken(Models.User? user)
{
        var claims = new[]
        {

        new Claim("userId", user.Id.ToString()),
        new Claim("roleId", user.Role.ToString())
    };

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config["JwtConfig:SecretKey"]??""));

    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: _config["JwtConfig:Issuer"],
        audience: _config["JwtConfig:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_config["JwtConfig:ExpiresInMinutes"])),
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}