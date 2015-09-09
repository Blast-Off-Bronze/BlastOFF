namespace BlastOFF.Services.Models.MusicModels
{
    using System.ComponentModel;

    public class MusicAlbumBindingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string CoverImageData { get; set; }

        [DefaultValue(1)]
        public bool IsPublic { get; set; }
    }
}