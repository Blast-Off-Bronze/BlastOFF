namespace BlastOFF.Services.Models.ImageModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.UserModels;

    using System;
    using BlastOFF.Models.GalleryModels;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class ImageViewModel : IMapFrom<Image>, IHaveCustomMappings
    {
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

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Image, ImageViewModel>()
                .ForMember(u => u.LikesCount, opt => opt.MapFrom(u => u.UserLikes.Count))
                .ForMember(u => u.CommentsCount, opt => opt.MapFrom(u => u.Comments.Count))
                .ForMember(u => u.Comments, opt => opt.MapFrom(u => u.Comments.Take(MainConstants.PageSize)))
                .ForMember(u => u.Likes, opt => opt.MapFrom(u => u.UserLikes.Take(MainConstants.PageSize)))
                .ForMember(u => u.UploadedBy, opt => opt.MapFrom(u => u.UploadedBy))
                .ForMember(u => u.DateAdded, opt => opt.MapFrom(u => u.DateCreated))
                .ForMember(u => u.ImageAlbum, opt => opt.MapFrom(u => u.ImageAlbum.Title))
                .ReverseMap();
        }
    }
}