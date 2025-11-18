using System.ComponentModel.DataAnnotations;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Requests.MyList;

public class AddToMyListDto
{
    [Required]
    public int TitleId { get; set; }

    public WatchStatus Status { get; set; } = WatchStatus.PlanToWatch;

    // Set internally by the controller from JWT token
    internal int UserId { get; set; }
}
