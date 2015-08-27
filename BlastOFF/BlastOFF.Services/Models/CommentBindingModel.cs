namespace BlastOFF.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public int? BlastId { get; set; }

        public int? ImageAlbumId { get; set; }

        public int? ImageId { get; set; }

        public int? MusicAlbumId { get; set; }

        public int? SongId { get; set; }
    }
}