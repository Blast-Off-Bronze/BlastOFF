using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ChatModels
{
    public class ChatBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string ChatRoomName { get; set; }

        [Required]
        public string ReceiverUsername { get; set; }
    }
}