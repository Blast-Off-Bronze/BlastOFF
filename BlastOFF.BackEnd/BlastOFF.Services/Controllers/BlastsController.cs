﻿namespace BlastOFF.Services.Controllers
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

    using BlastOFF.Models;
    using BlastOFF.Services.Models.CommentModels;

    using BlastOFF.Services.UserSessionUtils;

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
        public IHttpActionResult GetAll()
        {
            var blasts = this.Data.Blasts.All().Select(BlastViewModel.Create);

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

            var blastToReturn = BlastViewModel.Create(blast);

            return this.Ok(blastToReturn);
        }

        [HttpPost]
        [Route("api/blast/{id}/like")]
        public IHttpActionResult LikeBlast([FromUri] int id)
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
        public IHttpActionResult RemoveLike([FromUri] int id)
        {
            var blast = this.Data.Blasts.Find(id);

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

            var currentUser = this.Data.Users.Find(loggedUserId);

            blast.UserLikes.Remove(currentUser);
            currentUser.LikedBlasts.Remove(blast);
            this.Data.SaveChanges();

            return this.Ok();
        }


        [HttpPost]
        [Route("api/blasts")]
        public IHttpActionResult CreateNewBlast([FromBody] BlastBindingModel model)
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

            var blastToReturn = BlastViewModel.Create(newBlast);

            return Ok(blastToReturn);
        }

        [HttpPut]
        [Route("api/blasts/{id}")]
        public IHttpActionResult UpdateBlast([FromUri] int id, [FromBody] BlastBindingModel model)
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

            var blastToReturn = BlastViewModel.Create(oldBlast);

            return Ok(blastToReturn);
        }

        [HttpPost]
        [Route("api/blasts/{id}/comments")]
        public IHttpActionResult AddBlastComment([FromUri] int id, [FromBody] CommentCreateBindingModel comment)
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

            var returnItem = CommentViewModel.Create(newComment);

            this.Data.Dispose();

            return this.Ok(returnItem);
        }
    }
}
