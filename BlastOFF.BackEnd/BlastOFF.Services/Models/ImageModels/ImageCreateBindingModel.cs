namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel.DataAnnotations;

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