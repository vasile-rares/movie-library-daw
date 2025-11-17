using AutoMapper;
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
    private readonly IMapper _mapper;

    public ToWatchService(IToWatchRepository toWatchRepository, IMapper mapper)
    {
      _toWatchRepository = toWatchRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ToWatchResponseDto>> GetAllToWatchAsync()
    {
      var toWatchList = await _toWatchRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<ToWatchResponseDto>>(toWatchList);
    }

    public async Task<ToWatchResponseDto?> GetToWatchByIdAsync(int id)
    {
      var toWatch = await _toWatchRepository.GetByIdAsync(id);
      return toWatch == null ? null : _mapper.Map<ToWatchResponseDto>(toWatch);
    }

    public async Task<IEnumerable<ToWatchResponseDto>> GetToWatchByUserIdAsync(int userId)
    {
      var toWatchList = await _toWatchRepository.GetByUserIdAsync(userId);
      return _mapper.Map<IEnumerable<ToWatchResponseDto>>(toWatchList);
    }

    public async Task<ToWatchResponseDto> CreateToWatchAsync(CreateToWatchDto dto)
    {
      if (!dto.MovieId.HasValue && !dto.SeriesId.HasValue)
        throw new ArgumentException("Either MovieId or SeriesId must be provided");

      if (dto.MovieId.HasValue && dto.SeriesId.HasValue)
        throw new ArgumentException("Cannot add both Movie and Series at the same time");

      // Check if user already has this movie/series in their watch list
      var userWatchList = await _toWatchRepository.GetByUserIdAsync(dto.UserId);

      if (dto.MovieId.HasValue)
      {
        var existingMovie = userWatchList.FirstOrDefault(tw => tw.MovieId == dto.MovieId);
        if (existingMovie != null)
          throw new ArgumentException("This movie is already in your watch list");
      }

      if (dto.SeriesId.HasValue)
      {
        var existingSeries = userWatchList.FirstOrDefault(tw => tw.SeriesId == dto.SeriesId);
        if (existingSeries != null)
          throw new ArgumentException("This series is already in your watch list");
      }

      var toWatch = new ToWatchList
      {
        UserId = dto.UserId,
        MovieId = dto.MovieId,
        SeriesId = dto.SeriesId,
        AddedAt = DateTime.UtcNow
      };

      var createdToWatch = await _toWatchRepository.CreateAsync(toWatch);
      var toWatchWithDetails = await _toWatchRepository.GetByIdAsync(createdToWatch.Id);
      return _mapper.Map<ToWatchResponseDto>(toWatchWithDetails!);
    }

    public async Task<bool> DeleteToWatchAsync(int id)
    {
      return await _toWatchRepository.DeleteAsync(id);
    }
  }
}
