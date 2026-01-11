using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MovieLibrary.API.DTOs.Requests.Auth;
using MovieLibrary.API.DTOs.Responses.Auth;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services;

public class AuthService : IAuthService
{
  private readonly IUserRepository _userRepository;
  private readonly IConfiguration _configuration;

  public AuthService(IUserRepository userRepository, IConfiguration configuration)
  {
    _userRepository = userRepository;
    _configuration = configuration;
  }

  public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
  {
    var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
    if (existingUser != null)
      throw new ArgumentException("User with this email already exists");

    var existingNickname = await _userRepository.GetByNicknameAsync(dto.Nickname);
    if (existingNickname != null)
      throw new ArgumentException("Nickname is already taken");

    var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var user = new User
    {
      Nickname = dto.Nickname,
      Email = dto.Email,
      PasswordHash = passwordHash,
      ProfilePictureUrl = dto.ProfilePictureUrl,
      Role = "User",
      CreatedAt = DateTime.UtcNow
    };

    var createdUser = await _userRepository.CreateAsync(user);

    var token = GenerateJwtToken(createdUser);
    var expirationMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");
    var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

    return new AuthResponseDto
    {
      UserId = createdUser.Id,
      Nickname = createdUser.Nickname,
      Email = createdUser.Email,
      ProfilePictureUrl = createdUser.ProfilePictureUrl,
      Role = createdUser.Role,
      Token = token,
      ExpiresAt = expiresAt
    };
  }

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
  {
    // Find user by email
    var user = await _userRepository.GetByEmailAsync(dto.Email);
    if (user == null)
      throw new UnauthorizedAccessException("Invalid email or password");

    // Verify password
    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
      throw new UnauthorizedAccessException("Invalid email or password");

    // Generate JWT token
    var token = GenerateJwtToken(user);
    var expirationMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");
    var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

    return new AuthResponseDto
    {
      UserId = user.Id,
      Nickname = user.Nickname,
      Email = user.Email,
      ProfilePictureUrl = user.ProfilePictureUrl,
      Role = user.Role,
      Token = token,
      ExpiresAt = expiresAt
    };
  }

  private string GenerateJwtToken(User user)
  {
    var claims = new[]
    {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Nickname),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

    var secretKey = _configuration["JwtSettings:SecretKey"] ?? "";
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var issuer = _configuration["JwtSettings:Issuer"];
    var audience = _configuration["JwtSettings:Audience"];
    var expirationMinutes = _configuration.GetValue<int>("JwtSettings:ExpirationMinutes");

    var token = new JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
