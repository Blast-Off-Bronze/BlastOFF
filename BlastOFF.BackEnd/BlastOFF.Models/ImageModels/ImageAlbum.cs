namespace BlastOFF.Models.GalleryModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using System;
    using BlastOFF.Models.UserModel;

    public class ImageAlbum
    {
        private ICollection<ApplicationUser> userLikes;
        private ICollection<ApplicationUser> followers;
        private ICollection<Image> images;
        private ICollection<Comment> comments; 

        public ImageAlbum()
        {
            this.images = new HashSet<Image>();
            this.userLikes = new HashSet<ApplicationUser>();
            this.followers = new HashSet<ApplicationUser>();
            this.comments = new HashSet<Comment>();
            this.IsPublic = true;
            this.ViewsCount = 0;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

        public bool IsPublic { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public int ViewsCount { get; set; }

        public virtual ICollection<Image> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get { return this.userLikes; }
            set { this.userLikes = value; }
        }

        public virtual ICollection<ApplicationUser> Followers
        {
            get { return this.followers; }
            set { this.followers = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}