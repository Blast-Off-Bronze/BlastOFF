namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models;
    using BlastOFF.Models.GalleryModels;
    using BlastOFF.Services.Models;
    using BlastOFF.Services.Models.ImageModels;
    using Microsoft.AspNet.Identity;

    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.UserSessionUtils;

    [SessionAuthorize]
    public class ImageAlbumController : BaseApiController
    {
        public ImageAlbumController()
            : this(new BlastOFFData())
        {
        }

        public ImageAlbumController(IBlastOFFData data) :
            base(data)
        {
        }

        // Image Albums EndPoints

        [HttpGet]
        [AllowAnonymous]
        [Route("api/imageAlbums")]
        public IHttpActionResult GetAllImageAlbums()
        {
            var imageAlbums = this.Data.ImageAlbums.All().ToList()
                .Select(a => ImageAlbumViewModel.Create(a));

            return this.Ok(imageAlbums);
        }

        [HttpGet]
        [Route("api/imageAlbums/my")]
        public IHttpActionResult GetMyImageAlbums()
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var imageAlbums = this.Data.ImageAlbums.All().Where(a => a.CreatedById == user.Id).ToList()
                .Select(a => ImageAlbumViewModel.Create(a));

            return this.Ok(imageAlbums);
        }

        [HttpGet]
        [Route("api/imageAlbums/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetImageAlbumById([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var returnItem = ImageAlbumViewModel.Create(imageAlbum);

            return this.Ok(returnItem);
        }

        [HttpPost]
        [Route("api/imageAlbums")]
        public IHttpActionResult CreateNewImageAlbum([FromBody] ImageAlbumCreateBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (model == null)
            {
                return this.BadRequest("No data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var imageAlbum = new ImageAlbum()
            {
                Title = model.Title,
                CreatedById = loggedUserId,
                DateCreated = DateTime.Now
            };

            if (this.Data.ImageAlbums.All().Any(a => a.CreatedById == loggedUserId && a.Title == imageAlbum.Title))
            {
                return this.BadRequest(string.Format("A image album with the specified title already exists."));
            }

            this.Data.ImageAlbums.Add(imageAlbum); 
            this.Data.SaveChanges();

            var user = this.Data.Users.Find(loggedUserId);
            var returnItem = ImageAlbumViewModel.Create(imageAlbum);

            return this.Ok(returnItem);
        }

        [HttpPut]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult EditImageAlbum([FromUri]int id, [FromBody] ImageAlbumModifyBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            if (imageAlbum.CreatedById != loggedUserId)
            {
                return this.Unauthorized();
            }

            imageAlbum.Title = model.Title;

            this.Data.ImageAlbums.Update(imageAlbum);
            this.Data.SaveChanges();

            var returnItem = ImageAlbumViewModel.Create(imageAlbum);

            return Ok(returnItem);
        }

        [HttpDelete]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImageAlbum([FromUri] int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            if (imageAlbum.CreatedById != loggedUserId)
            {
                return this.Unauthorized();
            }

            this.Data.ImageAlbums.Delete(imageAlbum);
            this.Data.SaveChanges();

            return Ok();
        }


        // Image EndPoints

        [HttpGet]
        [Route("api/images/{id}")]
        [AllowAnonymous]
        public IHttpActionResult GetImageById([FromUri] int id)
        {
            var imagе = this.Data.Images.Find(id);

            if (imagе == null)
            {
                return this.NotFound();
            }

            var returnItem = ImageViewModel.Create(imagе);

            return this.Ok(returnItem);
        }

        [HttpPost]
        [Route("api/images")]
        public IHttpActionResult CreateNewImage([FromBody] ImageCreateBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            if (!this.Data.ImageAlbums.All().Any(a => a.Id == model.ImageAlbumId))
            {
                return this.BadRequest("There is no image album with this id.");
            }

            var image = new Image()
            {
                Title = model.Title,
                UploadedById = loggedUserId,
                DateCreated = DateTime.Now,
                ImageAlbumId = model.ImageAlbumId,
                ImageData = model.Base64ImageString
            };

            this.Data.Images.Add(image);
            this.Data.SaveChanges();

            var returnItem = ImageViewModel.Create(this.Data.Images.Find(image.Id));

            return Ok(returnItem);
        }

        [HttpPut]
        [Route("api/images/{id}")]
        public IHttpActionResult EditImage([FromUri] int id, [FromBody] ImageModifyBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (image.ImageAlbum.CreatedById != loggedUserId)
            {
                return this.Unauthorized();
            }

            image.Title = model.Title;
            

            this.Data.Images.Update(image);
            this.Data.SaveChanges();

            var returnItem = ImageViewModel.Create(image);

            return Ok(returnItem);
        }

        [HttpDelete]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImage([FromUri] int id)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (image.ImageAlbum.CreatedById != loggedUserId)
            {
                return this.Unauthorized();
            }

            this.Data.Images.Delete(image);
            this.Data.SaveChanges();

            return Ok();
        }

        //// POST /api/imageAlbums/{id}/likes
        [HttpPost]
        [Route("api/imageAlbums/{id}/like")]
        public IHttpActionResult LikeImagecAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = imageAlbum.UserLikes.Any(u => u.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this image album.");
            }

            if (imageAlbum.CreatedById == loggedUserId)
            {
                return this.BadRequest("Cannot like your own image album.");
            }

            imageAlbum.UserLikes.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully liked.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// DELETE /api/imageAlbums/{id}/likes
        [HttpDelete]
        [Route("api/imageAlbums/{id}/unlike")]
        public IHttpActionResult UnlikeImageAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = imageAlbum.UserLikes.Any(u => u.Id == loggedUserId);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this image album.");
            }

            if (imageAlbum.CreatedById == loggedUserId)
            {
                return this.BadRequest("Cannot unlike your own image album.");
            }

            imageAlbum.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully unliked.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// POST /api/imageAlbums/{id}/follow
        [HttpPost]
        [Route("api/imageAlbums/{id}/follow")]
        public IHttpActionResult FollowImageAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var isAlreadyFollowed = imageAlbum.Followers.Any(u => u.Id == loggedUserId);

            if (isAlreadyFollowed)
            {
                return this.BadRequest("You are currently following this image album.");
            }

            if (imageAlbum.CreatedById == loggedUserId)
            {
                return this.BadRequest("Cannot follow your own image album.");
            }

            imageAlbum.Followers.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully followed.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// DELETE /api/imageAlbums/{id}/follow
        [HttpDelete]
        [Route("api/imageAlbums/{id}/unfollow")]
        public IHttpActionResult UnfollowImageAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var isAlreadyFollowed = imageAlbum.Followers.Any(u => u.Id == loggedUserId);

            if (!isAlreadyFollowed)
            {
                return this.BadRequest("You are currently not following this image album.");
            }

            if (imageAlbum.CreatedById == loggedUserId)
            {
                return this.BadRequest("Cannot unfollow your own image album.");
            }

            imageAlbum.Followers.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully unfollowed.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// GET /api/imageAlbums/{id}/comments
        [HttpGet]
        [Route("api/imageAlbums/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllImageAlbumComments([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var comments = imageAlbum.Comments.Select(c => CommentViewModel.Create(c));

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// GET /api/images/{id}/comments
        [HttpGet]
        [Route("api/images/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllImageComments([FromUri] int id)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var comments = image.Comments.Select(c => CommentViewModel.Create(c));

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// POST /api/imageAlbums/{id}/comments
        [HttpPost]
        [Route("api/imageAlbums/{id}/comments")]
        public IHttpActionResult AddImageAlbumComment([FromUri] int id, [FromBody] CommentCreateBindingModel model)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newComment = new Comment
            {
                Content = model.Content,
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                ImageAlbumId = id
            };

            this.Data.Comments.Add(newComment);

            this.Data.SaveChanges();
            this.Data.Dispose();

            var commentToReturn = CommentViewModel.Create(newComment);

            return this.Ok(commentToReturn);
        }

        [HttpPost]
        [Route("api/images/{id}/comments")]
        public IHttpActionResult AddImagesComment([FromUri] int id, [FromBody] CommentCreateBindingModel model)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newComment = new Comment
            {
                Content = model.Content,
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                ImageId = id
            };

            this.Data.Comments.Add(newComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newComment);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// POST /api/images/{id}/likes
        [HttpPost]
        [Route("api/images/{id}/like")]
        public IHttpActionResult LikeImage([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = image.UserLikes.Any(u => u.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this image.");
            }

            if (image.UploadedById == loggedUserId)
            {
                return this.BadRequest("Cannot like your own images.");
            }

            image.UserLikes.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully liked.", image.Title, image.UploadedBy.UserName));
        }

        //// DELETE /api/images/{id}/likes
        [HttpDelete]
        [Route("api/images/{id}/unlike")]
        public IHttpActionResult UnlikeImage([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(id);

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = image.UserLikes.Any(u => u.Id == loggedUserId);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this image.");
            }

            if (image.UploadedById == loggedUserId)
            {
                return this.BadRequest("Cannot unlike your own images.");
            }

            image.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully unliked.", image.Title, image.UploadedBy.UserName));
        }

        [HttpDelete]
        [Route("api/images/{id}")]
        public IHttpActionResult DeleteSong([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != image.UploadedById)
            {
                return this.Unauthorized();
            }

            this.Data.Images.Delete(image);

            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok(image);
        }
    }
}