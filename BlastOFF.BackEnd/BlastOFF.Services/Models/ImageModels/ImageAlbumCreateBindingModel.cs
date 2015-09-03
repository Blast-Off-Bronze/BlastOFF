namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel.DataAnnotations;

    public class ImageAlbumCreateBindingModel
    {
        [Required]
        public string Title { get; set; }
    }
}