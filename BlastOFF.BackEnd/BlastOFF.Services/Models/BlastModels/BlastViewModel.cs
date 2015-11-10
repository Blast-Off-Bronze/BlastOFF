namespace BlastOFF.Services.Models.BlastModels
{
    using BlastOFF.Services.Models.UserModels;

    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Services.Models.CommentModels;

    using BlastOFF.Services.Constants;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class BlastViewModel : IMapFrom<Blast>, IHaveCustomMappings
    {
        public BlastViewModel()
        {
            this.IsLiked = false;
            this.IsMine = false;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public BlastType BlastType { get; set; }

        //Mapping

        public UserPreviewViewModel Author { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public IEnumerable<UserPreviewViewModel> Likes { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public bool IsLiked { get; set; }

        public bool IsMine { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Blast, BlastViewModel>()
               .ForMember(b => b.CommentsCount, opt => opt.MapFrom(b => b.Comments.Count))
               .ForMember(b => b.LikesCount, opt => opt.MapFrom(b => b.UserLikes.Count))
               .ForMember(b => b.Comments, opt => opt.MapFrom(b => b.Comments.Take(MainConstants.PageSize)))
               .ForMember(b => b.Likes, opt => opt.MapFrom(b => b.UserLikes.Take(MainConstants.PageSize)))
               .ForMember(b => b.Author, opt => opt.MapFrom(b => b.Author))
               .ReverseMap();
        }
    }
}