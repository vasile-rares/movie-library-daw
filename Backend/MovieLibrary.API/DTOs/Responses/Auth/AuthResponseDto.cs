namespace MovieLibrary.API.DTOs.Responses.Auth;

public class AuthResponseDto
{
  public int UserId { get; set; }
  public string Username { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Role { get; set; } = null!;
  public string Token { get; set; } = null!;
  public DateTime ExpiresAt { get; set; }
}
