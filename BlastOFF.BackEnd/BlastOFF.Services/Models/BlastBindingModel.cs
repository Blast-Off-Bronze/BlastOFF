namespace BlastOFF.Services.Models.BlastModels
{
    using System.ComponentModel.DataAnnotations;

    using BlastOFF.Models.Enumerations;
    
    public class BlastBindingModel
    {
        [Required]
        public string Content { get; set; }

        public BlastType BlastType { get; set; }
    }
}