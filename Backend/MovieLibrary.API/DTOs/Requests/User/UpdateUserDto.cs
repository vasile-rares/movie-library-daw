using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.User;

public class UpdateUserDto
{
  [MaxLength(30), MinLength(3)]
  [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Nickname can only contain letters, numbers, and underscores")]
  public string? Nickname { get; set; }

  [EmailAddress, MaxLength(100)]
  public string? Email { get; set; }

  [MinLength(6)]
  public string? Password { get; set; }

  [MaxLength(255)]
  public string? ProfilePictureUrl { get; set; }

  [MaxLength(20)]
  public string? Role { get; set; }
}
