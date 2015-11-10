namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Models.CommentModels;
    using AutoMapper;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Mapping;

    public class MusicAlbumViewModel : IMapFrom<MusicAlbum>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public string Author { get; set; }

        public bool IsOwn { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsLiked { get; set; }

        public int ViewsCount { get; set; }

        public string CoverImageData { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int FollowersCount { get; set; }

        public int SongsCount { get; set; }

        // Songs
        public IEnumerable<SongViewModel> Songs { get; set; }

        // Comments
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MusicAlbum, MusicAlbumViewModel>()
              .ForMember(u => u.SongsCount, opt => opt.MapFrom(u => u.Songs.Count))
              .ForMember(u => u.FollowersCount, opt => opt.MapFrom(u => u.Followers.Count))
              .ForMember(u => u.CommentsCount, opt => opt.MapFrom(u => u.Comments.Count))
              .ForMember(u => u.Author, opt => opt.MapFrom(u => u.Author.UserName))
              .ForMember(u => u.Songs, opt => opt.MapFrom(u => u.Songs.Take(MainConstants.PageSize)))
              .ForMember(u => u.Comments, opt => opt.MapFrom(u => u.Comments.Take(MainConstants.PageSize)))
              .ReverseMap();
        }
    }
}