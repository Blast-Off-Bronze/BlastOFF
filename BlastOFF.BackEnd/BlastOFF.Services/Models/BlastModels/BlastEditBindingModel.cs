namespace BlastOFF.Services.Models.BlastModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class BlastEditBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }
    }
}