using MovieLibrary.API.DTOs.Responses.Genre;

namespace MovieLibrary.API.DTOs.Responses.Movie;

public class MovieResponseDto
{
  public int Id { get; set; }
  public string Title { get; set; } = null!;
  public string? Description { get; set; }
  public int? ReleaseYear { get; set; }
  public string? ImageUrl { get; set; }
  public List<GenreResponseDto> Genres { get; set; } = new();
}
