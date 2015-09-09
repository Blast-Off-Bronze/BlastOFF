using BlastOFF.Models.UserModel;
using BlastOFF.Services.Models.CommentModels;

namespace BlastOFF.Services.Models.ImageModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.GalleryModels;

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
                CreatedBy = model.CreatedBy.UserName,
                CreatedById = model.CreatedBy.Id,
                DateCreated = model.DateCreated,
                LikesCount = model.UserLikes.Count,
                CommentsCount = model.Comments.Count,
                FollowersCount = model.Followers.Count,
                ImagesCount = model.Images.Count,
                Comments = model.Comments.ToList().Take(3).Select(c => CommentViewModel.Create(c, currentUser)),
                IsLiked = liked,
                AmITheAuthor = owner
            };

            return result;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string CreatedById { get; set; }

        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int FollowersCount { get; set; }

        public int ImagesCount { get; set; }

        public bool IsLiked { get; set; }

        public bool AmITheAuthor { get; set; }

        public IEnumerable<string> Images { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}