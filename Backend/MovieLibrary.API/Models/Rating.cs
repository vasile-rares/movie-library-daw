using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("Ratings")]
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int? MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        public int? SeriesId { get; set; }
        [ForeignKey("SeriesId")]
        public Series? Series { get; set; }

        [Range(1, 10)]
        public int Score { get; set; }

        [MaxLength(300)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}