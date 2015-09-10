namespace BlastOFF.Services.Models.ImageModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.UserModels;

    using System;
    using BlastOFF.Models.GalleryModels;

    public class ImageViewModel
    {
        public static ImageViewModel Create(Image model, ApplicationUser currentUser)
        {
            bool liked = false;
            bool owner = false;

            if (currentUser != null)
            {
                if (currentUser.LikedImages.Any(i => i.Id == model.Id))
                {
                    liked = true;
                }

                if (currentUser.Id == model.UploadedById)
                {
                    owner = true;
                }
            }

            var result = new ImageViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                ImageAlbumId = model.ImageAlbumId,
                ImageAlbum = model.ImageAlbum.Title,
                DateAdded = model.DateCreated,
                Likes = model.UserLikes.Take(MainConstants.PageSize)
                .ToList().Select(u => UserPreviewViewModel.Create(u, currentUser)),
                Comments = model.Comments.Take(MainConstants.PageSize)
                .ToList().Select(c => CommentViewModel.Create(c, currentUser)),
                UploadedBy = UserPreviewViewModel.Create(model.UploadedBy, currentUser),
                IsLiked = liked,
                IsMine = owner,
                LikesCount = model.UserLikes.Count,
                CommentsCount = model.Comments.Count
            };

            return result;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int ImageAlbumId { get; set; }

        public string ImageAlbum { get; set; }

        public UserPreviewViewModel UploadedBy { get; set; }

        public string ImageData { get; set; }

        public DateTime DateAdded { get; set; }

        public IEnumerable<UserPreviewViewModel> Likes { get; set; } 

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; } 

        public bool IsLiked { get; set; }

        public bool IsMine { get; set; }
    }
}