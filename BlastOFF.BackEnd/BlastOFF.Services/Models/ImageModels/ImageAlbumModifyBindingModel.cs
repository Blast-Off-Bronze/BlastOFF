namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ImageAlbumModifyBindingModel
    {
        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }
    }
}