namespace BlastOFF.Services.Models.UserModels
{
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Mapping;

    public class UserPreviewViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfileImage { get; set; }

        public bool FollowedByMe { get; set; }

        public bool IsMe { get; set; }
    }
}