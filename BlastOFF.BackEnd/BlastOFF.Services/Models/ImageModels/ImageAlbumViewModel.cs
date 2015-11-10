namespace BlastOFF.Services.Models.ImageModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.GalleryModels;

    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.UserModels;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class ImageAlbumViewModel : IMapFrom<ImageAlbum>, IHaveCustomMappings
    {
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

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ImageAlbum, ImageAlbumViewModel>()
               .ForMember(u => u.LikesCount, opt => opt.MapFrom(u => u.UserLikes.Count))
               .ForMember(u => u.CommentsCount, opt => opt.MapFrom(u => u.Comments.Count))
               .ForMember(u => u.Comments, opt => opt.MapFrom(u => u.Comments.Take(MainConstants.PageSize)))
               .ForMember(u => u.Likes, opt => opt.MapFrom(u => u.UserLikes.Take(MainConstants.PageSize)))
               .ForMember(u => u.FollowersCount, opt => opt.MapFrom(u => u.Followers.Count))
               .ForMember(u => u.Followers, opt => opt.MapFrom(u => u.Followers.Take(MainConstants.PageSize)))
               .ForMember(u => u.ImagesCount, opt => opt.MapFrom(u => u.Images.Count))
               .ForMember(u => u.Images, opt => opt.MapFrom(u => u.Images.Take(MainConstants.PageSize)))
               .ForMember(u => u.CreatedBy, opt => opt.MapFrom(u => u.CreatedBy))
               .ReverseMap();
        }
    }
}