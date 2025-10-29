using MovieLibrary.API.DTOs.Requests.ToWatch;
using MovieLibrary.API.DTOs.Responses.ToWatch;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IToWatchService
  {
    Task<IEnumerable<ToWatchResponseDto>> GetAllToWatchAsync();
    Task<ToWatchResponseDto?> GetToWatchByIdAsync(int id);
    Task<IEnumerable<ToWatchResponseDto>> GetToWatchByUserIdAsync(int userId);
    Task<ToWatchResponseDto> CreateToWatchAsync(CreateToWatchDto dto);
    Task<bool> DeleteToWatchAsync(int id);
  }
}


