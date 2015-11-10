namespace BlastOFF.Services.Models.CommentModels
{
    using BlastOFF.Services.Models.UserModels;

    using System;

    using BlastOFF.Models;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public CommentViewModel()
        {
            this.IsLiked = false;
            this.IsMine = false;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public UserPreviewViewModel Author { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public bool IsMine { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
               .ForMember(b => b.LikesCount, opt => opt.MapFrom(b => b.LikedBy.Count))
               .ForMember(b => b.Author, opt => opt.MapFrom(b => b.Author))
               .ReverseMap();
        }
    }
}