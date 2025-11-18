namespace MovieLibrary.API.DTOs.Responses.User;

public class UserResponseDto
{
  public int Id { get; set; }
  public string Nickname { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string? ProfilePictureUrl { get; set; }
  public string Role { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
}
