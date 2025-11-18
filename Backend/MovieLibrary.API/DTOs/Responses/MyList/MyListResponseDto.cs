using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Responses.MyList;

public class MyListResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public int TitleId { get; set; }
    public string TitleName { get; set; } = null!;
    public TitleType TitleType { get; set; }
    public string? TitleImageUrl { get; set; }
    public WatchStatus Status { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime? StatusUpdatedAt { get; set; }
}
