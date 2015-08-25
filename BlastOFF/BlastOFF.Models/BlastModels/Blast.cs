using BlastOFF.Models.Enumerations;

namespace BlastOFF.Models.BlastModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using UserModel;

    public class Blast
    {
        private ICollection<Comment> comments;
        private ICollection<ApplicationUser> usersLikes;

        public Blast()
        {
            this.comments = new HashSet<Comment>();
            this.usersLikes = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        public int AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<ApplicationUser> UserLikes
        {
            get { return this.usersLikes; }

            set { this.usersLikes = value; }
        }
    }
}
