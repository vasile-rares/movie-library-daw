using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Movie;
using MovieLibrary.API.DTOs.Responses.Movie;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class MoviesController : ControllerBase
  {
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
      _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieResponseDto>>> GetAll()
    {
      var movies = await _movieService.GetAllMoviesAsync();
      return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieResponseDto>> GetById(int id)
    {
      var movie = await _movieService.GetMovieByIdAsync(id);
      if (movie == null)
        return NotFound();

      return Ok(movie);
    }

    [HttpGet("genre/{genreId}")]
    public async Task<ActionResult<IEnumerable<MovieResponseDto>>> GetByGenre(int genreId)
    {
      var movies = await _movieService.GetMoviesByGenreAsync(genreId);
      return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult<MovieResponseDto>> Create([FromBody] CreateMovieDto dto)
    {
      var movie = await _movieService.CreateMovieAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieResponseDto>> Update(int id, [FromBody] UpdateMovieDto dto)
    {
      var movie = await _movieService.UpdateMovieAsync(id, dto);
      return Ok(movie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _movieService.DeleteMovieAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
