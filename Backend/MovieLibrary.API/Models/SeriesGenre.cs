using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
  [Table("SeriesGenres")]
  public class SeriesGenre
  {
    public int SeriesId { get; set; }
    [ForeignKey("SeriesId")]
    public Series Series { get; set; } = null!;

    public int GenreId { get; set; }
    [ForeignKey("GenreId")]
    public Genre Genre { get; set; } = null!;
  }
}
