namespace BlastOFF.Services.Models.ChatModels
{
    using System.ComponentModel.DataAnnotations;

    public class MessageModifyBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}