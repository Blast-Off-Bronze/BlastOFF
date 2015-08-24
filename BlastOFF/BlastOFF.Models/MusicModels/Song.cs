namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int? TrackNumber { get; set; }

        public string SongAlbum { get; set; }

        public DateTime? Year { get; set; }

        public string Genre { get; set; }

        public string Comment { get; set; }

        public string AlbumArtist { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }
    }
}