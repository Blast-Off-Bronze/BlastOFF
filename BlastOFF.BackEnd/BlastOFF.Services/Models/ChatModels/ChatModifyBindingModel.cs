using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ChatModels
{
    public class ChatModifyBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}