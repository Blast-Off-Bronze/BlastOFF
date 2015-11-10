namespace BlastOFF.Services.Models.UserModels
{
    using System.Linq;

    using BlastOFF.Models.UserModel;

    public class UserViewOwnModel : UserViewModel
    {
        public object SentMessagesDetails { get; set; }

        public object ReceivedMessagesDetails { get; set; }
    }
}