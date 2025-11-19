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
  public async Task<ActionResult> Register([FromBody] RegisterRequestDto dto)
  {
    var response = await _authService.RegisterAsync(dto);

    SetTokenCookie(response.Token);

    // Return response without token
    var responseData = new
    {
        response.UserId,
        response.Nickname,
        response.Email,
        response.ProfilePictureUrl,
        response.Role,
        response.ExpiresAt
    };

    return Ok(new { message = "User registered successfully.", data = responseData });
  }

  [HttpPost("login")]
  public async Task<ActionResult> Login([FromBody] LoginRequestDto dto)
  {
    var response = await _authService.LoginAsync(dto);

    SetTokenCookie(response.Token);

    // Return response without token
    var responseData = new
    {
        response.UserId,
        response.Nickname,
        response.Email,
        response.ProfilePictureUrl,
        response.Role,
        response.ExpiresAt
    };

    return Ok(new { message = "Login successful.", data = responseData });
  }

  [HttpPost("logout")]
  public IActionResult Logout()
  {
    Response.Cookies.Delete("jwt");
    return Ok(new { message = "Logged out successfully" });
  }

  private void SetTokenCookie(string token)
  {
    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Secure = false, // Set to true in production with HTTPS
      SameSite = SameSiteMode.Strict, // Works because we use proxy
      Expires = DateTime.UtcNow.AddDays(7)
    };
    Response.Cookies.Append("jwt", token, cookieOptions);
  }
}
