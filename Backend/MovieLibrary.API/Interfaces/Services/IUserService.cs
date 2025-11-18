using MovieLibrary.API.DTOs.Requests.User;
using MovieLibrary.API.DTOs.Responses.User;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IUserService
  {
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto);
    Task<UserResponseDto> UploadProfilePictureAsync(int id, IFormFile file);
    Task<bool> DeleteUserAsync(int id);
  }
}


