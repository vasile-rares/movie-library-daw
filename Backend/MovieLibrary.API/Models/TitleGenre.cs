using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("TitleGenres")]
    public class TitleGenre
    {
        public int TitleId { get; set; }
        [ForeignKey("TitleId")]
        public Title Title { get; set; } = null!;

        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; } = null!;
    }
}
