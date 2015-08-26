using System;
using System.Linq;
using System.Web.Http;
using BlastOFF.Data;
using BlastOFF.Data.Interfaces;
using BlastOFF.Models.GalleryModels;
using BlastOFF.Services.Models.ImageModels;
using Microsoft.AspNet.Identity;

namespace BlastOFF.Services.Controllers
{
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
        [Route("api/imageAlbums")]
        public IHttpActionResult GetAllImageAlbums()
        {
            var imageAlbums = this.Data.ImageAlbums.All().ToList();

            return this.Ok(imageAlbums);
        }

        

        [HttpGet]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult GetImageAlbumById([FromUri]int id)
        {
            var imageAlbum = this.Data.ImageAlbums.Find(id);

            if (imageAlbum == null)
            {
                return this.NotFound();
            }

            return this.Ok(imageAlbum);
        }

        [HttpPost]
        [Authorize]
        [Route("api/imageAlbums")]
        public IHttpActionResult CreateNewImageAlbum([FromBody]ImageAlbumBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
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

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult EditImageAlbum([FromUri]int id, [FromBody]ImageAlbumBindingModel model)
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

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImageAlbum([FromUri]int id)
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
        public IHttpActionResult GetImageById(int id)
        {
            var imagе = this.Data.Images.Find(id);

            if (imagе == null)
            {
                return this.NotFound();
            }

            return this.Ok(imagе);
        }

        [HttpPost]
        [Authorize]
        [Route("api/images")]
        public IHttpActionResult CreateNewImage([FromBody]ImageBindingModel model)
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
                ImageAlbumId = model.ImageAlbumId
            };

            this.Data.Images.Add(image);
            this.Data.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/images/{id}")]
        public IHttpActionResult EditImage([FromUri]int id, [FromBody]ImageBindingModel model)
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

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        [Route("api/imageAlbums/{id}")]
        public IHttpActionResult DeleteImage([FromUri]int id)
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
    }
}