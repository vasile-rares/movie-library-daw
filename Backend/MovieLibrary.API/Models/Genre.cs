using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("Genres")]
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        // Navigation properties
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<SeriesGenre> SeriesGenres { get; set; } = new List<SeriesGenre>();
    }
}