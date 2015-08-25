namespace BlastOFF.Models.BlastModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using UserModel;

    public class BlastComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey("Blast")]
        public int BlastId { get; set; }                

        public virtual Blast Blast { get; set; }
    }
}
