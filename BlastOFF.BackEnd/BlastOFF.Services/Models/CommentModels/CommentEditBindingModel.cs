using System.ComponentModel.DataAnnotations;

namespace BlastOFF.Services.Models.CommentModels
{
    public class CommentEditBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}