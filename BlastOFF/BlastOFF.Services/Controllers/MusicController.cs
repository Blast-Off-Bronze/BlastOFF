namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Models.MusicModels;

    using Microsoft.AspNet.Identity;

    public class MusicController : ApiController
    {
        private IBlastOFFData data;

        public MusicController()
            : this(new BlastOFFData())
        {
        }

        public MusicController(IBlastOFFData data)
        {
            this.data = data;
        }

        // START - MUSIC ALBUMS Endpoints

        //// GET /api/music/albums
        [HttpGet]
        [Route("api/music/albums")]
        public IHttpActionResult All()
        {
            var albums = this.data.MusicAlbums.All();

            return this.Ok(albums);
        }

        //// GET /api/music/albums/{id}
        [HttpGet]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Find(int id)
        {
            var album = this.data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.BadRequest(string.Format("A music album with id {0} does not exist", id));
            }

            this.data.Dispose();

            return this.Ok(album);
        }

        //// POST /api/music/albums
        [HttpPost]
        [Route("api/music/albums")]
        public IHttpActionResult Create(MusicAlbumBindingModel musicAlbum)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            if (musicAlbum == null)
            {
                return this.BadRequest("Cannot create an empty music album model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newMusicAlbum = new MusicAlbum
                                    {
                                        Title = musicAlbum.Title,
                                        CreatedById = loggedUserId,
                                        DateCreated = DateTime.Now,
                                        ViewsCount = 0
                                    };

            if (this.data.MusicAlbums.All().Any(a => a.CreatedById == loggedUserId && a.Title == newMusicAlbum.Title))
            {
                return this.BadRequest(string.Format("A music album with the specified title already exists."));
            }

            this.data.MusicAlbums.Add(newMusicAlbum);
            this.data.SaveChanges();

            musicAlbum.Id = newMusicAlbum.Id;

            this.data.Dispose();

            return this.Ok(musicAlbum);
        }

        //// PUT /api/music/albums/{id}
        [HttpPut]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Update(int id, MusicAlbumBindingModel musicAlbum)
        {
            var existingMusicAlbum = this.data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            if (loggedUserId != existingMusicAlbum.CreatedById)
            {
                return this.Unauthorized();
            }

            if (musicAlbum == null)
            {
                return this.BadRequest("Cannot create an empty music album model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingMusicAlbum.Title = musicAlbum.Title;
            this.data.SaveChanges();

            musicAlbum.Id = existingMusicAlbum.Id;

            this.data.Dispose();

            return this.Ok(musicAlbum);
        }

        //// DELETE /api/music/albums/{id}
        [HttpDelete]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var existingMusicAlbum = this.data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            if (loggedUserId != existingMusicAlbum.CreatedById)
            {
                return this.Unauthorized();
            }

            this.data.MusicAlbums.Delete(existingMusicAlbum);
            this.data.SaveChanges();

            this.data.Dispose();

            return this.Ok(string.Format("Category with id {0} successfully deleted", id));
        }

        // END - MUSIC ALBUMS Endpoints
    }
}