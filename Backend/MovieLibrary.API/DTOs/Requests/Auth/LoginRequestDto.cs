using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Auth;

public class LoginRequestDto
{
  [Required, EmailAddress]
  public string Email { get; set; } = null!;

  [Required, MinLength(6)]
  public string Password { get; set; } = null!;
}
