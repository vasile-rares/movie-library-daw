using Microsoft.AspNetCore.Mvc;
using MovieLibrary.API.DTOs.Requests.Auth;
using MovieLibrary.API.DTOs.Responses.Auth;
using MovieLibrary.API.Interfaces.Services;

namespace MovieLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
  {
    var response = await _authService.RegisterAsync(dto);
    return Ok(response);
  }

  [HttpPost("login")]
  public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
  {
    var response = await _authService.LoginAsync(dto);
    return Ok(response);
  }
}
