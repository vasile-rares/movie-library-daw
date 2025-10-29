using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.Domain.Entities
{
    public class Series
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Title { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ReleaseYear { get; set; }

        public int? SeasonsCount { get; set; }
        public int? EpisodesCount { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        public Guid? GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre? Genre { get; set; }

        // Navigation
        public ICollection<ToWatchList> ToWatchList { get; set; } = new List<ToWatchList>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}