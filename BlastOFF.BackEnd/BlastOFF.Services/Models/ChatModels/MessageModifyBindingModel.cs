namespace BlastOFF.Services.Models.ChatModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class MessageModifyBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }
    }
}