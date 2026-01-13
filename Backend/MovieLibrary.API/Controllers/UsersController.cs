using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.User;
using MovieLibrary.API.DTOs.Responses.User;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
      var users = await _userService.GetAllUsersAsync();
      return Ok(new { message = "Users retrieved successfully.", data = users });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null)
        return NotFound(new { message = $"User with ID {id} not found." });

      return Ok(new { message = "User retrieved successfully.", data = user });
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUserDto dto)
    {
      var user = await _userService.CreateUserAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = user.Id }, new { message = "User created successfully.", data = user });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
      var user = await _userService.UpdateUserAsync(id, dto);
      return Ok(new { message = "User updated successfully.", data = user });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      // Get the user ID from the JWT token
      var userIdClaim = User.FindFirst("userId")?.Value;
      if (userIdClaim == null)
        return Unauthorized(new { message = "User not authenticated." });

      var currentUserId = int.Parse(userIdClaim);
      var userRole = User.FindFirst("role")?.Value;

      // Users can only delete their own account, unless they're an admin
      if (currentUserId != id && userRole != "Admin")
        return Forbid();

      var result = await _userService.DeleteUserAsync(id);
      if (!result)
        return NotFound(new { message = $"User with ID {id} not found." });

      return Ok(new { message = "User deleted successfully." });
    }

    [HttpPost("{id}/profile-picture")]
    public async Task<ActionResult> UploadProfilePicture(int id, IFormFile file)
    {
      try
      {
        var user = await _userService.UploadProfilePictureAsync(id, file);
        return Ok(new { message = "Profile picture updated successfully.", data = user });
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
      catch (ArgumentException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }
  }
}
