namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Mapping;

    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Models.CommentModels;

    public class SongViewModel : IMapFrom<Song>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string FilePath { get; set; }

        public DateTime DateAdded { get; set; }

        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsOwn { get; set; }

        public bool IsLiked { get; set; }

        // Comments
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public bool AllCommentsDisplayed { get; set; }

        //// OPTIONAL
        public int? TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public DateTime? OriginalDate { get; set; }

        public string Genre { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Song, SongViewModel>()
              .ForMember(u => u.LikesCount, opt => opt.MapFrom(u => u.UserLikes.Count))
              .ForMember(u => u.CommentsCount, opt => opt.MapFrom(u => u.Comments.Count))
              .ForMember(u => u.Comments, opt => opt.MapFrom(u => u.Comments.Take(MainConstants.PageSize)))
              .ReverseMap();
        }
    }
}