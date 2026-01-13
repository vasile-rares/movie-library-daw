using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("Titles")]
    public class Title
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ReleaseYear { get; set; }

        [MaxLength(255)]
        public string? ImageUrl { get; set; }

        [Required]
        public TitleType Type { get; set; }

        public int? SeasonsCount { get; set; }
        public int? EpisodesCount { get; set; }

        public ICollection<TitleGenre> TitleGenres { get; set; } = new List<TitleGenre>();
        public ICollection<MyList> MyLists { get; set; } = new List<MyList>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
