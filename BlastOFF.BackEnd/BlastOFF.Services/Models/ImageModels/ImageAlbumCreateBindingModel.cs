using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ImageModels
{
    public class ImageAlbumCreateBindingModel
    {
        [Required]
        public string Title { get; set; }
    }
}