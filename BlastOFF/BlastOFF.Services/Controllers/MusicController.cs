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

        public MusicController(IBlastOFFData data)
            : base(data)
        {
        }

        // START - MUSIC ALBUM Endpoints

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
            var album =
                this.Data.MusicAlbums.All().Where(a => a.Id == id).Select(MusicAlbumViewModel.Get).FirstOrDefault();

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

        // END - MUSIC ALBUM Endpoints

        // START - SONG Endpoints

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

            var song = this.Data.Songs.All()
                    .Where(s => s.MusicAlbumId == albumId && s.Id == id)
                    .Select(SongViewModel.Get)
                    .FirstOrDefault();

            if (song == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(song);
        }

        //// POST /api/music/albums/{albumId}/songs
        [HttpPost]
        [Route("api/music/albums/{albumId}/songs")]
        public IHttpActionResult Create(int albumId, SongBindingModel song)
        {
            // TODO: Upload song to Google Drive and acquire link

            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var album = this.Data.MusicAlbums.Find(albumId);

            if (album == null)
            {
                return this.NotFound();
            }

            if (song == null)
            {
                return this.BadRequest("Cannot create an empty song model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var newSong = new Song
                {
                    Title = song.Title,
                    Artist = song.Artist,
                    // FilePath = link to Google Drive,
                    MusicAlbumId = album.Id,
                    UploaderId = loggedUserId,
                    DateAdded = DateTime.Now,
                    ViewsCount = 0,
                    TrackNumber = song.TrackNumber,
                    OriginalAlbumTitle = song.OriginalAlbumTitle,
                    OriginalAlbumArtist = song.OriginalAlbumArtist,
                    OriginalDate = song.OriginalDate,
                    Genre = song.Genre,
                    Composer = song.Composer,
                    Publisher = song.Publisher,
                    Bpm = song.Bpm
                };

            if (album.Songs.Any(s => s == newSong))
            {
                return this.BadRequest(string.Format("This song already exists in album."));
            }

            this.Data.Songs.Add(newSong);
            song.Id = newSong.Id;

            album.Songs.Add(newSong);
            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(song);
        }

        //// PUT /api/music/albums/{albumId}/songs/{id}
        [HttpPut]
        [Route("api/music/albums/{albumId}/songs/{id}")]
        public IHttpActionResult Update(int albumId, int id, SongBindingModel song)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var existingMusicAlbum = this.Data.MusicAlbums.Find(albumId);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingMusicAlbum.AuthorId)
            {
                return this.Unauthorized();
            }

            var existingSong = this.Data.Songs.All().FirstOrDefault(s => s.MusicAlbumId == albumId && s.Id == id);

            if (existingSong == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingSong.UploaderId)
            {
                return this.Unauthorized();
            }

            if (song == null)
            {
                return this.BadRequest("Cannot create an empty song model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            existingSong.Title = song.Title;
            existingSong.Artist = song.Artist;
            existingSong.TrackNumber = song.TrackNumber;
            existingSong.OriginalAlbumTitle = song.OriginalAlbumTitle;
            existingSong.OriginalAlbumArtist = song.OriginalAlbumArtist;
            existingSong.OriginalDate = song.OriginalDate;
            existingSong.Genre = song.Genre;
            existingSong.Composer = song.Composer;
            existingSong.Publisher = song.Publisher;
            existingSong.Bpm = song.Bpm;

            this.Data.SaveChanges();

            song.Id = existingSong.Id;

            this.Data.Dispose();

            return this.Ok(song);
        }

        //// DELETE /api/music/albums/{albumId}/songs/{id}
        [HttpDelete]
        [Route("api/music/albums/{albumId}/songs/{id}")]
        public IHttpActionResult Delete(int albumId, int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var existingMusicAlbum = this.Data.MusicAlbums.Find(albumId);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingMusicAlbum.AuthorId)
            {
                return this.Unauthorized();
            }

            var existingSong = this.Data.Songs.All().FirstOrDefault(s => s.MusicAlbumId == albumId && s.Id == id);

            if (existingSong == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingSong.UploaderId)
            {
                return this.Unauthorized();
            }

            this.Data.Songs.Delete(existingSong);
            existingMusicAlbum.Songs.Remove(existingSong);

            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok(existingSong);
        }

        //// END - SONG Endpoints
    }
}