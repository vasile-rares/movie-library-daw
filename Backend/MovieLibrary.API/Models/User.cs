using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
  [Table("Users")]
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Nickname can only contain letters, numbers, and underscores")]
    public string Nickname { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = null!;

    [MaxLength(255)]
    public string? ProfilePictureUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(20)]
    public string Role { get; set; } = "User";

    // Navigation properties
    public ICollection<MyList> MyLists { get; set; } = new List<MyList>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
  }
}
