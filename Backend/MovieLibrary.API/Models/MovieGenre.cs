using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
  [Table("MovieGenres")]
  public class MovieGenre
  {
    public int MovieId { get; set; }
    [ForeignKey("MovieId")]
    public Movie Movie { get; set; } = null!;

    public int GenreId { get; set; }
    [ForeignKey("GenreId")]
    public Genre Genre { get; set; } = null!;
  }
}
