using BlastOFF.Services.Models.CommentModels;

namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models;
    using BlastOFF.Services.Models;

    using Microsoft.AspNet.Identity;

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
        public IHttpActionResult FindCommentById([FromUri]int id)
        {
            var commentCollection = new List<Comment> { this.Data.Comments.Find(id) };

            var comment = commentCollection.AsQueryable().Select(CommentViewModel.Get);

            if (comment == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(comment);
        }

        //// PUT /api/comments/{id}
        [HttpPut]
        [Route("api/comments/{id}")]
        [Authorize]
        public IHttpActionResult UpdateComment([FromUri] int id, [FromBody] CommentEditBindingModel comment)
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

            if (comment == null)
            {
                return this.BadRequest("Cannot create an empty comment model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingComment.Content = comment.Content;
            existingComment.PostedOn = DateTime.Now;

            this.Data.SaveChanges();

            var commentCollection = new List<Comment> { existingComment };

            var commentToReturn = commentCollection.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// DELETE /api/comments/{id}
        [HttpDelete]
        [Route("api/comments/{id}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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