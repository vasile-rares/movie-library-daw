using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Genre;

public class CreateGenreDto
{
  [Required, MaxLength(50)]
  public string Name { get; set; } = null!;
}
