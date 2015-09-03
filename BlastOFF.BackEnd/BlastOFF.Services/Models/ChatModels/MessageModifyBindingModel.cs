using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.ChatModels
{
    public class MessageModifyBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}