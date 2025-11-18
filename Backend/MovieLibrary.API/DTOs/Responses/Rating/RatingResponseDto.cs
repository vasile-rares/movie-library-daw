using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Responses.Rating;

public class RatingResponseDto
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Username { get; set; } = null!;
  public int TitleId { get; set; }
  public string TitleName { get; set; } = null!;
  public TitleType TitleType { get; set; }
  public int Score { get; set; }
  public string? Comment { get; set; }
  public DateTime CreatedAt { get; set; }
}
