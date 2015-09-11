namespace BlastOFF.Services.Models.UserModels
{
    using System.Linq;
    using BlastOFF.Models.UserModel;

    public class UserPreviewViewModel
    {
        public static UserPreviewViewModel Create(ApplicationUser model, ApplicationUser currentUser)
        {
            bool isMe = false;
            bool followedByMe = false;

            if (currentUser != null)
            {
                if (currentUser.Id == model.Id)
                {
                    isMe = true;
                }

                if (model.FollowedBy.Any(u => u.Id == currentUser.Id))
                {
                    followedByMe = true;
                }
            }

            return new UserPreviewViewModel
            {
                Id = model.Id,
                Username = model.UserName,
                ProfileImage = model.ProfileImage,
                FollowedByMe = followedByMe,
                IsMe = isMe
            };
        }

        public string Id { get; set; }

        public bool FollowedByMe { get; set; }

        public bool IsMe { get; set; }

        public string Username { get; set; }

        public string ProfileImage { get; set; }
    }
}