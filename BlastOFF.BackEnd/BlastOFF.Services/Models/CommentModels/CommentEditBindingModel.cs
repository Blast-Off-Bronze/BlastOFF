﻿namespace BlastOFF.Services.Models.CommentModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class CommentEditBindingModel
    {
        [Required]
        [DisplayName("Content")]
        public string Content { get; set; }
    }
}