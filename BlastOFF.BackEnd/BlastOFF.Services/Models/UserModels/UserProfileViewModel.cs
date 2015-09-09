namespace BlastOFF.Services.Models.UserModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BlastOFF.Models.UserModel;

    public class UserProfileViewModel
    {
        public static UserProfileViewModel Create(ApplicationUser model, ApplicationUser currentUser)
        {
            return new UserProfileViewModel
            {
                Username = model.UserName,
                FollowedUsers = model.FollowedUsers.Select(u => u.UserName),
                FollowedBy = model.FollowedBy.Select(u => u.UserName),
                FollowedImageAlbums = model.FollowedImageAlbums.Select(a => a.Title),
                FollowedMusicAlbums = model.FollowedMusicAlbums.Select(a => a.Title),
                ProfileImage = model.ProfileImage,
                FollowedByMe = model.FollowedBy.Any(u => u.Id == currentUser.Id),
                BlastsCount = model.Blasts.Count,
            };
        }

        public bool FollowedByMe { get; set; }

        public string Username { get; set; }

        public string ProfileImage { get; set; }

        public int BlastsCount { get; set; }

        //public int ImagesCount { get; set; }

        //public int SongsCount { get; set; }

        public virtual IEnumerable<string> FollowedUsers { get; set; }

        public virtual IEnumerable<string> FollowedBy { get; set; }

        public virtual IEnumerable<string> FollowedImageAlbums { get; set; }

        public virtual IEnumerable<string> FollowedMusicAlbums { get; set; }
    }
}