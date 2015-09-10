namespace BlastOFF.Services.Controllers
{
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using System.Web.Http;

    using Models.BlastModels;
    using Data;
    using Data.Interfaces;
    using Models.UserModels;

    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.UserSessionUtils;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Testing;

    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.ImageModels;

    [SessionAuthorize]
    public class UsersController : BaseApiController
    {
        private readonly ApplicationUserManager userManager;

        public UsersController()
            : this(new BlastOFFData())
        {
            this.userManager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(new BlastOFFContext()));
        }

        public UsersController(IBlastOFFData data)
            : base(data)
        {

        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get
            {
                return this.Request.GetOwinContext().Authentication;
            }
        }

        // POST api/User/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/register")]
        public async Task<IHttpActionResult> RegisterUser([FromBody] RegisterUserBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var emailExists = this.Data.Users.All()
                .Any(x => x.Email == model.Email);
            if (emailExists)
            {
                return this.BadRequest("Email is already taken.");
            }

            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Name = model.Name,
                Email = model.Email,
                Gender = model.Gender
            };

            var identityResult = await this.UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            var loginResult = await this.LoginUser(new LoginUserBindingModel()
            {
                Username = model.Username,
                Password = model.Password
            });

            return loginResult;
        }

        // POST api/User/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/login")]
        public async Task<IHttpActionResult> LoginUser([FromBody] LoginUserBindingModel model)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                return this.BadRequest("User is already logged in.");
            }

            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            // Use Microsoft.Owin.Testing.TestServer to perform in-memory HTTP POST request
            var testServer = TestServer.Create<Startup>();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            if (tokenServiceResponse.StatusCode == HttpStatusCode.OK)
            {
                // Sucessful login --> create user session in the database
                var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                var jsSerializer = new JavaScriptSerializer();
                var responseData =
                    jsSerializer.Deserialize<Dictionary<string, string>>(responseString);
                var authToken = responseData["access_token"];
                var username = responseData["userName"];
                var owinContext = this.Request.GetOwinContext();
                var userSessionManager = new UserSessionManager(owinContext);
                userSessionManager.CreateUserSession(username, authToken);

                // Cleanup: delete expired sessions from the database
                userSessionManager.DeleteExpiredSessions();
            }

            return this.ResponseMessage(tokenServiceResponse);
        }

        // POST api/User/Logout
        [HttpPost]
        [Route("api/account/logout")]
        public IHttpActionResult Logout()
        {
            // This does not actually perform logout! The OWIN OAuth implementation
            // does not support "revoke OAuth token" (logout) by design.
            this.Authentication.SignOut(DefaultAuthenticationTypes.ExternalBearer);

            // Delete the user's session from the database (revoke its bearer token)
            var owinContext = this.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(owinContext);
            userSessionManager.InvalidateUserSession();

            return this.Ok(new
            {
                message = "Logout successful."
            });
        }

        [HttpGet]
        [Route("api/users/{username}/blasts")]
        public IHttpActionResult GetBlastsByAuthor([FromUri] string username, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var blasts = this.Data.Blasts.All()
                .OrderByDescending(b => b.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(b => BlastViewModel.Create(b, currentUser));

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/users/{username}/imageAlbums")]
        public IHttpActionResult GetImageAlbumsByUsername([FromUri] string username, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var imageAlbums = this.Data.ImageAlbums.All()
                .OrderByDescending(a => a.DateCreated)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(b => ImageAlbumViewModel.Create(b, currentUser));

            return this.Ok(imageAlbums);
        }

        [HttpGet]
        [Route("api/users/{username}/profile")]
        public IHttpActionResult ViewProfile([FromUri] string username)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            //if (searchedUser.Id == currentUser.Id)
            //{
            //    var ownProfile = UserViewOwnModel.Create(currentUser);

            //    return this.Ok(ownProfile);
            //}

            var userProfile = UserProfileViewModel.Create(searchedUser, currentUser);

            return this.Ok(userProfile);
        }

        [HttpGet]
        [Route("api/users/{username}/followers")]
        public IHttpActionResult GetFollowers([FromUri] string username, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            // needs view model

            var followers = searchedUser.FollowedBy
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList();

            return this.Ok(followers);
        }

        [HttpPost]
        [Route("api/users/{username}/follow")]
        public IHttpActionResult FollowUser([FromUri] string username)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            if (currentUser.Id == searchedUser.Id)
            {
                //cant follow yourself
                return this.BadRequest();
            }

            if (searchedUser.FollowedBy.Any(f => f.Id == currentUser.Id))
            {
                //already follow by you
                return this.BadRequest();
            }


            currentUser.FollowedUsers.Add(searchedUser);
            searchedUser.FollowedBy.Add(currentUser);

            this.Data.SaveChanges();
            return this.Ok();
        }

        [HttpDelete]
        [Route("api/users/{username}/unfollow")]
        public IHttpActionResult UnfollowUser([FromUri] string username)
        {
            var searchedUser = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            if (!searchedUser.FollowedBy.Any(f => f.Id == currentUser.Id))
            {
                //you are not following this user anyway
                return this.BadRequest();
            }

            currentUser.FollowedUsers.Remove(searchedUser);
            searchedUser.FollowedBy.Remove(currentUser);

            this.Data.SaveChanges();
            return this.Ok();
        }
    }
}
