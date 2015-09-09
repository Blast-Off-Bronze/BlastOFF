namespace BlastOFF.Services.Models.BlastModels
{
    using System.ComponentModel.DataAnnotations;

    using BlastOFF.Models.Enumerations;

    using System.ComponentModel;

    public class BlastCreateBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }

        [DisplayName("Blast Type")]
        public BlastType BlastType { get; set; }
    }
}