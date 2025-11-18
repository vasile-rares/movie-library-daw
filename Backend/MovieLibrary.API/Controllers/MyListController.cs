using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.MyList;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class MyListController : ControllerBase
  {
    private readonly IMyListService _myListService;

    public MyListController(IMyListService myListService)
    {
      _myListService = myListService;
    }

    [HttpGet("my-list")]
    public async Task<ActionResult> GetMyList()
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var myList = await _myListService.GetByUserIdAsync(userId);
      return Ok(new { message = "My list retrieved successfully.", data = myList });
    }

    [HttpPost]
    public async Task<ActionResult> AddToMyList([FromBody] AddToMyListDto dto)
    {
      dto.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var myListItem = await _myListService.AddToMyListAsync(dto);
      return Ok(new { message = "Added to my list successfully.", data = myListItem });
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateStatus(int id, [FromBody] UpdateMyListStatusDto dto)
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var myListItem = await _myListService.GetByIdAsync(id);

      if (myListItem == null)
        return NotFound(new { message = $"My list item with ID {id} not found." });

      if (myListItem.UserId != userId)
        return Forbid();

      var updatedItem = await _myListService.UpdateStatusAsync(id, dto);
      return Ok(new { message = "My list status updated successfully.", data = updatedItem });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromMyList(int id)
    {
      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
      var myListItem = await _myListService.GetByIdAsync(id);

      if (myListItem == null)
        return NotFound(new { message = $"My list item with ID {id} not found." });

      if (myListItem.UserId != userId)
        return Forbid();

      await _myListService.RemoveFromMyListAsync(id);
      return Ok(new { message = "Removed from my list successfully." });
    }
  }
}
