using MovieLibrary.API.DTOs.Requests.User;
using MovieLibrary.API.DTOs.Responses.User;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
      var users = await _userRepository.GetAllAsync();
      return users.Select(MapToResponseDto);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
      var user = await _userRepository.GetByIdAsync(id);
      return user == null ? null : MapToResponseDto(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
      // Check if username or email already exists
      var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
      if (existingUser != null)
        throw new ArgumentException("Username already exists");

      existingUser = await _userRepository.GetByEmailAsync(dto.Email);
      if (existingUser != null)
        throw new ArgumentException("Email already exists");

      var user = new User
      {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = HashPassword(dto.Password), // In production, use proper hashing
        Role = dto.Role,
        CreatedAt = DateTime.UtcNow
      };

      var createdUser = await _userRepository.CreateAsync(user);
      return MapToResponseDto(createdUser);
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
        throw new KeyNotFoundException($"User with ID {id} not found");

      if (!string.IsNullOrEmpty(dto.Username))
        user.Username = dto.Username;

      if (!string.IsNullOrEmpty(dto.Email))
        user.Email = dto.Email;

      if (!string.IsNullOrEmpty(dto.Password))
        user.PasswordHash = HashPassword(dto.Password);

      if (!string.IsNullOrEmpty(dto.Role))
        user.Role = dto.Role;

      var updatedUser = await _userRepository.UpdateAsync(user);
      return MapToResponseDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
      return await _userRepository.DeleteAsync(id);
    }

    private static UserResponseDto MapToResponseDto(User user)
    {
      return new UserResponseDto
      {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Role = user.Role,
        CreatedAt = user.CreatedAt
      };
    }

    private static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }
  }
}
