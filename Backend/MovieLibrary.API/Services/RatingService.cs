using AutoMapper;
using MovieLibrary.API.DTOs.Requests.Rating;
using MovieLibrary.API.DTOs.Responses.Rating;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class RatingService : IRatingService
  {
    private readonly IRatingRepository _ratingRepository;
    private readonly IMapper _mapper;

    public RatingService(IRatingRepository ratingRepository, IMapper mapper)
    {
      _ratingRepository = ratingRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<RatingResponseDto>> GetAllRatingsAsync()
    {
      var ratings = await _ratingRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<RatingResponseDto>>(ratings);
    }

    public async Task<RatingResponseDto?> GetRatingByIdAsync(int id)
    {
      var rating = await _ratingRepository.GetByIdAsync(id);
      return rating == null ? null : _mapper.Map<RatingResponseDto>(rating);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsByUserIdAsync(int userId)
    {
      var ratings = await _ratingRepository.GetByUserIdAsync(userId);
      return _mapper.Map<IEnumerable<RatingResponseDto>>(ratings);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsByMovieIdAsync(int movieId)
    {
      var ratings = await _ratingRepository.GetByMovieIdAsync(movieId);
      return _mapper.Map<IEnumerable<RatingResponseDto>>(ratings);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsBySeriesIdAsync(int seriesId)
    {
      var ratings = await _ratingRepository.GetBySeriesIdAsync(seriesId);
      return _mapper.Map<IEnumerable<RatingResponseDto>>(ratings);
    }

    public async Task<RatingResponseDto> CreateRatingAsync(CreateRatingDto dto)
    {
      if (!dto.MovieId.HasValue && !dto.SeriesId.HasValue)
        throw new ArgumentException("Either MovieId or SeriesId must be provided");

      if (dto.MovieId.HasValue && dto.SeriesId.HasValue)
        throw new ArgumentException("Cannot rate both Movie and Series at the same time");

      var rating = new Rating
      {
        UserId = dto.UserId,
        MovieId = dto.MovieId,
        SeriesId = dto.SeriesId,
        Score = dto.Score,
        Comment = dto.Comment,
        CreatedAt = DateTime.UtcNow
      };

      var createdRating = await _ratingRepository.CreateAsync(rating);
      var ratingWithDetails = await _ratingRepository.GetByIdAsync(createdRating.Id);
      return _mapper.Map<RatingResponseDto>(ratingWithDetails!);
    }

    public async Task<RatingResponseDto> UpdateRatingAsync(int id, UpdateRatingDto dto)
    {
      var rating = await _ratingRepository.GetByIdAsync(id);
      if (rating == null)
        throw new KeyNotFoundException($"Rating with ID {id} not found");

      if (dto.Score.HasValue)
        rating.Score = dto.Score.Value;

      if (dto.Comment != null)
        rating.Comment = dto.Comment;

      var updatedRating = await _ratingRepository.UpdateAsync(rating);
      var ratingWithDetails = await _ratingRepository.GetByIdAsync(updatedRating.Id);
      return _mapper.Map<RatingResponseDto>(ratingWithDetails!);
    }

    public async Task<bool> DeleteRatingAsync(int id)
    {
      return await _ratingRepository.DeleteAsync(id);
    }
  }
}
