namespace BlastOFF.Services.Models.UserModels
{
    using BlastOFF.Models.UserModel;
    using System.Linq;

    public class UserViewOwnModel : UserViewModel
    {
        public static new UserViewOwnModel Create(ApplicationUser model)
        {
            return new UserViewOwnModel
            {
                Username = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                FollowedUsers = model.FollowedUsers.Select(u => u.UserName),
                FollowedBy = model.FollowedBy.Select(u => u.UserName),
                FollowedImageAlbums = model.FollowedImageAlbums.Select(a => a.Title),
                FollowedMusicAlbums = model.FollowedMusicAlbums.Select(a => a.Title),
                LikedSongs = model.LikedSongs.Select(a => a.Title),
                LikedImages = model.LikedImages.Select(i => i.Title),
                LikedMusicAlbums = model.LikedMusicAlbums.Select(a => a.Title),
                LikedImageAlbums = model.LikedImageAlbums.Select(a => a.Title),
                SentMessagesDetails = model.SentMessages.Select(m => new
                {
                    Content = m.Content,
                    Receiver = m.Receiver.UserName,
                    PostedOn = m.SentDateTime
                }),
                ReceivedMessagesDetails = model.ReceivedMessages.Select(m => new
                {
                    Content = m.Content,
                    Sender = m.Sender.UserName,
                    PostedOn = m.SentDateTime
                }),
                ProfileImage = model.ProfileImage
            };
        }

        public object SentMessagesDetails { get; set; } 

        public object ReceivedMessagesDetails { get; set; }
    }
}