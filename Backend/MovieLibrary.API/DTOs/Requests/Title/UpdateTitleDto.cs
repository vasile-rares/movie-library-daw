using System.ComponentModel.DataAnnotations;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Requests.Title;

public class UpdateTitleDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(1800, 2100)]
    public int? ReleaseYear { get; set; }

    [MaxLength(255)]
    public string? ImageUrl { get; set; }

    [Required]
    public TitleType Type { get; set; }

    // For Series only
    public int? SeasonsCount { get; set; }
    public int? EpisodesCount { get; set; }

    public List<int> GenreIds { get; set; } = new();
}
