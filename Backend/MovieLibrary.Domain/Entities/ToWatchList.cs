using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.Domain.Entities
{
    public class ToWatchList
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public Guid? MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        public Guid? SeriesId { get; set; }
        [ForeignKey("SeriesId")]
        public Series? Series { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;

    }
}