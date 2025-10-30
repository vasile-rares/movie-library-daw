namespace MovieLibrary.API.DTOs.Requests.ToWatch;

public class CreateToWatchDto
{
  public int? MovieId { get; set; }
  public int? SeriesId { get; set; }

  // Set internally by the controller from JWT token
  internal int UserId { get; set; }
}
