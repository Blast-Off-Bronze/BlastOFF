namespace BlastOFF.Services.Models.BlastModels
{
    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class BlastViewModel
    {
        public static Expression<Func<Blast, BlastViewModel>> Create
        {
            get
            {
                return model => new BlastViewModel
                {
                    Content = model.Content,
                    PostedOn = model.PostedOn,
                    BlastType = model.BlastType,
                    Author = model.Author.UserName,
                    Comments = model.Comments.Select(c => new
                    {
                        CommentAuthor = c.Author,
                        CommentedOn = c.PostedOn,
                        Content = c.Content
                    }),
                    LikedBy = model.UserLikes.Select(ul => ul.UserName)
                };
            }
        }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        public string Author { get; set; }

        public IEnumerable<object> Comments { get; set; }

        public IEnumerable<string> LikedBy { get; set; }
    }
}