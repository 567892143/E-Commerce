
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
using Shared.Services.Exceptions;
using System.Security.Cryptography;

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
        _logger.LogInformation("Fetching access token for user with email:{email}", userLoginDto.email);
        IQueryable<Models.User> user = _userRepository.GetAllUsers();
        IQueryable<Models.Contact> contact = _userRepository.GetAllContacts();


        var userContact = contact.Where(c => c.Email == userLoginDto.email).FirstOrDefault();
        if (userContact == null)
        {
            _logger.LogError("No contact found with email: {Email}", userLoginDto.email);
            throw new NotFoundCustomException("No contact found with email");
        }


        var validUser = user
            .FirstOrDefault(u => u.ContactId == userContact.Id);

        if (validUser == null)
        {
            _logger.LogError("No user found linked to contact with email: {Email}", userLoginDto.email);
            throw new NotFoundCustomException("No user found linked to contact with email");

        }

       bool verification= VerifyPassword(userLoginDto.Password,validUser.Password);
        if (!verification)
        {
            _logger.LogError("Invalid password provided for email : {Email}", userLoginDto.email);
            throw new  Exception("Bad request invalid password");
        }

        string accessToken = GenerateJwtToken(validUser);
        _logger.LogInformation("Fetched access token for user with email:{Email}", userLoginDto.email);
        return new JwtTokenResponseDto { AccessToken = accessToken, RefreshToken = null };
    }
    public void RegisterUser(UserRegisterDto dto)
    {
         IQueryable<Models.Contact> contactList = _userRepository.GetAllContacts();
        var userContact = contactList.Where(c => c.Email == dto.Email).FirstOrDefault();

         if (userContact != null)
        {
            _logger.LogError("Email already exists: {Email}", dto.Email);
            throw new Exception("No contact found with email");
        }

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Mobile = dto.Mobile,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            PostalCode = dto.PostalCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        //var role = __userRepository.GetByName("Customer"); // default role

        var user = new Models.User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Password = HashPassword(dto.Password),
            Role = 1,
            ContactId = contact.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _userRepository.AddNewUser(user, contact);
    }


    public UserDto? GetCurrentUser()
    {
        // This is just a placeholder. In real use, inject IHttpContextAccessor to get UserId from token.
        return null;
    }

    public bool UpdateUser(Guid id, UpdateUserDto dto)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return false;

        var contact = _userRepository.GetContactById(user.ContactId);
        if (contact == null) return false;

        user.Name = dto.Name;
        user.UpdatedAt = DateTime.UtcNow;

        contact.Mobile = dto.Mobile;
        contact.AddressLine1 = dto.AddressLine1;
        contact.AddressLine2 = dto.AddressLine2;
        contact.City = dto.City;
        contact.State = dto.State;
        contact.Country = dto.Country;
        contact.PostalCode = dto.PostalCode;
        contact.UpdatedAt = DateTime.UtcNow;

        _userRepository.UpdateUserContact(user, contact);

        return true;
    }

    public List<UserDto> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            // Email = user.Contact?.Email ?? "",
            // Mobile = user.Contact?.Mobile ?? "",
           // Role = user.Role?.Name ?? "Unknown",
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
        //    LastLogin = user.LastLogin
        }).ToList();
    }

    public bool DeactivateUser(Guid id)
    {
        var user = _userRepository.GetUserById(id);
        if (user == null) return false;

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        _userRepository.Update(user);
        return true;
    }

    // ------------------ Password Helpers ------------------

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        var inputHash = HashPassword(inputPassword);
        return storedHash == inputHash;
    }


    private string GenerateJwtToken(Models.User? user)
    {
        var claims = new[]
        {

        new Claim("userId", user.Id.ToString()),
        new Claim("roleId", user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtConfig:SecretKey"] ?? ""));

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