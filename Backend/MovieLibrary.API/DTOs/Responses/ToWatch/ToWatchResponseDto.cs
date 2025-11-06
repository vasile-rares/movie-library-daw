namespace MovieLibrary.API.DTOs.Responses.ToWatch;

public class ToWatchResponseDto
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Username { get; set; } = null!;
  public int? MovieId { get; set; }
  public string? MovieTitle { get; set; }
  public string? MovieImageUrl { get; set; }
  public int? SeriesId { get; set; }
  public string? SeriesTitle { get; set; }
  public string? SeriesImageUrl { get; set; }
  public DateTime AddedAt { get; set; }
}
