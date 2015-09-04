namespace BlastOFF.Services.Models.ImageModels
{
    using System.ComponentModel;

    public class ImageModifyBindingModel
    {
        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Base64 Image String")]
        public string Base64ImageString { get; set; }
    }
}