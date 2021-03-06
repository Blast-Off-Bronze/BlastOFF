﻿namespace BlastOFF.Services.Models.UserModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.UserModel;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string ProfileImage { get; set; }

        public bool FollowedByMe { get; set; }

        public bool IsMe { get; set; }

        public int BlastsCount { get; set; }

        //public int ImagesCount { get; set; }

        //public int SongsCount { get; set; }

        public virtual IEnumerable<string> FollowedUsers { get; set; }

        public virtual IEnumerable<string> FollowedBy { get; set; }

        public virtual IEnumerable<string> FollowedImageAlbums { get; set; }

        public virtual IEnumerable<string> FollowedMusicAlbums { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfileViewModel>()
              .ForMember(u => u.BlastsCount, opt => opt.MapFrom(u => u.Blasts.Count))
              .ForMember(u => u.FollowedBy, opt => opt.MapFrom(u => u.FollowedBy.Select(fu => fu.UserName)))
              .ForMember(u => u.FollowedUsers, opt => opt.MapFrom(u => u.FollowedUsers.Select(fu => fu.UserName)))
              .ForMember(u => u.FollowedImageAlbums, opt => opt.MapFrom(u => u.FollowedImageAlbums.Select(i => i.Title)))
              .ForMember(u => u.FollowedMusicAlbums, opt => opt.MapFrom(u => u.FollowedMusicAlbums.Select(m => m.Title)))
              .ReverseMap();
        }
    }
}