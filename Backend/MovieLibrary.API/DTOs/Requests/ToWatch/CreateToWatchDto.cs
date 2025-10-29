using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.API.DTOs.Requests.ToWatch;

public class CreateToWatchDto
{
  [Required]
  public int UserId { get; set; }

  public int? MovieId { get; set; }
  public int? SeriesId { get; set; }
}
