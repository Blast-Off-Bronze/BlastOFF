using System;
using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Models.ChatModels
{
    public class Chat
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string SenderUsername { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }

        public DateTime PostedOn { get; set; }

        //[ForeignKey("Sender")]
        //public string SenderId { get; set; }

        //public ApplicationUser Sender { get; set; }


        //[ForeignKey("Receiver")]
        //public string ReceiverId { get; set; }

        //public ApplicationUser Receiver { get; set; }
    }
}