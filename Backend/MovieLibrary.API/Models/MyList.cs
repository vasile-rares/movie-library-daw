using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLibrary.API.Models
{
    [Table("MyList")]
    public class MyList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int TitleId { get; set; }
        [ForeignKey("TitleId")]
        public Title Title { get; set; } = null!;

        [Required]
        public WatchStatus Status { get; set; } = WatchStatus.PlanToWatch;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public DateTime? StatusUpdatedAt { get; set; }
    }
}
