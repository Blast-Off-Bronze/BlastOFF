namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    using Data;
    using Models.BlastModels;
    using BlastOFF.Models.BlastModels;
    using System.Linq;

    using BlastOFF.Data.Interfaces;

    using BlastOFF.Models;
    using BlastOFF.Services.Models.CommentModels;

    using BlastOFF.Services.UserSessionUtils;

    using BlastOFF.Services.Constants;

    using BlastOFF.Services.Models.UserModels;

    [SessionAuthorize]
    public class BlastsController : BaseApiController
    {
        public BlastsController()
            : this(new BlastOFFData())
        {

        }

        public BlastsController(IBlastOFFData data)
            : base(data)
        {

        }

        [HttpGet]
        [Route("api/blasts")]
        [AllowAnonymous]
        public IHttpActionResult GetAll([FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var blasts = this.Data.Blasts.All()
                .OrderByDescending(b => b.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(b => BlastViewModel.Create(b, user));

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/blasts/{username}/wall")]
        [AllowAnonymous]
        public IHttpActionResult GetWallBlasts([FromUri] string username, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var user = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.BadRequest("No user with that username.");
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            var blasts = user.Blasts
                .OrderByDescending(b => b.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(b => BlastViewModel.Create(b, currentUser));

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/blasts/public")]
        [AllowAnonymous]
        public IHttpActionResult GetPublicBlasts([FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var blasts = this.Data.Blasts.All()
                .Where(b => b.IsPublic == true)
                .OrderByDescending(b => b.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(b => BlastViewModel.Create(b, currentUser));

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/blasts/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetBlastById([FromUri] int id)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var blastToReturn = BlastViewModel.Create(blast, currentUser);

            return this.Ok(blastToReturn);
        }

        [HttpDelete]
        [Route("api/blasts/{id}")]
        [AllowAnonymous]
        public IHttpActionResult DeleteById([FromUri] int id)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            this.Data.Blasts.Delete(blast);

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/blasts/{id}/likes")]
        public IHttpActionResult BlastLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var userLikes = blast.UserLikes
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage*PageSize)
                .Take(PageSize)
                .ToList()
                .Select(u => UserPreviewViewModel.Create(u, currentUser)); ;

            return this.Ok(userLikes);
        }

        [HttpPost]
        [Route("api/blasts/{id}/like")]
        public IHttpActionResult LikeBlast([FromUri] int id)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (blast.AuthorId == currentUser.Id || blast.UserLikes.Any(ul => ul.Id == currentUser.Id))
            {
                //already liked or trying to like own blast
                return this.BadRequest();
            }

            blast.UserLikes.Add(currentUser);
            currentUser.LikedBlasts.Add(blast);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpDelete]
        [Route("api/blasts/{id}/unlike")]
        public IHttpActionResult RemoveLike([FromUri] int id)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (!blast.UserLikes.Any(ul => ul.Id == currentUser.Id))
            {
                //cannot remove other users' likes
                return this.BadRequest();
            }    

            blast.UserLikes.Remove(currentUser);
            currentUser.LikedBlasts.Remove(blast);
            this.Data.SaveChanges();

            return this.Ok();
        }


        [HttpPost]
        [Route("api/blasts")]
        public IHttpActionResult CreateNewBlast([FromBody] BlastCreateBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            var newBlast = new Blast
            {
                AuthorId = user.Id,
                Content = model.Content,
                PostedOn = DateTime.Now,
                BlastType = model.BlastType
            };

            this.Data.Blasts.Add(newBlast);
            this.Data.SaveChanges();
         
            var blastToReturn = BlastViewModel.Create(newBlast, user);

            return Ok(blastToReturn);
        }

        [HttpPut]
        [Route("api/blasts/{id}")]
        public IHttpActionResult UpdateBlast([FromUri] int id, [FromBody] BlastEditBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var oldBlast = this.Data.Blasts.Find(id);

            if (oldBlast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (oldBlast.AuthorId != currentUser.Id)
            {
                return this.Unauthorized();
            }

            oldBlast.Content = model.Content;

            this.Data.SaveChanges();

            var blastToReturn = BlastViewModel.Create(oldBlast, currentUser);

            return Ok(blastToReturn);
        }

        [HttpPost]
        [Route("api/blasts/{id}/comments")]
        public IHttpActionResult AddBlastComment([FromUri] int id, [FromBody] CommentCreateBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            var newComment = new Comment
            {
                Content = model.Content,
                AuthorId = currentUser.Id,
                PostedOn = DateTime.Now,
                BlastId = id
            };

            this.Data.Comments.Add(newComment);
            this.Data.SaveChanges();

            var returnItem = CommentViewModel.Create(newComment, currentUser);

            return this.Ok(returnItem);
        }
    }
}