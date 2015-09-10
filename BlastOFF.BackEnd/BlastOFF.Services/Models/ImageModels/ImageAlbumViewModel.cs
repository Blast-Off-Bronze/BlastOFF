namespace BlastOFF.Services.Models.ImageModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.GalleryModels;

    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.UserModels;

    public class ImageAlbumViewModel
    {
        public static ImageAlbumViewModel Create(ImageAlbum model, ApplicationUser currentUser)
        {
            bool liked = false;
            bool owner = false;

            if (currentUser != null)
            {
                if (currentUser.LikedImageAlbums.Any(b => b.Id == model.Id))
                {
                    liked = true;
                }

                if (currentUser.Id == model.CreatedById)
                {
                    owner = true;
                }
            }

            var result = new ImageAlbumViewModel()
            {
                Id = model.Id,
                Title = model.Title,
                CreatedBy = UserPreviewViewModel.Create(model.CreatedBy, currentUser),
                DateCreated = model.DateCreated,
                Likes = model.UserLikes.Take(MainConstants.PageSize)
                .ToList().Select(u => UserPreviewViewModel.Create(u, currentUser)),
                LikesCount = model.UserLikes.Count,
                Followers = model.Followers.Take(MainConstants.PageSize)
                .ToList().Select(u => UserPreviewViewModel.Create(u, currentUser)),
                FollowersCount = model.Followers.Count,
                CommentsCount = model.Comments.Count,
                ImagesCount = model.Images.Count,
                Comments = model.Comments.Take(MainConstants.PageSize).ToList()
                .Select(c => CommentViewModel.Create(c, currentUser)),
                IsLiked = liked,
                IsMine = owner,
                Images = model.Images.Take(MainConstants.PageSize).ToList()
                .Select(i => ImageViewModel.Create(i, currentUser))
            };

            return result;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public UserPreviewViewModel CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<UserPreviewViewModel> Likes { get; set; }

        public IEnumerable<UserPreviewViewModel> Followers { get; set; }

        public bool IsLiked { get; set; }

        public bool IsMine { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int ImagesCount { get; set; }

        public int FollowersCount { get; set; }

        public IEnumerable<ImageViewModel> Images { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}