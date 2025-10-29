using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Rating;

public class CreateRatingDto
{
  [Required]
  public int UserId { get; set; }

  public int? MovieId { get; set; }
  public int? SeriesId { get; set; }

  [Required, Range(1, 10)]
  public int Score { get; set; }

  [MaxLength(300)]
  public string? Comment { get; set; }
}
