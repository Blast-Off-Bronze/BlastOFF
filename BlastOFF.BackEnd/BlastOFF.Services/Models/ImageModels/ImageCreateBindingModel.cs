namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class ImageCreateBindingModel
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Image Album Id")]
        public int ImageAlbumId { get; set; }

        [Required]
        [DisplayName("Base64 Image String")]
        public string Base64ImageString { get; set; }
    }
}