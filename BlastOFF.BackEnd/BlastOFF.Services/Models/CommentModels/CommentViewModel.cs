namespace BlastOFF.Services.Models.CommentModels
{
    using System.Linq;
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Models.UserModels;

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
                Author = UserPreviewViewModel.Create(model.Author, currentUser),
                LikesCount = model.LikedBy.Count,
                IsLiked = liked,
                IsMine = owner
            };

            if (result.Author.ProfileImage == null || result.Author.ProfileImage.Length <= 0)
            {
                result.Author.ProfileImage = "http://www.filecluster.com/howto/wp-content/uploads/2014/07/User-Default.jpg";
            }

            return result;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public UserPreviewViewModel Author { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public bool IsMine { get; set; }
    }
}