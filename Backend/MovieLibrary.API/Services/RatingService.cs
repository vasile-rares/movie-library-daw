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
    private readonly ITitleRepository _titleRepository;
    private readonly IMapper _mapper;

    public RatingService(IRatingRepository ratingRepository, ITitleRepository titleRepository, IMapper mapper)
    {
      _ratingRepository = ratingRepository;
      _titleRepository = titleRepository;
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

    public async Task<IEnumerable<RatingResponseDto>> GetRatingsByTitleIdAsync(int titleId)
    {
      var ratings = await _ratingRepository.GetByTitleIdAsync(titleId);
      return _mapper.Map<IEnumerable<RatingResponseDto>>(ratings);
    }

    public async Task<RatingResponseDto> CreateRatingAsync(CreateRatingDto dto)
    {
      // Validate that the title exists
      var titleExists = await _titleRepository.ExistsAsync(dto.TitleId);
      if (!titleExists)
        throw new KeyNotFoundException($"Title with ID {dto.TitleId} not found");

      var rating = new Rating
      {
        UserId = dto.UserId,
        TitleId = dto.TitleId,
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
