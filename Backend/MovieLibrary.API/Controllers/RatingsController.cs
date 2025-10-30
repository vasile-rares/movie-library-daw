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
    public async Task<ActionResult> GetAll()
    {
      var ratings = await _ratingService.GetAllRatingsAsync();
      return Ok(new { message = "Ratings retrieved successfully.", data = ratings });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var rating = await _ratingService.GetRatingByIdAsync(id);
      if (rating == null)
        return NotFound(new { message = $"Rating with ID {id} not found." });

      return Ok(new { message = "Rating retrieved successfully.", data = rating });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult> GetByUserId(int userId)
    {
      var ratings = await _ratingService.GetRatingsByUserIdAsync(userId);
      return Ok(new { message = "Ratings retrieved successfully.", data = ratings });
    }

    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult> GetByMovieId(int movieId)
    {
      var ratings = await _ratingService.GetRatingsByMovieIdAsync(movieId);
      return Ok(new { message = "Ratings retrieved successfully.", data = ratings });
    }

    [HttpGet("series/{seriesId}")]
    public async Task<ActionResult> GetBySeriesId(int seriesId)
    {
      var ratings = await _ratingService.GetRatingsBySeriesIdAsync(seriesId);
      return Ok(new { message = "Ratings retrieved successfully.", data = ratings });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateRatingDto dto)
    {
      var rating = await _ratingService.CreateRatingAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = rating.Id }, new { message = "Rating created successfully.", data = rating });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateRatingDto dto)
    {
      var rating = await _ratingService.UpdateRatingAsync(id, dto);
      return Ok(new { message = "Rating updated successfully.", data = rating });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _ratingService.DeleteRatingAsync(id);
      if (!result)
        return NotFound(new { message = $"Rating with ID {id} not found." });

      return Ok(new { message = "Rating deleted successfully." });
    }
  }
}
