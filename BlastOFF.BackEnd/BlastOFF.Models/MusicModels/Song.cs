﻿namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using UserModel;

    public class Song
    {
        private ICollection<ApplicationUser> usersLikes;
        private ICollection<Comment> comments;

        public Song()
        {
            this.usersLikes = new HashSet<ApplicationUser>();
            this.comments = new HashSet<Comment>();
            this.ViewsCount = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }

        public DateTime DateAdded { get; set; }

        public int ViewsCount { get; set; }

        public int? MusicAlbumId { get; set; }

        public virtual MusicAlbum MusicAlbum { get; set; }

        public string UploaderId { get; set; }

        public virtual ApplicationUser Uploader { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get
            {
                return this.comments;
            }

            set
            {
                this.comments = value;
            }
        }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get
            {
                return this.usersLikes;
            }

            set
            {
                this.usersLikes = value;
            }
        }

        //// OPTIONAL
        public int? TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public DateTime? OriginalDate { get; set; }

        public string Genre { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }
    }
}