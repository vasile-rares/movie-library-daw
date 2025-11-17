using System.ComponentModel.DataAnnotations;
using MovieLibrary.API.Attributes;

namespace MovieLibrary.API.DTOs.Requests.Series;

public class CreateSeriesDto
{
  [Required, MaxLength(100)]
  public string Title { get; set; } = null!;

  [MaxLength(500)]
  public string? Description { get; set; }

  [Range(1800, 2100)]
  public int? ReleaseYear { get; set; }

  [Range(1, 100)]
  public int? SeasonsCount { get; set; }

  [Range(1, 10000)]
  public int? EpisodesCount { get; set; }

  [MaxLength(255), SafeUrl]
  public string? ImageUrl { get; set; }

  public List<int> GenreIds { get; set; } = new();
}
