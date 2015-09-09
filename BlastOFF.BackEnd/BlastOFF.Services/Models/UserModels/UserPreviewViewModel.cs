namespace BlastOFF.Services.Models.UserModels
{
    using System.Linq;
    using BlastOFF.Models.UserModel;

    public class UserPreviewViewModel
    {
        public static UserPreviewViewModel Create(ApplicationUser model, ApplicationUser currentUser)
        {
            return new UserPreviewViewModel
            {
                Id = model.Id,
                Username = model.UserName,
                ProfileImage = model.ProfileImage,
                FollowedByMe = model.FollowedBy.Any(u => u.Id == currentUser.Id),
                IsMe = model.Id == currentUser.Id
            };
        }

        public string Id { get; set; }

        public bool FollowedByMe { get; set; }

        public bool IsMe { get; set; }

        public string Username { get; set; }

        public string ProfileImage { get; set; }
    }
}