namespace BlastOFF.Models.MusicModels
{
    using System.ComponentModel.DataAnnotations;

    public class SongLike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SongId { get; set; }

        public virtual Song Song { get; set; }

        //// [Required]
        //// public string UserId { get; set; }

        //// public virtual ApplicationUser User { get; set; }
    }
}