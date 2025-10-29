using System.ComponentModel.DataAnnotations;

namespace MovieLibrary.Domain.Entities
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public string Name { get; set; } = null!;

        // Navigation properties
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Series> Series { get; set; } = new List<Series>();
    }
}