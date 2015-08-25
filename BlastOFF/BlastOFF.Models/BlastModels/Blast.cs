namespace BlastOFF.Models.BlastModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using UserModel;

    public class Blast
    {
        private ICollection<BlastComment> comments;

        public Blast()
        {
            this.comments = new HashSet<BlastComment>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<BlastComment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}
