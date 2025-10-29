using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("ToWatch")]
    public class ToWatchList
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

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}