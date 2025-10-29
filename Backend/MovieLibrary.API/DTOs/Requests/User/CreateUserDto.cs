using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.User;

public class CreateUserDto
{
  [Required, MaxLength(50)]
  public string Username { get; set; } = null!;

  [Required, EmailAddress, MaxLength(100)]
  public string Email { get; set; } = null!;

  [Required, MinLength(6)]
  public string Password { get; set; } = null!;

  [MaxLength(20)]
  public string Role { get; set; } = "User";
}
