namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.MusicModels;
    using BlastOFF.Services.Services;
    using BlastOFF.Services.UserSessionUtils;

    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;

    using Microsoft.AspNet.Identity;

    using Comment = BlastOFF.Models.Comment;
    using File = Google.Apis.Drive.v2.Data.File;

    [SessionAuthorize]
    public class MusicController : BaseApiController
    {
        protected const int SongKilobyteLimit = 20480;

        public MusicController()
            : this(new BlastOFFData())
        {
        }

        public MusicController(IBlastOFFData data)
            : base(data)
        {
        }

        //// ALL

        //// GET /api/music/albums
        [HttpGet]
        [Route("api/music/albums")]
        [AllowAnonymous]
        public IHttpActionResult AllMusicAlbums()
        {
            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var albums = this.Data.MusicAlbums
                .All()
                .Where(a => a.IsPublic || a.AuthorId == currentUser.Id)
                .OrderBy(a => a.DateCreated)
                .ToList()
                .Select(MusicAlbumViewModel.Create);

            return this.Ok(albums);
        }









        //// GET /api/music/albums/{id}/songs
        [HttpGet]
        [Route("api/music/albums/{id}/songs")]
        [AllowAnonymous]
        public IHttpActionResult AllSongs([FromUri] int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var songs = album.Songs.Select(s => SongViewModel.Create(s));

            this.Data.Dispose();

            return this.Ok(songs);
        }

        //// GET /api/music/albums/{id}/comments
        [HttpGet]
        [Route("api/music/albums/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllMusicAlbumComments([FromUri] int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var comments = album.Comments.Select(c => CommentViewModel.Create(c));

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// GET /api/songs/{id}/comments
        [HttpGet]
        [Route("api/songs/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllSongComments([FromUri] int id)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var comments = song.Comments.Select(c => CommentViewModel.Create(c));

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// BY ID

        //// GET /api/music/albums/{id}
        [HttpGet]
        [Route("api/music/albums/{id}")]
        [AllowAnonymous]
        public IHttpActionResult FindMusicAlbumById([FromUri] int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            var albumToReturn = MusicAlbumViewModel.Create(album);

            return this.Ok(albumToReturn);
        }

        //// GET /api/songs/{id}
        [HttpGet]
        [Route("api/songs/{id}")]
        [AllowAnonymous]
        public IHttpActionResult FindSongById([FromUri] int id)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            var songToReturn = SongViewModel.Create(song);

            return this.Ok(songToReturn);
        }

        //// ADD

        //// POST /api/music/albums
        [HttpPost]
        [Route("api/music/albums")]
        public IHttpActionResult AddMusicAlbum([FromBody] MusicAlbumBindingModel musicAlbum)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (musicAlbum == null)
            {
                return this.BadRequest("Cannot create an empty music album model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (musicAlbum.CoverImageData != null && musicAlbum.CoverImageData.IndexOf(',') == -1)
            {
                musicAlbum.CoverImageData = string.Format("{0}{1}", "data:image/jpg;base64,", musicAlbum.CoverImageData);
            }

            var newMusicAlbum = new MusicAlbum
                                    {
                                        Title = musicAlbum.Title, 
                                        AuthorId = user.Id, 
                                        DateCreated = DateTime.Now, 
                                        CoverImageData = musicAlbum.CoverImageData
                                    };

            if (
                this.Data.MusicAlbums.All()
                    .Any(a => a.Title == newMusicAlbum.Title && a.AuthorId == newMusicAlbum.AuthorId))
            {
                return this.BadRequest(string.Format("This music album already exists."));
            }

            this.Data.MusicAlbums.Add(newMusicAlbum);
            this.Data.SaveChanges();

            this.Data.Dispose();

            var musicAlbumToReturn = MusicAlbumViewModel.Create(newMusicAlbum);

            return this.Ok(musicAlbumToReturn);
        }

        //// POST /api/music/albums/{id}/songs
        [HttpPost]
        [Route("api/music/albums/{id}/songs")]
        public IHttpActionResult AddSong([FromUri] int id, [FromBody] SongBindingModel song)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var album = this.Data.MusicAlbums.Find(id);

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

            if (user.Id != album.AuthorId)
            {
                return this.Unauthorized();
            }

            if (!this.ValidateAudioFileType(song.FileDataUrl))
            {
                return this.BadRequest("Invalid file type. Valid file type includes .mp3 only");
            }

            if (!this.ValidateFileSize(song.FileDataUrl, SongKilobyteLimit))
            {
                return this.BadRequest(string.Format("Song size should be less than {0} kB.", SongKilobyteLimit));
            }

            var metadataStart = song.FileDataUrl.IndexOf("data:audio/mp3;base64,");

            if (metadataStart != -1)
            {
                song.FileDataUrl = song.FileDataUrl.Remove(metadataStart, metadataStart + 22);
            }

            string googleDriveFileName = song.Artist + " - " + song.Title + ".mp3";
            song.FileDataUrl = this.UploadSongToGoogleDrive(song.FileDataUrl, googleDriveFileName);

            var newSong = new Song
                              {
                                  Title = song.Title, 
                                  Artist = song.Artist, 
                                  FilePath = song.FileDataUrl, 
                                  MusicAlbumId = int.Parse(song.MusicAlbumId), 
                                  UploaderId = album.AuthorId, 
                                  DateAdded = DateTime.Now, 
                                  TrackNumber = song.TrackNumber == null ? (int?)null : int.Parse(song.TrackNumber), 
                                  OriginalAlbumTitle = song.OriginalAlbumTitle, 
                                  OriginalAlbumArtist = song.OriginalAlbumArtist, 
                                  OriginalDate =
                                      song.OriginalDate == null ? (DateTime?)null : DateTime.Parse(song.OriginalDate), 
                                  Genre = song.Genre, 
                                  Composer = song.Composer, 
                                  Publisher = song.Publisher, 
                                  Bpm = song.Bpm == null ? (int?)null : int.Parse(song.Bpm)
                              };

            if (album.Songs.Contains(newSong))
            {
                return this.BadRequest(string.Format("This song already exists in album."));
            }

            this.Data.Songs.Add(newSong);
            this.Data.SaveChanges();

            var songToReturn = SongViewModel.Create(newSong);

            this.Data.Dispose();

            return this.Ok(songToReturn);
        }

        private bool ValidateAudioFileType(string fileDataUrl)
        {
            if (fileDataUrl.IndexOf("data:audio/mp3;base64,") == -1)
            {
                return false;
            }

            return true;
        }

        private bool ValidateFileSize(string fileDataUrl, int kilobyteLimit)
        {
            if ((fileDataUrl.Length / 4) * 3 >= kilobyteLimit * 1024)
            {
                return false;
            }

            return true;
        }

        private string UploadSongToGoogleDrive(string fileDataUrl, string fileName)
        {
            const string AudioMimeType = "audio/mpeg";

            byte[] byteArray = Convert.FromBase64String(fileDataUrl);
            MemoryStream stream = new System.IO.MemoryStream(byteArray);

            var service = GoogleDriveService.Get();

            File body = new File();
            body.Title = fileName;
            body.MimeType = AudioMimeType;
            body.Parents = new List<ParentReference>
                               {
                                   new ParentReference
                                       {
                                           Id =
                                               MusicConstants
                                               .GoogleDriveBlastOFFMusicFolderId
                                       }
                               };

            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, AudioMimeType);
                request.Upload();

                return "https://drive.google.com/open?id=" + request.ResponseBody.Id;
            }
            catch (Exception exception)
            {
                return string.Format("An error occurred: " + exception.Message);
            }
        }

        //// POST /api/music/albums/{id}/comments
        [HttpPost]
        [Route("api/music/albums/{id}/comments")]
        public IHttpActionResult AddMusicAlbumComment([FromUri] int id, [FromBody] CommentCreateBindingModel comment)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var album = this.Data.MusicAlbums.Find(id);

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

            var newMusicAlbumComment = new Comment
                                           {
                                               Content = comment.Content, 
                                               AuthorId = user.Id, 
                                               PostedOn = DateTime.Now, 
                                               MusicAlbumId = id
                                           };

            this.Data.Comments.Add(newMusicAlbumComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newMusicAlbumComment);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// POST /api/songs/{id}/comments
        [HttpPost]
        [Route("api/songs/{id}/comments")]
        public IHttpActionResult AddSongComment([FromUri] int id, [FromBody] CommentCreateBindingModel comment)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var song = this.Data.Songs.Find(id);

            if (song == null)
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

            var newSongComment = new Comment
                                     {
                                         Content = comment.Content, 
                                         AuthorId = user.Id, 
                                         PostedOn = DateTime.Now, 
                                         MusicAlbumId = id
                                     };

            this.Data.Comments.Add(newSongComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newSongComment);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// UPDATE

        //// PUT /api/music/albums/{id}
        [HttpPut]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult UpdateMusicAlbum([FromUri] int id, [FromBody] MusicAlbumBindingModel musicAlbum)
        {
            var existingMusicAlbum = this.Data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            string loggedUserId = this.User.Identity.GetUserId();

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

            var musicAlbumToReturn = MusicAlbumViewModel.Create(existingMusicAlbum);

            this.Data.Dispose();

            return this.Ok(musicAlbumToReturn);
        }

        //// PUT /api/songs/{id}
        [HttpPut]
        [Route("api/songs/{id}")]
        public IHttpActionResult UpdateSong([FromUri] int id, [FromBody] SongBindingModel song)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var existingSong = this.Data.Songs.Find(id);

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
            existingSong.DateAdded = DateTime.Now;
            existingSong.TrackNumber = song.TrackNumber == null ? (int?)null : int.Parse(song.TrackNumber);
            existingSong.OriginalAlbumTitle = song.OriginalAlbumTitle;
            existingSong.OriginalAlbumArtist = song.OriginalAlbumArtist;
            existingSong.OriginalDate = song.OriginalDate == null ? (DateTime?)null : DateTime.Parse(song.OriginalDate);
            existingSong.Genre = song.Genre;
            existingSong.Composer = song.Composer;
            existingSong.Publisher = song.Publisher;
            existingSong.Bpm = song.Bpm == null ? (int?)null : int.Parse(song.Bpm);

            this.Data.SaveChanges();

            var songToReturn = SongViewModel.Create(existingSong);

            this.Data.Dispose();

            return this.Ok(songToReturn);
        }

        //// DELETE

        //// DELETE /api/music/albums/{id}
        [HttpDelete]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult DeleteMusicAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var existingMusicAlbum = this.Data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingMusicAlbum.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.MusicAlbums.Delete(existingMusicAlbum);
            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok();
        }

        //// DELETE /api/songs/{id}
        [HttpDelete]
        [Route("api/songs/{id}")]
        public IHttpActionResult DeleteSong([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var existingSong = this.Data.Songs.Find(id);

            if (existingSong == null)
            {
                return this.NotFound();
            }

            if (loggedUserId != existingSong.UploaderId)
            {
                return this.Unauthorized();
            }

            this.Data.Songs.Delete(existingSong);
            this.Data.SaveChanges();

            this.Data.Dispose();

            return this.Ok();
        }

        //// LIKE - UNLIKE

        //// POST /api/music/albums/{id}/likes
        [HttpPost]
        [Route("api/music/albums/{id}/likes")]
        public IHttpActionResult LikeMusicAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

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

            return
                this.Ok(
                    string.Format(
                        "Music Album {0}, created by {1}, successfully liked.", 
                        album.Title, 
                        album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/likes
        [HttpDelete]
        [Route("api/music/albums/{id}/likes")]
        public IHttpActionResult UnlikeMusicAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

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

            return
                this.Ok(
                    string.Format(
                        "Music Album {0}, created by {1}, successfully unliked.", 
                        album.Title, 
                        album.Author.UserName));
        }

        //// POST /api/songs/{id}/likes
        [HttpPost]
        [Route("api/songs/{id}/likes")]
        public IHttpActionResult LikeSong([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

            var song = this.Data.Songs.Find(id);

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

            return
                this.Ok(string.Format("{0}, uploaded by {1}, successfully liked.", song.Title, song.Uploader.UserName));
        }

        //// DELETE /api/songs/{id}/likes
        [HttpDelete]
        [Route("api/songs/{id}/likes")]
        public IHttpActionResult UnlikeSong([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(id);

            var song = this.Data.Songs.Find(id);

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

            return
                this.Ok(
                    string.Format("{0}, uploaded by {1}, successfully unliked.", song.Title, song.Uploader.UserName));
        }

        // FOLLOWERS

        //// POST /api/music/albums/{id}/follow
        [HttpPost]
        [Route("api/music/albums/{id}/follow")]
        public IHttpActionResult FollowMusicAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

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

            return
                this.Ok(
                    string.Format(
                        "Music Album {0}, created by {1}, successfully followed.", 
                        album.Title, 
                        album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/follow
        [HttpDelete]
        [Route("api/music/albums/{id}/follow")]
        public IHttpActionResult UnfollowMusicAlbum([FromUri] int id)
        {
            string loggedUserId = this.User.Identity.GetUserId();

            var currentUser = this.Data.Users.Find(loggedUserId);

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

            return
                this.Ok(
                    string.Format(
                        "Music Album {0}, created by {1}, successfully unfollowed.", 
                        album.Title, 
                        album.Author.UserName));
        }
    }
}