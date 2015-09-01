using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ChatModels
{
    public class ChatCreateBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}