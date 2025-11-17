using MovieLibrary.API.DTOs.Requests.ToWatch;
using MovieLibrary.API.DTOs.Responses.ToWatch;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class ToWatchService : IToWatchService
  {
    private readonly IToWatchRepository _toWatchRepository;

    public ToWatchService(IToWatchRepository toWatchRepository)
    {
      _toWatchRepository = toWatchRepository;
    }

    public async Task<IEnumerable<ToWatchResponseDto>> GetAllToWatchAsync()
    {
      var toWatchList = await _toWatchRepository.GetAllAsync();
      return toWatchList.Select(MapToResponseDto);
    }

    public async Task<ToWatchResponseDto?> GetToWatchByIdAsync(int id)
    {
      var toWatch = await _toWatchRepository.GetByIdAsync(id);
      return toWatch == null ? null : MapToResponseDto(toWatch);
    }

    public async Task<IEnumerable<ToWatchResponseDto>> GetToWatchByUserIdAsync(int userId)
    {
      var toWatchList = await _toWatchRepository.GetByUserIdAsync(userId);
      return toWatchList.Select(MapToResponseDto);
    }

    public async Task<ToWatchResponseDto> CreateToWatchAsync(CreateToWatchDto dto)
    {
      if (!dto.MovieId.HasValue && !dto.SeriesId.HasValue)
        throw new ArgumentException("Either MovieId or SeriesId must be provided");

      if (dto.MovieId.HasValue && dto.SeriesId.HasValue)
        throw new ArgumentException("Cannot add both Movie and Series at the same time");

      var toWatch = new ToWatchList
      {
        UserId = dto.UserId,
        MovieId = dto.MovieId,
        SeriesId = dto.SeriesId,
        AddedAt = DateTime.UtcNow
      };

      var createdToWatch = await _toWatchRepository.CreateAsync(toWatch);
      var toWatchWithDetails = await _toWatchRepository.GetByIdAsync(createdToWatch.Id);
      return MapToResponseDto(toWatchWithDetails!);
    }

    public async Task<bool> DeleteToWatchAsync(int id)
    {
      return await _toWatchRepository.DeleteAsync(id);
    }

    private static ToWatchResponseDto MapToResponseDto(ToWatchList toWatch)
    {
      return new ToWatchResponseDto
      {
        Id = toWatch.Id,
        UserId = toWatch.UserId,
        Username = toWatch.User?.Username ?? "Unknown",
        MovieId = toWatch.MovieId,
        MovieTitle = toWatch.Movie?.Title,
        MovieImageUrl = toWatch.Movie?.ImageUrl,
        SeriesId = toWatch.SeriesId,
        SeriesTitle = toWatch.Series?.Title,
        SeriesImageUrl = toWatch.Series?.ImageUrl,
        AddedAt = toWatch.AddedAt
      };
    }
  }
}
