namespace BlastOFF.Services.Models.CommentModels
{
    using System.ComponentModel.DataAnnotations;

    public class CommentEditBindingModel
    {
        [Required]
        public string Content { get; set; }
    }
}