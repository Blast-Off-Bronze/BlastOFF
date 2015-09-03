﻿namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;

    using Microsoft.AspNet.Identity;

    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.UserSessionUtils;

    [SessionAuthorize]
    public class CommentsController : BaseApiController
    {
        public CommentsController()
            : this(new BlastOFFData())
        {
        }

        public CommentsController(IBlastOFFData data)
            : base(data)
        {
        }

        //// GET /api/comments/{id}
        [HttpGet]
        [Route("api/comments/{id}")]
        [AllowAnonymous]
        public IHttpActionResult FindCommentById([FromUri] int id)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            var returnItem = CommentViewModel.Create(comment);

            return this.Ok(returnItem);
        }

        //// PUT /api/comments/{id}
        [HttpPut]
        [Route("api/comments/{id}")]
        public IHttpActionResult UpdateComment([FromUri] int id, [FromBody] CommentEditBindingModel model)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var existingComment = this.Data.Comments.Find(id);

            if (existingComment == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingComment.AuthorId)
            {
                return this.Unauthorized();
            }

            if (model == null)
            {
                return this.BadRequest("Cannot create an empty comment model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingComment.Content = model.Content;
            existingComment.PostedOn = DateTime.Now;

            this.Data.SaveChanges();

            var returnItem = CommentViewModel.Create(existingComment);

            this.Data.Dispose();

            return this.Ok(returnItem);
        }

        //// DELETE /api/comments/{id}
        [HttpDelete]
        [Route("api/comments/{id}")]
        public IHttpActionResult DeleteComment([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var existingComment = this.Data.Comments.Find(id);

            if (existingComment == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingComment.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.Comments.Delete(existingComment);

            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok();
        }

        //// POST /api/comments/{id}/likes
        [HttpPost]
        [Route("api/comments/{id}/likes")]
        public IHttpActionResult LikeComment([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = comment.LikedBy.Any(u => u.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this comment.");
            }

            if (comment.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot like your own comment.");
            }

            comment.LikedBy.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok("Comment successfully liked.");
        }

        //// DELETE /api/comments/{id}/likes
        [HttpDelete]
        [Route("api/comments/{id}/likes")]
        public IHttpActionResult UnlikeComment([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = comment.LikedBy.Any(u => u.Id == loggedUserId);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this comment.");
            }

            if (comment.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot unlike your own comment.");
            }

            comment.LikedBy.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok("Comment successfully unliked.");
        }
    }
}