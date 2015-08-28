namespace BlastOFF.Models.ChatModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using BlastOFF.Models.UserModel;

    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }
    }
}