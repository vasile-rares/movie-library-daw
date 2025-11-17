using System.ComponentModel.DataAnnotations;
using MovieLibrary.API.Attributes;

namespace MovieLibrary.API.DTOs.Requests.Movie;

public class UpdateMovieDto
{
  [MaxLength(100)]
  public string? Title { get; set; }

  [MaxLength(500)]
  public string? Description { get; set; }

  [Range(1800, 2100)]
  public int? ReleaseYear { get; set; }

  [MaxLength(255), SafeUrl]
  public string? ImageUrl { get; set; }

  public List<int>? GenreIds { get; set; }
}
