using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Series;

public class CreateSeriesDto
{
  [Required, MaxLength(100)]
  public string Title { get; set; } = null!;

  [MaxLength(500)]
  public string? Description { get; set; }

  public int? ReleaseYear { get; set; }
  public int? SeasonsCount { get; set; }
  public int? EpisodesCount { get; set; }

  [MaxLength(255)]
  public string? ImageUrl { get; set; }

  public List<int> GenreIds { get; set; } = new();
}
