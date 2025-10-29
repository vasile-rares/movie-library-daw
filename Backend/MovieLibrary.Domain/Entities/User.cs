using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Domain.Entities
{
  public class User
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public string Username { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [MaxLength(20)]
    public string Role { get; set; } = "User";

    // Navigation properties
    public ICollection<ToWatchList> ToWatchList { get; set; } = new List<ToWatchList>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
  }
}
