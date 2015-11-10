namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;

    using Microsoft.AspNet.Identity;

    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.UserSessionUtils;

    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.UserModels;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var returnItem = Mapper.Map<CommentViewModel>(comment);

            if (currentUser != null)
            {
                returnItem.IsLiked = comment.LikedBy.Any(l => l.Id == currentUser.Id);
                returnItem.IsMine = comment.AuthorId == currentUser.Id;
            }

            return this.Ok(returnItem);
        }

        [HttpGet]
        [Route("api/comments/{id}/likes")]
        [AllowAnonymous]
        public IHttpActionResult CommentLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            var userLikes = comment.LikedBy
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage*PageSize)
                .Take(PageSize)
                .AsQueryable()
                .ProjectTo<UserPreviewViewModel>()
                .ToList();

            return this.Ok(userLikes);
        }

        //// PUT /api/comments/{id}
        [HttpPut]
        [Route("api/comments/{id}")]
        public IHttpActionResult UpdateComment([FromUri] int id, [FromBody] CommentEditBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var existingComment = this.Data.Comments.Find(id);

            if (existingComment == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id != existingComment.AuthorId)
            {
                return this.Unauthorized();
            }

            existingComment.Content = model.Content;
            existingComment.PostedOn = DateTime.Now;

            this.Data.SaveChanges();

            var returnItem = Mapper.Map<CommentViewModel>(existingComment);
            returnItem.IsMine = true;

            return this.Ok(returnItem);
        }

        //// DELETE /api/comments/{id}
        [HttpDelete]
        [Route("api/comments/{id}")]
        public IHttpActionResult DeleteComment([FromUri] int id)
        {
            var existingComment = this.Data.Comments.Find(id);

            if (existingComment == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();
            
            if (loggedUserId != existingComment.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.Comments.Delete(existingComment);

            this.Data.SaveChanges();

            return this.Ok();
        }

        //// POST /api/comments/{id}/likes
        [HttpPost]
        [Route("api/comments/{id}/like")]
        public IHttpActionResult LikeComment([FromUri] int id)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            var isAlreadyLiked = comment.LikedBy.Any(u => u.Id == currentUser.Id);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this comment.");
            }

            if (comment.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot like your own comment.");
            }

            comment.LikedBy.Add(currentUser);

            this.Data.SaveChanges();

            return this.Ok("Comment successfully liked.");
        }

        //// DELETE /api/comments/{id}/likes
        [HttpDelete]
        [Route("api/comments/{id}/unlike")]
        public IHttpActionResult UnlikeComment([FromUri] int id)
        {
            var comment = this.Data.Comments.Find(id);

            if (comment == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = comment.LikedBy.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this comment.");
            }

            if (comment.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot unlike your own comment.");
            }

            comment.LikedBy.Remove(currentUser);

            this.Data.SaveChanges();

            return this.Ok("Comment successfully unliked.");
        }
    }
}