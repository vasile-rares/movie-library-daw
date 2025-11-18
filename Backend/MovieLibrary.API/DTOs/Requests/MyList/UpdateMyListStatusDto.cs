using System.ComponentModel.DataAnnotations;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.DTOs.Requests.MyList;

public class UpdateMyListStatusDto
{
    [Required]
    public WatchStatus Status { get; set; }
}
