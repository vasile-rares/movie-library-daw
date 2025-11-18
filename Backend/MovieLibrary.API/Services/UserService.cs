using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using MovieLibrary.API.DTOs.Requests.User;
using MovieLibrary.API.DTOs.Responses.User;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace MovieLibrary.API.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public UserService(IUserRepository userRepository, IMapper mapper, IWebHostEnvironment environment)
    {
      _userRepository = userRepository;
      _mapper = mapper;
      _environment = environment;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
      var users = await _userRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
      var user = await _userRepository.GetByIdAsync(id);
      return user == null ? null : _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
      // Check if nickname or email already exists
      var existingUser = await _userRepository.GetByNicknameAsync(dto.Nickname);
      if (existingUser != null)
        throw new ArgumentException("Nickname already exists");

      existingUser = await _userRepository.GetByEmailAsync(dto.Email);
      if (existingUser != null)
        throw new ArgumentException("Email already exists");

      var user = new User
      {
        Nickname = dto.Nickname,
        Email = dto.Email,
        PasswordHash = HashPassword(dto.Password),
        ProfilePictureUrl = dto.ProfilePictureUrl,
        Role = dto.Role,
        CreatedAt = DateTime.UtcNow
      };

      var createdUser = await _userRepository.CreateAsync(user);
      return _mapper.Map<UserResponseDto>(createdUser);
    }

    public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto)
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
        throw new KeyNotFoundException($"User with ID {id} not found");

      if (!string.IsNullOrEmpty(dto.Nickname))
        user.Nickname = dto.Nickname;

      if (!string.IsNullOrEmpty(dto.Email))
        user.Email = dto.Email;

      if (!string.IsNullOrEmpty(dto.Password))
        user.PasswordHash = HashPassword(dto.Password);

      if (dto.ProfilePictureUrl != null)
        user.ProfilePictureUrl = dto.ProfilePictureUrl;

      if (!string.IsNullOrEmpty(dto.Role))
        user.Role = dto.Role;

      var updatedUser = await _userRepository.UpdateAsync(user);
      return _mapper.Map<UserResponseDto>(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
      return await _userRepository.DeleteAsync(id);
    }

    public async Task<UserResponseDto> UploadProfilePictureAsync(int id, IFormFile file)
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
        throw new KeyNotFoundException($"User with ID {id} not found");

      if (file == null || file.Length == 0)
        throw new ArgumentException("No file uploaded");

      var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
      var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
      if (!allowedExtensions.Contains(extension))
        throw new ArgumentException("Invalid file type. Only .jpg, .jpeg, .png, .gif are allowed.");

      var webRootPath = _environment.WebRootPath;
      if (string.IsNullOrEmpty(webRootPath))
      {
        webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
      }

      var uploadsFolder = Path.Combine(webRootPath, "uploads", "profiles");
      if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

      var fileName = $"{Guid.NewGuid()}{extension}";
      var filePath = Path.Combine(uploadsFolder, fileName);

      using (var stream = new FileStream(filePath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      // Construct the URL. Assuming the frontend can access this via the base URL.
      // We need to make sure the path is accessible.
      // If we use "wwwroot" in path above, we should strip it for the URL if UseStaticFiles is used on wwwroot.
      // Usually WebRootPath points to wwwroot.

      // If WebRootPath is set, it points to wwwroot.
      // So uploadsFolder = wwwroot/uploads/profiles
      // URL should be /uploads/profiles/filename

      var relativePath = $"/uploads/profiles/{fileName}";

      // If there was an old picture (and it's local), maybe delete it?
      // Skipping deletion for now to be safe.

      user.ProfilePictureUrl = relativePath;
      var updatedUser = await _userRepository.UpdateAsync(user);

      return _mapper.Map<UserResponseDto>(updatedUser);
    }

    private static string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }
  }
}
