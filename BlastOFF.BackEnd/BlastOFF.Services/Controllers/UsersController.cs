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
    }
}
