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
    public async Task<ActionResult> GetAll()
    {
      var toWatchList = await _toWatchService.GetAllToWatchAsync();
      return Ok(new { message = "To-watch list retrieved successfully.", data = toWatchList });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var toWatch = await _toWatchService.GetToWatchByIdAsync(id);
      if (toWatch == null)
        return NotFound(new { message = $"To-watch item with ID {id} not found." });

      return Ok(new { message = "To-watch item retrieved successfully.", data = toWatch });
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult> GetByUserId(int userId)
    {
      var toWatchList = await _toWatchService.GetToWatchByUserIdAsync(userId);
      return Ok(new { message = "To-watch list retrieved successfully.", data = toWatchList });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateToWatchDto dto)
    {
      var toWatch = await _toWatchService.CreateToWatchAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = toWatch.Id }, new { message = "Added to watch list successfully.", data = toWatch });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _toWatchService.DeleteToWatchAsync(id);
      if (!result)
        return NotFound(new { message = $"To-watch item with ID {id} not found." });

      return Ok(new { message = "Removed from watch list successfully." });
    }
  }
}
