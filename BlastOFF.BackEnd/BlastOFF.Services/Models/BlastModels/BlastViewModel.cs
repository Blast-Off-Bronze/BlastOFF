using BlastOFF.Services.Models.CommentModels;

namespace BlastOFF.Services.Models.BlastModels
{
    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BlastViewModel
    {
        public static BlastViewModel Create(Blast model)
        {
            return new BlastViewModel
            {
                Content = model.Content,
                PostedOn = model.PostedOn,
                BlastType = model.BlastType,
                Author = model.Author.UserName,
                Comments = model.Comments.Select(c => CommentViewModel.Create(c)),
                LikedBy = model.UserLikes.Select(ul => ul.UserName)
            };
        }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        public string Author { get; set; }

        public IEnumerable<object> Comments { get; set; }

        public IEnumerable<string> LikedBy { get; set; }
    }
}