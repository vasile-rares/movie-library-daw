using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ReleaseYear { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        // Navigation properties
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<ToWatchList> ToWatchList { get; set; } = new List<ToWatchList>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
