namespace BlastOFF.Services.Models.UserModels
{
    using BlastOFF.Models.UserModel;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using BlastOFF.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfileImage { get; set; }

        public bool FollowedByMe { get; set; }

        public virtual IEnumerable<string> FollowedUsers { get; set; }

        public virtual IEnumerable<string> FollowedBy { get; set; }

        public virtual IEnumerable<string> FollowedImageAlbums { get; set; }

        public virtual IEnumerable<string> FollowedMusicAlbums { get; set; }

        public virtual IEnumerable<string> LikedSongs { get; set; }

        public virtual IEnumerable<string> LikedImages { get; set; }

        public virtual IEnumerable<string> LikedMusicAlbums { get; set; }

        public virtual IEnumerable<string> LikedImageAlbums { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserViewModel>()
              .ForMember(u => u.FollowedBy, opt => opt.MapFrom(u => u.FollowedBy.Select(fu => fu.UserName)))
              .ForMember(u => u.FollowedUsers, opt => opt.MapFrom(u => u.FollowedUsers.Select(fu => fu.UserName)))
              .ForMember(u => u.FollowedImageAlbums, opt => opt.MapFrom(u => u.FollowedImageAlbums.Select(i => i.Title)))
              .ForMember(u => u.FollowedMusicAlbums, opt => opt.MapFrom(u => u.FollowedMusicAlbums.Select(m => m.Title)))
              .ReverseMap();
        }
    }
}