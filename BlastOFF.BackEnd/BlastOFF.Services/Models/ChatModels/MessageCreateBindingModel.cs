namespace BlastOFF.Services.Models.ChatModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class MessageCreateBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }
                
        [Required]
        [DisplayName("Receiver Id")]
        public string ReceiverId { get; set; }
    }
}