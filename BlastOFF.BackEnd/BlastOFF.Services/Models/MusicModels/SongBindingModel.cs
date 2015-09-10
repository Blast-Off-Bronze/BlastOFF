namespace BlastOFF.Services.Models.MusicModels
{
    using System;

    public class SongBindingModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string FileDataUrl { get; set; }

        // OPTIONAL
        public string TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public string OriginalDate { get; set; }

        public string Genre { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public string Bpm { get; set; }
    }
}