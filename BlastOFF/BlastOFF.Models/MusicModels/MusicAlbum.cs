namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using UserModel;

    public class MusicAlbum
    {
        private ICollection<ApplicationUser> followers;

        private ICollection<Song> songs;

        private ICollection<ApplicationUser> userLikes;

        public MusicAlbum()
        {
            this.userLikes = new HashSet<ApplicationUser>();
            this.songs = new HashSet<Song>();
            this.followers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [DefaultValue(0)]
        public int ViewsCount { get; set; }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get
            {
                return this.userLikes;
            }

            set
            {
                this.userLikes = value;
            }
        }

        public virtual ICollection<Song> Songs
        {
            get
            {
                return this.songs;
            }

            set
            {
                this.songs = value;
            }
        }

        public virtual ICollection<ApplicationUser> Followers
        {
            get
            {
                return this.followers;
            }

            set
            {
                this.followers = value;
            }
        }

        //// Optional
        public string CoverImageData { get; set; }
    }
}