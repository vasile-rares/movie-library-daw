using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Series;
using MovieLibrary.API.DTOs.Responses.Series;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class SeriesController : ControllerBase
  {
    private readonly ISeriesService _seriesService;

    public SeriesController(ISeriesService seriesService)
    {
      _seriesService = seriesService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
      var series = await _seriesService.GetAllSeriesAsync();
      return Ok(new { message = "Series retrieved successfully.", data = series });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var series = await _seriesService.GetSeriesByIdAsync(id);
      if (series == null)
        return NotFound(new { message = $"Series with ID {id} not found." });

      return Ok(new { message = "Series retrieved successfully.", data = series });
    }

    [HttpGet("genre/{genreId}")]
    public async Task<ActionResult> GetByGenre(int genreId)
    {
      var series = await _seriesService.GetSeriesByGenreAsync(genreId);
      return Ok(new { message = "Series retrieved successfully.", data = series });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateSeriesDto dto)
    {
      var series = await _seriesService.CreateSeriesAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = series.Id }, new { message = "Series created successfully.", data = series });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateSeriesDto dto)
    {
      var series = await _seriesService.UpdateSeriesAsync(id, dto);
      return Ok(new { message = "Series updated successfully.", data = series });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _seriesService.DeleteSeriesAsync(id);
      if (!result)
        return NotFound(new { message = $"Series with ID {id} not found." });

      return Ok(new { message = "Series deleted successfully." });
    }
  }
}
