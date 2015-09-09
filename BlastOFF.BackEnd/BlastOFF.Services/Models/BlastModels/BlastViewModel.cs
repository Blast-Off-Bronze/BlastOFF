using BlastOFF.Models.UserModel;

namespace BlastOFF.Services.Models.BlastModels
{
    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Services.Models.CommentModels;

    public class BlastViewModel
    {
        public static BlastViewModel Create(Blast model, ApplicationUser currentUser)
        {
            bool liked = false;
            bool owner = false;

            if (currentUser != null)
            {
                if (currentUser.LikedBlasts.Any(b => b.Id == model.Id))
                {
                    liked = true;
                }

                if (currentUser.Id == model.AuthorId)
                {
                    owner = true;
                }
            }

            BlastViewModel result = new BlastViewModel()
            {
                Content = model.Content,
                PostedOn = model.PostedOn,
                BlastType = model.BlastType,
                Author = model.Author.UserName,
                Comments = model.Comments.ToList().Take(3).Select(c => CommentViewModel.Create(c, currentUser)),
                LikesCount = model.UserLikes.Count,
                Id = model.Id,
                IsLiked = liked,
                AmITheAuthor = owner
            };

            return result;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        public string Author { get; set; }

        public bool IsLiked { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public int LikesCount { get; set; }

        public bool AmITheAuthor { get; set; }
    }
}