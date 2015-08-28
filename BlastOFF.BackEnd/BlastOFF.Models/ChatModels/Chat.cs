using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Models.ChatModels
{
    public class Chat
    {
        public int Id { get; set; }

        [Required]
        public string ChatRoomName { get; set; }

        [Required]
        public string SenderUsername { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string PostedOn { get; set; }
    }
}