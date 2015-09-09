namespace BlastOFF.Services.Models.CommentModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class CommentCreateBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }

        public int? BlastId { get; set; }

        public int? ImageAlbumId { get; set; }

        public int? ImageId { get; set; }

        public int? MusicAlbumId { get; set; }

        public int? SongId { get; set; }
    }
}