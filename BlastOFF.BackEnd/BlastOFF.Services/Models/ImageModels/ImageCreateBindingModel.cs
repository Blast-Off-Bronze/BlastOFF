using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ImageModels
{
    public class ImageCreateBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int ImageAlbumId { get; set; }

        [Required]
        public string Base64ImageString { get; set; }
    }
}