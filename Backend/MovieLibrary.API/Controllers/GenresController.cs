using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Genre;
using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class GenresController : ControllerBase
  {
    private readonly IGenreService _genreService;

    public GenresController(IGenreService genreService)
    {
      _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreResponseDto>>> GetAll()
    {
      var genres = await _genreService.GetAllGenresAsync();
      return Ok(genres);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GenreResponseDto>> GetById(int id)
    {
      var genre = await _genreService.GetGenreByIdAsync(id);
      if (genre == null)
        return NotFound();

      return Ok(genre);
    }

    [HttpPost]
    public async Task<ActionResult<GenreResponseDto>> Create([FromBody] CreateGenreDto dto)
    {
      var genre = await _genreService.CreateGenreAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = genre.Id }, genre);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GenreResponseDto>> Update(int id, [FromBody] CreateGenreDto dto)
    {
      var genre = await _genreService.UpdateGenreAsync(id, dto);
      return Ok(genre);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _genreService.DeleteGenreAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
