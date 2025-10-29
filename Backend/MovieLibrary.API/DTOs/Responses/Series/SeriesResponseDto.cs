using MovieLibrary.API.DTOs.Responses.Genre;

namespace MovieLibrary.API.DTOs.Responses.Series;

public class SeriesResponseDto
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string? Description { get; set; }
  public int? ReleaseYear { get; set; }
  public int? SeasonsCount { get; set; }
  public int? EpisodesCount { get; set; }
  public string? ImageUrl { get; set; }
  public List<GenreResponseDto> Genres { get; set; } = new();
}
