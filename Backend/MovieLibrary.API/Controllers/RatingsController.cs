using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Rating;
using MovieLibrary.API.DTOs.Responses.Rating;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class RatingsController : ControllerBase
  {
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
      _ratingService = ratingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RatingResponseDto>>> GetAll()
    {
      var ratings = await _ratingService.GetAllRatingsAsync();
      return Ok(ratings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RatingResponseDto>> GetById(int id)
    {
      var rating = await _ratingService.GetRatingByIdAsync(id);
      if (rating == null)
        return NotFound();

      return Ok(rating);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<RatingResponseDto>>> GetByUserId(int userId)
    {
      var ratings = await _ratingService.GetRatingsByUserIdAsync(userId);
      return Ok(ratings);
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<IEnumerable<RatingResponseDto>>> GetByMovieId(int movieId)
    {
      var ratings = await _ratingService.GetRatingsByMovieIdAsync(movieId);
      return Ok(ratings);
    }

    [HttpGet("series/{seriesId}")]
    public async Task<ActionResult<IEnumerable<RatingResponseDto>>> GetBySeriesId(int seriesId)
    {
      var ratings = await _ratingService.GetRatingsBySeriesIdAsync(seriesId);
      return Ok(ratings);
    }

    [HttpPost]
    public async Task<ActionResult<RatingResponseDto>> Create([FromBody] CreateRatingDto dto)
    {
      var rating = await _ratingService.CreateRatingAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = rating.Id }, rating);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RatingResponseDto>> Update(int id, [FromBody] UpdateRatingDto dto)
    {
      var rating = await _ratingService.UpdateRatingAsync(id, dto);
      return Ok(rating);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _ratingService.DeleteRatingAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
