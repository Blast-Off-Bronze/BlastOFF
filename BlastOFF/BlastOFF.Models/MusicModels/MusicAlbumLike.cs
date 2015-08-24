namespace BlastOFF.Models.MusicModels
{
    using System.ComponentModel.DataAnnotations;

    public class MusicAlbumLike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MusicAlbumId { get; set; }

        public virtual MusicAlbum MusicAlbum { get; set; }

        //// [Required]
        //// public string UserId { get; set; }

        //// public virtual ApplicationUser User { get; set; }
    }
}