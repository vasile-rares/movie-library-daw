using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Title;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class TitlesController : ControllerBase
  {
    private readonly ITitleService _titleService;

    public TitlesController(ITitleService titleService)
    {
      _titleService = titleService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
      var titles = await _titleService.GetAllAsync();
      return Ok(new { message = "Titles retrieved successfully.", data = titles });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var title = await _titleService.GetByIdAsync(id);
      if (title == null)
        return NotFound(new { message = $"Title with ID {id} not found." });

      return Ok(new { message = "Title retrieved successfully.", data = title });
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult> GetByType(TitleType type)
    {
      var titles = await _titleService.GetByTypeAsync(type);
      return Ok(new { message = "Titles retrieved successfully.", data = titles });
    }

    [HttpGet("genre/{genreId}")]
    public async Task<ActionResult> GetByGenre(int genreId)
    {
      var titles = await _titleService.GetByGenreAsync(genreId);
      return Ok(new { message = "Titles retrieved successfully.", data = titles });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTitleDto dto)
    {
      var title = await _titleService.CreateAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = title.Id }, new { message = "Title created successfully.", data = title });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateTitleDto dto)
    {
      var title = await _titleService.UpdateAsync(id, dto);
      return Ok(new { message = "Title updated successfully.", data = title });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _titleService.DeleteAsync(id);
      if (!result)
        return NotFound(new { message = $"Title with ID {id} not found." });

      return Ok(new { message = "Title deleted successfully." });
    }
  }
}
