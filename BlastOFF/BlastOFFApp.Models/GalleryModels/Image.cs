using System.ComponentModel.DataAnnotations;

namespace BlastOFFApp.Models.GalleryModels
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int AlbumId { get; set; }

        public virtual Album Album { get; set; }
    }
}
