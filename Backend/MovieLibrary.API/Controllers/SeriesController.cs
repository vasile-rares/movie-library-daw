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
    public async Task<ActionResult<IEnumerable<SeriesResponseDto>>> GetAll()
    {
      var series = await _seriesService.GetAllSeriesAsync();
      return Ok(series);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SeriesResponseDto>> GetById(int id)
    {
      var series = await _seriesService.GetSeriesByIdAsync(id);
      if (series == null)
        return NotFound();

      return Ok(series);
    }

    [HttpGet("genre/{genreId}")]
    public async Task<ActionResult<IEnumerable<SeriesResponseDto>>> GetByGenre(int genreId)
    {
      var series = await _seriesService.GetSeriesByGenreAsync(genreId);
      return Ok(series);
    }

    [HttpPost]
    public async Task<ActionResult<SeriesResponseDto>> Create([FromBody] CreateSeriesDto dto)
    {
      var series = await _seriesService.CreateSeriesAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = series.Id }, series);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SeriesResponseDto>> Update(int id, [FromBody] UpdateSeriesDto dto)
    {
      var series = await _seriesService.UpdateSeriesAsync(id, dto);
      return Ok(series);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _seriesService.DeleteSeriesAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
