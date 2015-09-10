using BlastOFF.Models.Enumerations;

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
    using BlastOFF.Services.Constants;

    using BlastOFF.Services.Models.UserModels;

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
        public IHttpActionResult GetAllImageAlbums([FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var imageAlbums = this.Data.ImageAlbums.All()
                .OrderByDescending(a => a.DateCreated)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(a => ImageAlbumViewModel.Create(a, currentUser));

            return this.Ok(imageAlbums);
        }

        [HttpGet]
        [Route("api/imageAlbums/my")]
        public IHttpActionResult GetMyImageAlbums()
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var imageAlbums = this.Data.ImageAlbums.All().Where(a => a.CreatedById == user.Id).ToList()
                .Select(a => ImageAlbumViewModel.Create(a, user));

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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var returnItem = ImageAlbumViewModel.Create(imageAlbum, currentUser);

            return this.Ok(returnItem);
        }

        [HttpPost]
        [Route("api/imageAlbums")]
        public IHttpActionResult CreateNewImageAlbum([FromBody] ImageAlbumCreateBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("No data.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var imageAlbum = new ImageAlbum()
            {
                Title = model.Title,
                CreatedById = user.Id,
                DateCreated = DateTime.Now
            };

            if (this.Data.ImageAlbums.All().Any(a => a.CreatedById == user.Id && a.Title == imageAlbum.Title))
            {
                return this.BadRequest(string.Format("A image album with the specified title already exists."));
            }

            this.Data.ImageAlbums.Add(imageAlbum); 
            this.Data.SaveChanges();

            foreach (var userId in user.FollowedBy.Select(u => u.Id))
            {
                var notification = new Notification()
                {
                    ImageAlbumId = imageAlbum.Id,
                    RecipientId = userId,
                    NotificationType = NotificationType.CreatedImageAlbum,
                    DateCreated = DateTime.Now,
                    Message = user.UserName + " created image album " + imageAlbum.Title + "."
                };

                this.Data.Notifications.Add(notification);
            }

            this.Data.SaveChanges();

            var returnItem = ImageAlbumViewModel.Create(imageAlbum, user);

            return this.Ok(returnItem);
        }

        [HttpPut]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult EditImageAlbum([FromUri]int id, [FromBody] ImageAlbumModifyBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());
            
            if (imageAlbum.CreatedById != currentUser.Id)
            {
                return this.Unauthorized();
            }

            imageAlbum.Title = model.Title;

            this.Data.ImageAlbums.Update(imageAlbum);
            this.Data.SaveChanges();

            var returnItem = ImageAlbumViewModel.Create(imageAlbum, currentUser);

            return Ok(returnItem);
        }

        [HttpDelete]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImageAlbum([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();
            
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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var returnItem = ImageViewModel.Create(imagе, currentUser);

            return this.Ok(returnItem);
        }

        [HttpPost]
        [Route("api/images")]
        public IHttpActionResult CreateNewImage([FromBody] ImageCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            if (!this.Data.ImageAlbums.All().Any(a => a.Id == model.ImageAlbumId))
            {
                return this.BadRequest("There is no image album with this id.");
            }

            var imageAlbum = this.Data.ImageAlbums.Find(model.ImageAlbumId);
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var image = new Image()
            {
                Title = model.Title,
                UploadedById = user.Id,
                DateCreated = DateTime.Now,
                ImageAlbumId = imageAlbum.Id,
                ImageData = model.Base64ImageString
            };

            this.Data.Images.Add(image);
            this.Data.SaveChanges();

            var userFollowers = user.FollowedBy.Select(u => u.Id);
            var albumFollowers = imageAlbum.Followers.Where(u => !userFollowers.Contains(u.Id))
                .Select(u => u.Id);

            foreach (var userId in userFollowers)
            {
                var notification = new Notification()
                {
                    ImageId = image.Id,
                    RecipientId = userId,
                    NotificationType = NotificationType.AddedImage,
                    DateCreated = DateTime.Now,
                    Message = user.UserName + " created image."
                };

                this.Data.Notifications.Add(notification);
            }

            foreach (var userId in albumFollowers)
            {
                var notification = new Notification()
                {
                    ImageId = image.Id,
                    RecipientId = userId,
                    NotificationType = NotificationType.AddedImage,
                    DateCreated = DateTime.Now,
                    Message = imageAlbum.Title + " new image."
                };

                this.Data.Notifications.Add(notification);
            }

            this.Data.SaveChanges();

            var returnItem = ImageViewModel.Create(image, user);

            return Ok(returnItem);
        }

        [HttpPut]
        [Route("api/images/{id}")]
        public IHttpActionResult EditImage([FromUri] int id, [FromBody] ImageModifyBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (image.ImageAlbum.CreatedById != currentUser.Id)
            {
                return this.Unauthorized();
            }

            image.Title = model.Title;
            
            this.Data.Images.Update(image);
            this.Data.SaveChanges();

            var returnItem = ImageViewModel.Create(image, currentUser);

            return Ok(returnItem);
        }

        [HttpDelete]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImage([FromUri] int id)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (image.ImageAlbum.CreatedById != loggedUserId)
            {
                return this.Unauthorized();
            }

            this.Data.Images.Delete(image);
            this.Data.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/imageAlbums/{id}/likes")]
        public IHttpActionResult ImageAlbumLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var userLikes = imageAlbum.UserLikes
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(userLikes);
        }

        //// POST /api/imageAlbums/{id}/likes
        [HttpPost]
        [Route("api/imageAlbums/{id}/like")]
        public IHttpActionResult LikeImagecAlbum([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = imageAlbum.UserLikes.Any(u => u.Id == currentUser.Id);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this image album.");
            }

            if (imageAlbum.CreatedById == currentUser.Id)
            {
                return this.BadRequest("Cannot like your own image album.");
            }

            imageAlbum.UserLikes.Add(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully liked.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// DELETE /api/imageAlbums/{id}/likes
        [HttpDelete]
        [Route("api/imageAlbums/{id}/unlike")]
        public IHttpActionResult UnlikeImageAlbum([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = imageAlbum.UserLikes.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this image album.");
            }

            if (imageAlbum.CreatedById == currentUser.Id)
            {
                return this.BadRequest("Cannot unlike your own image album.");
            }

            imageAlbum.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully unliked.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/imageAlbums/{id}/followers")]
        public IHttpActionResult ImageAlbumFollowers([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var followers = imageAlbum.Followers
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage*PageSize)
                .Take(PageSize)
                .ToList()
                .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(followers);
        }

        //// POST /api/imageAlbums/{id}/follow
        [HttpPost]
        [Route("api/imageAlbums/{id}/follow")]
        public IHttpActionResult FollowImageAlbum([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyFollowed = imageAlbum.Followers.Any(u => u.Id == currentUser.Id);

            if (isAlreadyFollowed)
            {
                return this.BadRequest("You are currently following this image album.");
            }

            if (imageAlbum.CreatedById == currentUser.Id)
            {
                return this.BadRequest("Cannot follow your own image album.");
            }

            imageAlbum.Followers.Add(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("Image Album {0}, created by {1} successfully followed.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// DELETE /api/imageAlbums/{id}/follow
        [HttpDelete]
        [Route("api/imageAlbums/{id}/unfollow")]
        public IHttpActionResult UnfollowImageAlbum([FromUri] int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyFollowed = imageAlbum.Followers.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyFollowed)
            {
                return this.BadRequest("You are currently not following this image album.");
            }

            if (imageAlbum.CreatedById == currentUser.Id)
            {
                return this.BadRequest("Cannot unfollow your own image album.");
            }

            imageAlbum.Followers.Remove(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully unfollowed.", imageAlbum.Title, imageAlbum.CreatedBy.UserName));
        }

        //// GET /api/imageAlbums/{id}/comments
        [HttpGet]
        [Route("api/imageAlbums/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllImageAlbumComments([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var comments = imageAlbum.Comments
                .OrderByDescending(c => c.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(c => CommentViewModel.Create(c, currentUser));

            return this.Ok(comments);
        }

        //// GET /api/images/{id}/comments
        [HttpGet]
        [Route("api/images/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllImageComments([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var comments = image.Comments
                .OrderByDescending(c => c.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(c => CommentViewModel.Create(c, currentUser));

            return this.Ok(comments);
        }

        //// POST /api/imageAlbums/{id}/comments
        [HttpPost]
        [Route("api/imageAlbums/{id}/comments")]
        public IHttpActionResult AddImageAlbumComment([FromUri] int id, [FromBody] CommentCreateBindingModel model)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var newComment = new Comment
            {
                Content = model.Content,
                AuthorId = user.Id,
                PostedOn = DateTime.Now,
                ImageAlbumId = id
            };

            this.Data.Comments.Add(newComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newComment, user);

            return this.Ok(commentToReturn);
        }

        [HttpPost]
        [Route("api/images/{id}/comments")]
        public IHttpActionResult AddImagesComment([FromUri] int id, [FromBody] CommentCreateBindingModel model)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var newComment = new Comment
            {
                Content = model.Content,
                AuthorId = user.Id,
                PostedOn = DateTime.Now,
                ImageId = id
            };

            this.Data.Comments.Add(newComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newComment, user);

            return this.Ok(commentToReturn);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/images/{id}/likes")]
        public IHttpActionResult ImageLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var userLikes = image.UserLikes
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(userLikes);
        }

        //// POST /api/images/{id}/likes
        [HttpPost]
        [Route("api/images/{id}/like")]
        public IHttpActionResult LikeImage([FromUri] int id)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = image.UserLikes.Any(u => u.Id == currentUser.Id);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this image.");
            }

            if (image.UploadedById == currentUser.Id)
            {
                return this.BadRequest("Cannot like your own images.");
            }

            image.UserLikes.Add(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully liked.", image.Title, image.UploadedBy.UserName));
        }

        //// DELETE /api/images/{id}/likes
        [HttpDelete]
        [Route("api/images/{id}/unlike")]
        public IHttpActionResult UnlikeImage([FromUri] int id)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = image.UserLikes.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this image.");
            }

            if (image.UploadedById == currentUser.Id)
            {
                return this.BadRequest("Cannot unlike your own images.");
            }

            image.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully unliked.", image.Title, image.UploadedBy.UserName));
        }

        [HttpDelete]
        [Route("api/images/{id}")]
        public IHttpActionResult DeleteSong([FromUri] int id)
        {
            var image = this.Data.Images.Find(id);

            if (image == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId != image.UploadedById)
            {
                return this.Unauthorized();
            }

            this.Data.Images.Delete(image);

            this.Data.SaveChanges();

            return this.Ok(image);
        }
    }
}