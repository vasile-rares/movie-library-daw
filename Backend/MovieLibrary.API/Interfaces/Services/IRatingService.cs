using MovieLibrary.API.DTOs.Requests.Rating;
using MovieLibrary.API.DTOs.Responses.Rating;

namespace MovieLibrary.API.Interfaces.Services
{
  public interface IRatingService
  {
    Task<IEnumerable<RatingResponseDto>> GetAllRatingsAsync();
    Task<RatingResponseDto?> GetRatingByIdAsync(int id);
    Task<IEnumerable<RatingResponseDto>> GetRatingsByUserIdAsync(int userId);
    Task<IEnumerable<RatingResponseDto>> GetRatingsByTitleIdAsync(int titleId);
    Task<RatingResponseDto> CreateRatingAsync(CreateRatingDto dto);
    Task<RatingResponseDto> UpdateRatingAsync(int id, UpdateRatingDto dto);
    Task<bool> DeleteRatingAsync(int id);
  }
}


