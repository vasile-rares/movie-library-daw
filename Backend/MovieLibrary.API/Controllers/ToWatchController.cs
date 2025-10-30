using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.ToWatch;
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

    [HttpGet("my-list")]
    public async Task<ActionResult> GetMyWatchList()
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var toWatchList = await _toWatchService.GetToWatchByUserIdAsync(userId);
      return Ok(new { message = "To-watch list retrieved successfully.", data = toWatchList });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateToWatchDto dto)
    {
      dto.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var toWatch = await _toWatchService.CreateToWatchAsync(dto);
      return Ok(new { message = "Added to watch list successfully.", data = toWatch });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var toWatch = await _toWatchService.GetToWatchByIdAsync(id);

      if (toWatch == null)
        return NotFound(new { message = $"To-watch item with ID {id} not found." });

      if (toWatch.UserId != userId)
        return Forbid();

      await _toWatchService.DeleteToWatchAsync(id);
      return Ok(new { message = "Removed from watch list successfully." });
    }
  }
}
