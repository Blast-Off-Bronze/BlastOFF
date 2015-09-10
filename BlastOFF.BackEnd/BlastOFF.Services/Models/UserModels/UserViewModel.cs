namespace BlastOFF.Services.Models.UserModels
{
    using BlastOFF.Models.UserModel;
    using System.Collections.Generic;
    using System.Linq;

    public class UserViewModel
    {
        public static UserViewModel Create(ApplicationUser model, ApplicationUser currentUser)
        {
                return new UserViewModel
                {
                    Username = model.UserName,
                    Email = model.Email,
                    FollowedUsers = model.FollowedUsers.Select(u => u.UserName),
                    FollowedBy = model.FollowedBy.Select(u => u.UserName),
                    FollowedImageAlbums = model.FollowedImageAlbums.Select(a => a.Title),
                    FollowedMusicAlbums = model.FollowedMusicAlbums.Select(a => a.Title),
                    LikedSongs = model.LikedSongs.Select(a => a.Title),
                    LikedImages = model.LikedImages.Select(i => i.Title),
                    LikedMusicAlbums = model.LikedMusicAlbums.Select(a => a.Title),
                    LikedImageAlbums = model.LikedImageAlbums.Select(a => a.Title),
                    ProfileImage = model.ProfileImage,
                    FollowedByMe = model.FollowedBy.Any(u => u.Id == currentUser.Id)
                };
        }

        public bool FollowedByMe { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string ProfileImage { get; set; }

        public virtual IEnumerable<string> FollowedUsers { get; set; }

        public virtual IEnumerable<string> FollowedBy { get; set; }

        public virtual IEnumerable<string> FollowedImageAlbums { get; set; }

        public virtual IEnumerable<string> FollowedMusicAlbums { get; set; }

        public virtual IEnumerable<string> LikedSongs { get; set; }

        public virtual IEnumerable<string> LikedImages { get; set; }

        public virtual IEnumerable<string> LikedMusicAlbums { get; set; }

        public virtual IEnumerable<string> LikedImageAlbums { get; set; }
    }
}