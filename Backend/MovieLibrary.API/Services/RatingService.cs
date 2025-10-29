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

    public RatingService(IRatingRepository ratingRepository)
    {
      _ratingRepository = ratingRepository;
    }

    public async Task<IEnumerable<RatingResponseDto>> GetAllRatingsAsync()
    {
      var ratings = await _ratingRepository.GetAllAsync();
      return ratings.Select(MapToResponseDto);
    }

    public async Task<RatingResponseDto?> GetRatingByIdAsync(int id)
    {
      var rating = await _ratingRepository.GetByIdAsync(id);
      return rating == null ? null : MapToResponseDto(rating);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsByUserIdAsync(int userId)
    {
      var ratings = await _ratingRepository.GetByUserIdAsync(userId);
      return ratings.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsByMovieIdAsync(int movieId)
    {
      var ratings = await _ratingRepository.GetByMovieIdAsync(movieId);
      return ratings.Select(MapToResponseDto);
    }

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsBySeriesIdAsync(int seriesId)
    {
      var ratings = await _ratingRepository.GetBySeriesIdAsync(seriesId);
      return ratings.Select(MapToResponseDto);
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
        CreatedAt = DateTime.Now
      };

      var createdRating = await _ratingRepository.CreateAsync(rating);
      var ratingWithDetails = await _ratingRepository.GetByIdAsync(createdRating.Id);
      return MapToResponseDto(ratingWithDetails!);
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
      return MapToResponseDto(ratingWithDetails!);
    }

    public async Task<bool> DeleteRatingAsync(int id)
    {
      return await _ratingRepository.DeleteAsync(id);
    }

    private static RatingResponseDto MapToResponseDto(Rating rating)
    {
      return new RatingResponseDto
      {
        Id = rating.Id,
        UserId = rating.UserId,
        Username = rating.User.Username,
        MovieId = rating.MovieId,
        MovieTitle = rating.Movie?.Title,
        SeriesId = rating.SeriesId,
        SeriesTitle = rating.Series?.Title,
        Score = rating.Score,
        Comment = rating.Comment,
        CreatedAt = rating.CreatedAt
      };
    }
  }
}
