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

    public class MusicController : BaseApiController
    {
        public MusicController()
            : this(new BlastOFFData())
        {
        }

        public MusicController(IBlastOFFData data):
            base(data)
        {
        }

        // START - MUSIC ALBUMS Endpoints

        //// GET /api/music/albums
        [HttpGet]
        [Route("api/music/albums")]
        public IHttpActionResult All()
        {
            var albums = this.Data.MusicAlbums.All().Select(MusicAlbumViewModel.Get);

            this.Data.Dispose();

            return this.Ok(albums);
        }

        //// GET /api/music/albums/{id}
        [HttpGet]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Find(int id)
        {
            var album = this.Data.MusicAlbums.All().Where(a => a.Id == id).Select(MusicAlbumViewModel.Get).FirstOrDefault();

            if (album == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

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
                AuthorId = loggedUserId,
                DateCreated = DateTime.Now,
                ViewsCount = 0
            };

            if (this.Data.MusicAlbums.All().Any(a => a.AuthorId == loggedUserId && a.Title == newMusicAlbum.Title))
            {
                return this.BadRequest(string.Format("A music album with the specified title already exists."));
            }

            this.Data.MusicAlbums.Add(newMusicAlbum);
            this.Data.SaveChanges();

            musicAlbum.Id = newMusicAlbum.Id;

            this.Data.Dispose();

            return this.Ok(musicAlbum);
        }

        //// PUT /api/music/albums/{id}
        [HttpPut]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Update(int id, MusicAlbumBindingModel musicAlbum)
        {
            var existingMusicAlbum = this.Data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            if (loggedUserId != existingMusicAlbum.AuthorId)
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
            this.Data.SaveChanges();

            musicAlbum.Id = existingMusicAlbum.Id;

            this.Data.Dispose();

            return this.Ok(musicAlbum);
        }

        //// DELETE /api/music/albums/{id}
        [HttpDelete]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var existingMusicAlbum = this.Data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            if (loggedUserId != existingMusicAlbum.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.MusicAlbums.Delete(existingMusicAlbum);
            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok(existingMusicAlbum);
        }

        // END - MUSIC ALBUMS Endpoints

        // START - SONGS Endpoints

        //// GET /api/music/albums/{albumId}/songs
        [HttpGet]
        [Route("api/music/albums/{albumId}/songs")]
        public IHttpActionResult All(int albumId)
        {
            var album = this.Data.MusicAlbums.Find(albumId);

            if (album == null)
            {
                return this.NotFound();
            }

            var songs = this.Data.Songs.All().Where(s => s.MusicAlbumId == albumId).Select(SongViewModel.Get);

            this.Data.Dispose();

            return this.Ok(songs);
        }

        //// GET /api/music/albums/{albumId}/songs/{id}
        [HttpGet]
        [Route("api/music/albums/{albumId}/songs/{id}")]
        public IHttpActionResult Find(int albumId, int id)
        {
            var album = this.Data.MusicAlbums.Find(albumId);

            if (album == null)
            {
                return this.NotFound();
            }

            var song = this.Data.Songs.All().Where(s => s.MusicAlbumId == albumId && s.Id == id).Select(SongViewModel.Get).FirstOrDefault();

            if (song == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(song);
        }

        ////// POST /api/music/albums
        //[HttpPost]
        //[Route("api/music/albums")]
        //public IHttpActionResult Create(MusicAlbumBindingModel musicAlbum)
        //{
        //    string loggedUserId = this.User.Identity.GetUserId();

        //    if (string.IsNullOrEmpty(loggedUserId))
        //    {
        //        return this.BadRequest("You have to be logged in to continue.");
        //    }

        //    if (musicAlbum == null)
        //    {
        //        return this.BadRequest("Cannot create an empty music album model.");
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    var newMusicAlbum = new MusicAlbum
        //    {
        //        Title = musicAlbum.Title,
        //        AuthorId = loggedUserId,
        //        DateCreated = DateTime.Now,
        //        ViewsCount = 0
        //    };

        //    if (this.data.MusicAlbums.All().Any(a => a.AuthorId == loggedUserId && a.Title == newMusicAlbum.Title))
        //    {
        //        return this.BadRequest(string.Format("A music album with the specified title already exists."));
        //    }

        //    this.data.MusicAlbums.Add(newMusicAlbum);
        //    this.data.SaveChanges();

        //    musicAlbum.Id = newMusicAlbum.Id;

        //    this.data.Dispose();

        //    return this.Ok(musicAlbum);
        //}

        ////// PUT /api/music/albums/{id}
        //[HttpPut]
        //[Route("api/music/albums/{id}")]
        //public IHttpActionResult Update(int id, MusicAlbumBindingModel musicAlbum)
        //{
        //    var existingMusicAlbum = this.data.MusicAlbums.Find(id);

        //    if (existingMusicAlbum == null)
        //    {
        //        return this.NotFound();
        //    }

        //    string loggedUserId = this.User.Identity.GetUserId();

        //    if (string.IsNullOrEmpty(loggedUserId))
        //    {
        //        return this.BadRequest("You have to be logged in to continue.");
        //    }

        //    if (loggedUserId != existingMusicAlbum.AuthorId)
        //    {
        //        return this.Unauthorized();
        //    }

        //    if (musicAlbum == null)
        //    {
        //        return this.BadRequest("Cannot create an empty music album model.");
        //    }

        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    existingMusicAlbum.Title = musicAlbum.Title;
        //    this.data.SaveChanges();

        //    musicAlbum.Id = existingMusicAlbum.Id;

        //    this.data.Dispose();

        //    return this.Ok(musicAlbum);
        //}

        ////// DELETE /api/music/albums/{id}
        //[HttpDelete]
        //[Route("api/music/albums/{id}")]
        //public IHttpActionResult Delete(int id)
        //{
        //    var existingMusicAlbum = this.data.MusicAlbums.Find(id);

        //    if (existingMusicAlbum == null)
        //    {
        //        return this.NotFound();
        //    }

        //    string loggedUserId = this.User.Identity.GetUserId();

        //    if (string.IsNullOrEmpty(loggedUserId))
        //    {
        //        return this.BadRequest("You have to be logged in to continue.");
        //    }

        //    if (loggedUserId != existingMusicAlbum.AuthorId)
        //    {
        //        return this.Unauthorized();
        //    }

        //    this.data.MusicAlbums.Delete(existingMusicAlbum);
        //    this.data.SaveChanges();

        //    this.data.Dispose();

        //    return this.Ok(string.Format("Category with id {0} successfully deleted", id));
        //}

        //// END - MUSIC ALBUMS Endpoints
    }
}