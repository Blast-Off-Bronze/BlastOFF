namespace BlastOFF.Models.GalleryModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using BlastOFF.Models.UserModel;

    public class Image
    {
        private ICollection<Comment> comments;

        private ICollection<ApplicationUser> userLikes;

        public Image()
        {
            this.comments = new HashSet<Comment>();
            this.userLikes = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ImageData { get; set; }

        [Required]
        public int ImageAlbumId { get; set; }

        public virtual ImageAlbum ImageAlbum { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public string UploadedById { get; set; }

        public virtual ApplicationUser UploadedBy { get; set; }

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
                return this.userLikes;
            }

            set
            {
                this.userLikes = value;
            }
        }
    }
}