using System.ComponentModel;
using BlastOFF.Models.UserModel;

namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Song
    {
        private ICollection<Comment> comments;
        private ICollection<ApplicationUser> usersLikes; 

        public Song()
        {
            this.comments = new HashSet<Comment>();
            this.usersLikes = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }

        public int? MusicAlbumId { get; set; } // TO BE FIXED !!!

        public virtual MusicAlbum MusicAlbum { get; set; }

        [DefaultValue(0)]
        public int ViewsCount { get; set; }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get { return this.usersLikes; }
            set { this.usersLikes = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        //// Optional
        public int? TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public DateTime? Year { get; set; }

        public string Genre { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }
    }
}