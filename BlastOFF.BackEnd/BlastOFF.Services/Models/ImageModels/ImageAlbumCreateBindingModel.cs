namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel.DataAnnotations;

    using System.ComponentModel;

    public class ImageAlbumCreateBindingModel
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }
    }
}