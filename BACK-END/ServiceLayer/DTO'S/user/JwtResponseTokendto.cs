namespace ServiceLayer.User.Dto;

public class JwtTokenResponseDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
