namespace BlastOFF.Services.Models.ImageModels
{
    using System;
    using BlastOFF.Models.GalleryModels;

    public class ImageViewModel
    {
        public static ImageViewModel Create(Image i)
        {
            return new ImageViewModel
            {
                Id = i.Id,
                Title = i.Title,
                ImageAlbumId = i.ImageAlbumId,
                ImageAlbum = i.ImageAlbum.Title,
                DateAdded = i.DateCreated,
                LikesCount = i.UserLikes.Count,
                CommentsCount = i.Comments.Count,
                UploadedBy = i.UploadedBy.UserName,
                UploadedById = i.UploadedBy.Id
            };
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int? ImageAlbumId { get; set; }

        public string ImageAlbum { get; set; }

        public string UploadedById { get; set; }

        public string UploadedBy { get; set; }

        public string ImageData { get; set; }

        public DateTime DateAdded { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }
    }
}