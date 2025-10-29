using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.Rating;

public class UpdateRatingDto
{
  [Range(1, 10)]
  public int? Score { get; set; }

  [MaxLength(300)]
  public string? Comment { get; set; }
}
