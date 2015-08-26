using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlastOFF.Models.BlastModels;
using BlastOFF.Models.GalleryModels;
using BlastOFF.Models.MusicModels;
using BlastOFF.Models.UserModel;

namespace BlastOFF.Models
{
    public class Comment
    {
        private ICollection<ApplicationUser> likedBy;

        public Comment()
        {
            this.likedBy = new HashSet<ApplicationUser>();    
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int? SongId { get; set; }

        public virtual Song Song { get; set; }

        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public int? BlastId { get; set; }

        public int? GalleryAlbumId { get; set; }

        public virtual GalleryAlbum GalleryAlbum { get; set; }

        public int? MusicAlbumId { get; set; }

        public virtual MusicAlbum MusicAlbum { get; set; }

        public virtual Blast Blast { get; set; }

        public virtual ICollection<ApplicationUser> LikedBy
        {
            get { return this.likedBy; }
            set { this.likedBy = value; }
        } 
    }
}
