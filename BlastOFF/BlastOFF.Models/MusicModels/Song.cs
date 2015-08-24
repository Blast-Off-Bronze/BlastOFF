namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Song
    {
        private ICollection<SongLike> songLikes;

        public Song()
        {
            this.songLikes = new HashSet<SongLike>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public int MusicAlbumId { get; set; }

        public virtual MusicAlbum MusicAlbum { get; set; }

        [Required]
        public int ViewsCount { get; set; }

        public virtual ICollection<SongLike> SongLikes
        {
            get
            {
                return this.songLikes;
            }

            set
            {
                this.songLikes = value;
            }
        }

        //// Optional
        public int? TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public DateTime? Year { get; set; }

        public string Genre { get; set; }

        //public Comment Comment { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }
    }
}