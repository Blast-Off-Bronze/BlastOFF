namespace BlastOFF.Services.Models.ChatModels
{
    using System.ComponentModel.DataAnnotations;

    public class MessageCreateBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}