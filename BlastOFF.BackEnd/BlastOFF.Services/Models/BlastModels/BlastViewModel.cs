namespace BlastOFF.Services.Models.BlastModels
{
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Models.UserModels;

    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Services.Models.CommentModels;

    using BlastOFF.Services.Constants;

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
                Author = UserPreviewViewModel.Create(model.Author, currentUser),
                Comments = model.Comments.Take(MainConstants.PageSize)
                .Select(c => CommentViewModel.Create(c, currentUser)).ToList(),
                Likes = model.UserLikes.Take(MainConstants.PageSize)
                .Select(u => UserPreviewViewModel.Create(u, currentUser)).ToList(),
                LikesCount = model.UserLikes.Count,
                Id = model.Id,
                IsLiked = liked,
                IsMine = owner,
                CommentsCount = model.Comments.Count
            };

            if(result.Author.ProfileImage == null || result.Author.ProfileImage.Length <= 0)
            {
                result.Author.ProfileImage = "http://www.filecluster.com/howto/wp-content/uploads/2014/07/User-Default.jpg";
            }

            return result;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        public UserPreviewViewModel Author { get; set; }

        public bool IsLiked { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public IEnumerable<UserPreviewViewModel> Likes { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public bool IsMine { get; set; }
    }
}