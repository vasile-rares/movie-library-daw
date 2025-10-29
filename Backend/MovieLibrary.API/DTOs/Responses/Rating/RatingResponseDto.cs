namespace MovieLibrary.API.DTOs.Responses.Rating;

public class RatingResponseDto
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Username { get; set; } = null!;
  public int? MovieId { get; set; }
  public string? MovieTitle { get; set; }
  public int? SeriesId { get; set; }
  public string? SeriesTitle { get; set; }
  public int Score { get; set; }
  public string? Comment { get; set; }
  public DateTime CreatedAt { get; set; }
}
