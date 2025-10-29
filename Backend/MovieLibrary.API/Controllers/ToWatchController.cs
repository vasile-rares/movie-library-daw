using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.ToWatch;
using MovieLibrary.API.DTOs.Responses.ToWatch;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class ToWatchController : ControllerBase
  {
    private readonly IToWatchService _toWatchService;

    public ToWatchController(IToWatchService toWatchService)
    {
      _toWatchService = toWatchService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToWatchResponseDto>>> GetAll()
    {
      var toWatchList = await _toWatchService.GetAllToWatchAsync();
      return Ok(toWatchList);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ToWatchResponseDto>> GetById(int id)
    {
      var toWatch = await _toWatchService.GetToWatchByIdAsync(id);
      if (toWatch == null)
        return NotFound();

      return Ok(toWatch);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ToWatchResponseDto>>> GetByUserId(int userId)
    {
      var toWatchList = await _toWatchService.GetToWatchByUserIdAsync(userId);
      return Ok(toWatchList);
    }

    [HttpPost]
    public async Task<ActionResult<ToWatchResponseDto>> Create([FromBody] CreateToWatchDto dto)
    {
      var toWatch = await _toWatchService.CreateToWatchAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = toWatch.Id }, toWatch);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _toWatchService.DeleteToWatchAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
