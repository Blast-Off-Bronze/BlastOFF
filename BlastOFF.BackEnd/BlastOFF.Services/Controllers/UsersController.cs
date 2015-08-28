namespace BlastOFF.Services.Controllers
{
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using System.Web.Http;

    using Models.BlastModels;
    using Data;
    using Data.Interfaces;
    using Models.UserModels;

    public class UsersController : BaseApiController
    {
        public UsersController()
            : this(new BlastOFFData())
        {

        }

        public UsersController(IBlastOFFData data)
            : base(data)
        {

        }

        [HttpGet]
        [Route("api/users/{username}/blasts")]
        public IHttpActionResult GetBlastsByAuthor([FromUri]string username)
        {
            var blasts = this.Data.Blasts.All().Where(b => b.Author.UserName == username)
                .Select(BlastViewModel.Create);

            if (blasts.Count() == 0)
            {
                return this.NotFound();
            }

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/users/{username}/profile")]
        public IHttpActionResult ViewProfile([FromUri]string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            if (searchedUser.Id == loggedUserId)
            {
                var ownProfile = this.Data.Users.All().Where(u => u.UserName == username)
                    .Select(UserViewOwnModel.Create);

                return this.Ok(ownProfile);
            }

            var userProfile = this.Data.Users.All().Where(u => u.UserName == username)
                .Select(UserViewModel.Create);

            return this.Ok(userProfile);

        }

        [HttpGet]
        [Route("api/users/{username}/followers")]
        public IHttpActionResult GetFollowers([FromUri]string username)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            // needs view model

            var followers = searchedUser.FollowedBy.ToList();

            return this.Ok(followers);
        }

        [HttpPost]
        [Route("api/users/{username}/follow")]
        [Authorize]
        public IHttpActionResult FollowUser([FromUri]string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            if (loggedUserId == searchedUser.Id)
            {
                //cant follow yourself
                return this.BadRequest();
            }

            if (searchedUser.FollowedBy.Any(f => f.Id == loggedUserId))
            {
                //already follow by you
                return this.BadRequest();
            }

            var currentUser = this.Data.Users.All().First(u => u.Id == loggedUserId);

            currentUser.FollowedUsers.Add(searchedUser);
            searchedUser.FollowedBy.Add(currentUser);

            this.Data.SaveChanges();
            return this.Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("api/users/{username}/unfollow")]
        public IHttpActionResult UnfollowUser([FromUri]string username)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            if (!searchedUser.FollowedBy.Any(f => f.Id == loggedUserId))
            {
                //you are not following this user anyway
                return this.BadRequest();
            }

            var currentUser = this.Data.Users.All().First(u => u.Id == loggedUserId);

            currentUser.FollowedUsers.Remove(searchedUser);
            searchedUser.FollowedBy.Remove(currentUser);

            this.Data.SaveChanges();
            return this.Ok();
        }
    }
}
