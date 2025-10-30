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
    public async Task<ActionResult> GetAll()
    {
      var genres = await _genreService.GetAllGenresAsync();
      return Ok(new { message = "Genres retrieved successfully.", data = genres });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var genre = await _genreService.GetGenreByIdAsync(id);
      if (genre == null)
        return NotFound(new { message = $"Genre with ID {id} not found." });

      return Ok(new { message = "Genre retrieved successfully.", data = genre });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateGenreDto dto)
    {
      var genre = await _genreService.CreateGenreAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = genre.Id }, new { message = "Genre created successfully.", data = genre });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] CreateGenreDto dto)
    {
      var genre = await _genreService.UpdateGenreAsync(id, dto);
      return Ok(new { message = "Genre updated successfully.", data = genre });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _genreService.DeleteGenreAsync(id);
      if (!result)
        return NotFound(new { message = $"Genre with ID {id} not found." });

      return Ok(new { message = "Genre deleted successfully." });
    }
  }
}
