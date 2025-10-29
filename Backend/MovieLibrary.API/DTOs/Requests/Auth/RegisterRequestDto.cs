using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Auth;

public class RegisterRequestDto
{
  [Required, MaxLength(50)]
  public string Username { get; set; } = null!;

  [Required, EmailAddress, MaxLength(100)]
  public string Email { get; set; } = null!;

  [Required, MinLength(6)]
  public string Password { get; set; } = null!;
}
