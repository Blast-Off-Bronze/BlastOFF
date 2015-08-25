using System.Collections.Generic;
using BlastOFF.Models.UserModel;

namespace BlastOFF.Models.GalleryModels
{
    using System.ComponentModel.DataAnnotations;

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
        public int AlbumId { get; set; }

        public virtual GalleryAlbum Album { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get { return this.userLikes; }
            set { this.userLikes = value; }
        }
    }
}