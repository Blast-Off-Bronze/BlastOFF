namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Models;
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
        public IHttpActionResult AllMusicAlbums()
        {
            var albums = this.Data.MusicAlbums.All().Select(MusicAlbumViewModel.Get);

            this.Data.Dispose();

            return this.Ok(albums);
        }

        //// GET /api/music/albums/{id}
        [HttpGet]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult FindMusicAlbumById(int id)
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
        public IHttpActionResult AddMusicAlbum(MusicAlbumBindingModel musicAlbum)
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
        public IHttpActionResult UpdateMusicAlbum(int id, MusicAlbumBindingModel musicAlbum)
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
        public IHttpActionResult DeleteMusicAlbum(int id)
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

        // START - MUSIC ALBUM (LIKES) Endpoints

        //// POST /api/music/albums/{id}/likes
        [HttpPost]
        [Route("api/music/albums/{id}/likes")]
        public IHttpActionResult LikeMusicAlbum(int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = album.UserLikes.Any(u => u.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this music album.");
            }

            if (album.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot like your own music album.");
            }

            album.UserLikes.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully liked.", album.Title, album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/likes
        [HttpDelete]
        [Route("api/music/albums/{id}/likes")]
        public IHttpActionResult UnlikeMusicAlbum(int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = album.UserLikes.Any(u => u.Id == loggedUserId);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this music album.");
            }

            if (album.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot unlike your own music album.");
            }

            album.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully unliked.", album.Title, album.Author.UserName));
        }

        // END - MUSIC ALBUM (LIKES) Endpoints

        // START - MUSIC ALBUM (FOLLOWERS) Endpoints

        //// POST /api/music/albums/{id}/follow
        [HttpPost]
        [Route("api/music/albums/{id}/follow")]
        public IHttpActionResult FollowMusicAlbum(int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var isAlreadyFollowed = album.Followers.Any(u => u.Id == loggedUserId);

            if (isAlreadyFollowed)
            {
                return this.BadRequest("You are currently following this music album.");
            }

            if (album.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot follow your own music album.");
            }

            album.Followers.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully followed.", album.Title, album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/follow
        [HttpDelete]
        [Route("api/music/albums/{id}/follow")]
        public IHttpActionResult UnfollowMusicAlbum(int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var isAlreadyFollowed = album.Followers.Any(u => u.Id == loggedUserId);

            if (!isAlreadyFollowed)
            {
                return this.BadRequest("You are currently not following this music album.");
            }

            if (album.AuthorId == loggedUserId)
            {
                return this.BadRequest("Cannot unfollow your own music album.");
            }

            album.Followers.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("Music Album {0}, created by {1} successfully unfollowed.", album.Title, album.Author.UserName));
        }

        // END - MUSIC ALBUM (FOLLOWERS) Endpoints

        // START - MUSIC ALBUM (COMMENTS) Endpoints

        //// GET /api/music/albums/{id}/comments
        [HttpGet]
        [Route("api/music/albums/{id}/comments")]
        public IHttpActionResult AllMusicAlbumComments(int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var comments = this.Data.Comments.All().Where(c => c.MusicAlbumId == id).Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// GET /api/music/albums/{albumId}/comments/{id}
        [HttpGet]
        [Route("api/music/albums/{albumId}/comments/{id}")]
        public IHttpActionResult FindMusicAlbumCommentById(int albumId, int id)
        {
            var album = this.Data.MusicAlbums.Find(albumId);

            if (album == null)
            {
                return this.NotFound();
            }

            var comment = this.Data.Comments.All()
                    .Where(c => c.MusicAlbumId == albumId && c.Id == id)
                    .Select(CommentViewModel.Get)
                    .FirstOrDefault();

            if (comment == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(comment);
        }

        //// POST /api/music/albums/{albumId}/comments
        [HttpPost]
        [Route("api/music/albums/{albumId}/comments")]
        public IHttpActionResult AddMusicAlbumComment(int albumId, CommentBindingModel comment)
        {
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
                MusicAlbumId = albumId
            };

            if (album.Comments.Any(c => c == newComment))
            {
                return this.BadRequest(string.Format("This comment already exists for the music album."));
            }

            this.Data.Comments.Add(newComment);
            comment.Id = newComment.Id;

            album.Comments.Add(newComment);
            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(comment);
        }

        //// PUT /api/music/albums/{albumId}/comments/{id}
        [HttpPut]
        [Route("api/music/albums/{albumId}/comments/{id}")]
        public IHttpActionResult UpdateMusicAlbumComment(int albumId, int id, CommentBindingModel comment)
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

            var existingComment = this.Data.Comments.All().FirstOrDefault(c => c.MusicAlbumId == albumId && c.Id == id);

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
            
            this.Data.SaveChanges();

            comment.Id = existingComment.Id;

            this.Data.Dispose();

            return this.Ok(comment);
        }

        //// DELETE /api/music/albums/{albumId}/comments/{id}
        [HttpDelete]
        [Route("api/music/albums/{albumId}/comments/{id}")]
        public IHttpActionResult DeleteMusicAlbumComment(int albumId, int id)
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

            var existingComment = this.Data.Comments.All().FirstOrDefault(c => c.MusicAlbumId == albumId && c.Id == id);

            if (existingComment == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingComment.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.Comments.Delete(existingComment);
            existingMusicAlbum.Comments.Remove(existingComment);

            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok(existingComment);
        }

        //// END - MUSIC ALBUM (COMMENTS) Endpoints

        // START - MUSIC ALBUM (COMMENT LIKES) Endpoints

        //// POST /api/music/albums/{albumId}/comment/{id}/likes
        [HttpPost]
        [Route("api/music/albums/{albumId}/comment/{id}/likes")]
        public IHttpActionResult LikeMusicAlbumComment(int albumId, int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var comment = this.Data.Comments.All().FirstOrDefault(c => c.MusicAlbumId == albumId && c.Id == id);

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

        //// DELETE /api/music/albums/{albumId}/comments/{id}/likes
        [HttpDelete]
        [Route("api/music/albums/{albumId}/comments/{id}/likes")]
        public IHttpActionResult UnlikeMusicAlbumComment(int albumId, int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var comment = this.Data.Comments.All().FirstOrDefault(c => c.MusicAlbumId == albumId && c.Id == id);

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

        // END - MUSIC ALBUM (COMMENT LIKES) Endpoints











        // START - SONG Endpoints

        //// GET /api/music/albums/{albumId}/songs
        [HttpGet]
        [Route("api/music/albums/{albumId}/songs")]
        public IHttpActionResult AllSongs(int albumId)
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
        public IHttpActionResult FindSongById(int albumId, int id)
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
        public IHttpActionResult AddSong(int albumId, SongBindingModel song)
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
                UploaderId = album.AuthorId,
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
        public IHttpActionResult UpdateSong(int albumId, int id, SongBindingModel song)
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
        public IHttpActionResult DeleteSong(int albumId, int id)
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

        // START - SONG (LIKES) Endpoints

        //// POST /api/music/albums/{albumId}/song/{id}/likes
        [HttpPost]
        [Route("api/music/albums/{albumId}/song/{id}/likes")]
        public IHttpActionResult LikeSong(int albumId, int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var song = this.Data.Songs.All().FirstOrDefault(s => s.MusicAlbumId == albumId && s.Id == id);

            if (song == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = song.UserLikes.Any(u => u.Id == loggedUserId);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this song.");
            }

            if (song.UploaderId == loggedUserId)
            {
                return this.BadRequest("Cannot like your own songs.");
            }

            song.UserLikes.Add(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully liked.", song.Title, song.Uploader.UserName));
        }

        //// DELETE /api/music/albums/{albumId}/song/{id}/likes
        [HttpDelete]
        [Route("api/music/albums/{albumId}/song/{id}/likes")]
        public IHttpActionResult UnlikeSong(int albumId, int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(loggedUserId))
            {
                return this.BadRequest("You have to be logged in to continue.");
            }

            var currentUser = this.Data.Users.All().FirstOrDefault(u => u.Id == loggedUserId);

            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var song = this.Data.Songs.All().FirstOrDefault(s => s.MusicAlbumId == albumId && s.Id == id);

            if (song == null)
            {
                return this.NotFound();
            }

            var isAlreadyLiked = song.UserLikes.Any(u => u.Id == loggedUserId);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this song.");
            }

            if (song.UploaderId == loggedUserId)
            {
                return this.BadRequest("Cannot unlike your own songs.");
            }

            song.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();
            this.Data.Dispose();

            return this.Ok(string.Format("{0}, uploaded by {1} successfully unliked.", song.Title, song.Uploader.UserName));
        }

        // END - SONG (LIKES) Endpoints

    }
}