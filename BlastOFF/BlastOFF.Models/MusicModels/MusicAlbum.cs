namespace BlastOFF.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class MusicAlbum
    {
        private ICollection<MusicAlbumLike> musicAlbumLikes;
        private ICollection<Song> songs;
        ////private ICollection<ApplicationUser> followers;

        public MusicAlbum()
        {
            this.musicAlbumLikes = new HashSet<MusicAlbumLike>();
            this.songs = new HashSet<Song>();
            ////this.followers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        ////[Required]
        ////public string CreatedById { get; set; }

        ////public virtual ApplicationUser CreatedBy { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public int ViewsCount { get; set; }

        public virtual ICollection<MusicAlbumLike> MusicAlbumLikes
        {
            get
            {
                return this.musicAlbumLikes;
            }

            set
            {
                this.musicAlbumLikes = value;
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

        //public virtual ICollection<ApplicationUser> Followers
        //{
        //    get { return this.followers; }
        //    set { this.followers = value; }
        //}

        //// Optional
        public string CoverImageData { get; set; }
        
        //public Comment Comment { get; set; }
    }
}