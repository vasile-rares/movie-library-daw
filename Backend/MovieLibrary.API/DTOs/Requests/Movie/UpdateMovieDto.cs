using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Movie;

public class UpdateMovieDto
{
  [MaxLength(100)]
  public string? Title { get; set; }

  [MaxLength(500)]
  public string? Description { get; set; }

  public int? ReleaseYear { get; set; }

  [MaxLength(255)]
  public string? ImageUrl { get; set; }

  public List<int>? GenreIds { get; set; }
}
