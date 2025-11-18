using MovieLibrary.API.DTOs.Responses.Genre;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Responses.Title;

public class TitleResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? ReleaseYear { get; set; }
    public string? ImageUrl { get; set; }
    public TitleType Type { get; set; }
    public int? SeasonsCount { get; set; }
    public int? EpisodesCount { get; set; }
    public List<GenreResponseDto> Genres { get; set; } = new();
}
