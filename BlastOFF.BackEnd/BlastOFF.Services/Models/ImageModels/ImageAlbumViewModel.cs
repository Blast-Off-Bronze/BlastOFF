namespace BlastOFF.Services.Models.ImageModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.GalleryModels;
    using BlastOFF.Models.UserModel;

    public class ImageAlbumViewModel
    {
        public static ImageAlbumViewModel Create(ImageAlbum a, ApplicationUser user)
        {
            return new ImageAlbumViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                CreatedBy = a.CreatedBy.UserName,
                CreatedById = a.CreatedBy.Id,
                DateCreated = a.DateCreated,
                LikesCount = a.UserLikes.Count,
                CommentsCount = a.Comments.Count,
                FollowersCount = a.Followers.Count,
                ImagesCount = a.Images.Count,
                Images = a.Images.Select(i => i.Title).Take(3)
            };
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

        public IEnumerable<string> Images { get; set; }
    }
}