using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.User;

public class UpdateUserDto
{
  [MaxLength(50)]
  public string? Username { get; set; }

  [EmailAddress, MaxLength(100)]
  public string? Email { get; set; }

  [MinLength(6)]
  public string? Password { get; set; }

  [MaxLength(20)]
  public string? Role { get; set; }
}
