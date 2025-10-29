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
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
      var users = await _userService.GetAllUsersAsync();
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(int id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      if (user == null)
        return NotFound();

      return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto dto)
    {
      var user = await _userService.CreateUserAsync(dto);
      return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> Update(int id, [FromBody] UpdateUserDto dto)
    {
      var user = await _userService.UpdateUserAsync(id, dto);
      return Ok(user);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
      var result = await _userService.DeleteUserAsync(id);
      if (!result)
        return NotFound();

      return NoContent();
    }
  }
}
