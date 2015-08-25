﻿using BlastOFF.Models.UserModel;

namespace BlastOFF.Models.GalleryModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GalleryAlbum
    {
        private ICollection<ApplicationUser> userLikes;
        private ICollection<ApplicationUser> followers;
        private ICollection<Image> images;

        public GalleryAlbum()
        {
            this.images = new HashSet<Image>();
            this.userLikes = new HashSet<ApplicationUser>();
            this.followers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }

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
    }
}