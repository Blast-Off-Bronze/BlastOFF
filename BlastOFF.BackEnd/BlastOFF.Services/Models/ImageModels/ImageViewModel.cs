using System.Linq;
using BlastOFF.Models.UserModel;

namespace BlastOFF.Services.Models.ImageModels
{
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
                LikesCount = model.UserLikes.Count,
                CommentsCount = model.Comments.Count,
                UploadedBy = model.UploadedBy.UserName,
                UploadedById = model.UploadedBy.Id,
                IsLiked = liked,
                AmITheOwner = owner
            };

            return result;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int ImageAlbumId { get; set; }

        public string ImageAlbum { get; set; }

        public string UploadedById { get; set; }

        public string UploadedBy { get; set; }

        public string ImageData { get; set; }

        public DateTime DateAdded { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsLiked { get; set; }

        public bool AmITheOwner { get; set; }
    }
}