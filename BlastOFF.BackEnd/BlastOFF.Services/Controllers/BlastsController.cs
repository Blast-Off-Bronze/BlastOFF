using BlastOFF.Data.Interfaces;

namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    using Data;
    using Models.BlastModels;
    using BlastOFF.Models.BlastModels;
    using System.Linq;

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
        public IHttpActionResult GetAll()
        {
            var blasts = this.Data.Blasts.All().Select(BlastViewModel.Create);

            return this.Ok(blasts);
        }

        [HttpGet]
        [Route("api/blasts/{id}")]
        public IHttpActionResult GetBlastById(int id)
        {
            var blast = this.Data.Blasts.All().Where(b => b.Id == id)
                .Select(BlastViewModel.Create);

            if (blast.Count() == 0)
            {
                return this.NotFound();
            }

            return this.Ok(blast);
        }

        [HttpGet]
        [Route("api/blasts/{username}")]
        public IHttpActionResult GetBlastsByAuthor(string username)
        {
            var blasts = this.Data.Blasts.All().Where(b => b.Author.UserName == username)
                .Select(BlastViewModel.Create);

            if (blasts.Count() == 0)
            {
                return this.NotFound();
            }

            return this.Ok(blasts);
        }

        [HttpPost]
        [Route("api/blast/{id}/like")]
        [Authorize]
        public IHttpActionResult LikeBlast(int id)
        {
            var blast = this.Data.Blasts.All().FirstOrDefault(b => b.Id == id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.Users.All().First(u => u.Id == loggedUserId);

            if (blast.AuthorId == loggedUserId || blast.UserLikes.Any(ul => ul.Id == loggedUserId))
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
        [Route("api/blast/{id}/like")]
        [Authorize]
        public IHttpActionResult RemoveLike(int id)
        {
            var blast = this.Data.Blasts.All().FirstOrDefault(b => b.Id == id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (!blast.UserLikes.Any(ul => ul.Id == loggedUserId))
            {
                //cannot remove other users' likes
                return this.BadRequest();
            }

            var currentUser = this.Data.Users.All().First(u => u.Id == loggedUserId);

            blast.UserLikes.Remove(currentUser);
            currentUser.LikedBlasts.Remove(blast);
            this.Data.SaveChanges();

            return this.Ok();
        }


        [HttpPost]
        [Authorize]
        [Route("api/blasts")]
        public IHttpActionResult CreateNewBlast([FromBody]BlastBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var newBlast = new Blast
            {
                AuthorId = loggedUserId,
                Content = model.Content,
                PostedOn = DateTime.Now,
                BlastType = model.BlastType
            };

            this.Data.Blasts.Add(newBlast);
            this.Data.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/blasts/{id}")]
        public IHttpActionResult UpdateBlast(int id, [FromBody]BlastBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var oldBlast = this.Data.Blasts.Find(id);

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (oldBlast == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            if (oldBlast.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            oldBlast.Content = model.Content;
            oldBlast.BlastType = model.BlastType;
            this.Data.SaveChanges();

            return Ok();
        }
    }
}
