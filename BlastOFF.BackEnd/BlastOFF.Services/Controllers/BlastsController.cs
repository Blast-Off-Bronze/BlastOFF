using BlastOFF.Models;
using BlastOFF.Services.Models;
using BlastOFF.Services.Models.CommentModels;

namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    using Data;
    using Models.BlastModels;
    using BlastOFF.Models.BlastModels;
    using System.Linq;

    using System.Collections.Generic;
    using BlastOFF.Data.Interfaces;

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
        public IHttpActionResult GetBlastById([FromUri]int id)
        {
            var blast = this.Data.Blasts.Find(id);

            var blastToReturn = new List<Blast>() {blast}
                                .AsQueryable()
                                .Select(BlastViewModel.Create)
                                .First();

            if (blastToReturn == null)
            {
                return this.NotFound();
            }

            return this.Ok(blastToReturn);
        }

        [HttpPost]
        [Route("api/blast/{id}/like")]
        [Authorize]
        public IHttpActionResult LikeBlast([FromUri]int id)
        {
            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(loggedUserId);

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
        public IHttpActionResult RemoveLike([FromUri]int id)
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
        public IHttpActionResult UpdateBlast([FromUri]int id, [FromBody]BlastBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var oldBlast = this.Data.Blasts.Find(id);

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

        [HttpPost]
        [Route("api/blasts/{id}/comments")]
        [Authorize]
        public IHttpActionResult AddBlastComment([FromUri]int id, [FromBody] CommentBindingModel comment)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var blast = this.Data.Blasts.Find(id);

            if (blast == null)
            {
                return this.NotFound();
            }

            if (comment == null)
            {
                return this.BadRequest("Cannot create an empty comment model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newComment = new Comment
            {
                Content = comment.Content,
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                BlastId = id
            };

            this.Data.Comments.Add(newComment);
            this.Data.SaveChanges();

            comment.Id = newComment.Id;

            var commentCollection = new List<Comment> { newComment };

            var commentToReturn = commentCollection.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }
    }
}
