using System.Linq;
using BlastOFF.Models.UserModel;

namespace BlastOFF.Services.Models.CommentModels
{
    using System;

    using BlastOFF.Models;

    public class CommentViewModel
    {
        public static CommentViewModel Create(Comment model, ApplicationUser currentUser)
        {
            bool liked = false;

            bool owner = false;

            if (currentUser != null)
            {
                if (currentUser.LikedComments.Any(c => c.Id == model.Id))
                {
                    liked = true;
                }

                if (currentUser.Id == model.AuthorId)
                {
                    owner = true;
                }
            }

            var result = new CommentViewModel()
            {
                Id = model.Id,
                Content = model.Content,
                PostedOn = model.PostedOn,
                AuthorId = model.AuthorId,
                Author = model.Author.UserName,
                LikesCount = model.LikedBy.Count,
                IsLiked = liked,
                AmITheAuthor = owner
            };

            return result;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public bool AmITheAuthor { get; set; }
    }
}