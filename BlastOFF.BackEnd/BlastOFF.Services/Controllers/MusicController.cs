namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Services.Constants;
    using BlastOFF.Services.Models.CommentModels;
    using BlastOFF.Services.Models.MusicModels;
    using BlastOFF.Services.Services;
    using BlastOFF.Services.UserSessionUtils;
    using BlastOFF.Services.Models.UserModels;

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
        public IHttpActionResult AllMusicAlbums([FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var albums = this.Data.MusicAlbums.All()
                    .Where(a => a.IsPublic || a.AuthorId == currentUser.Id)
                    .OrderBy(a => a.DateCreated)
                    .Skip(CurrentPage * PageSize)
                    .Take(PageSize)
                    .ToList()
                    .Select(a => MusicAlbumViewModel.Create(a, currentUser));

            return this.Ok(albums);
        }

        //// GET /api/music/albums/{id}/songs
        [HttpGet]
        [Route("api/music/albums/{id}/songs")]
        [AllowAnonymous]
        public IHttpActionResult AllSongs([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var songs = album.Songs
                .OrderBy(s => s.DateAdded)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(s => SongViewModel.Create(s, currentUser));

            return this.Ok(songs);
        }

        //// GET /api/music/albums/{id}/comments
        [HttpGet]
        [Route("api/music/albums/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllMusicAlbumComments([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var comments = album.Comments
                .OrderByDescending(c => c.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(a => CommentViewModel.Create(a, currentUser));

            return this.Ok(comments);
        }

        //// GET /api/songs/{id}/comments
        [HttpGet]
        [Route("api/songs/{id}/comments")]
        [AllowAnonymous]
        public IHttpActionResult AllSongComments([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var comments = song.Comments
                .OrderByDescending(c => c.PostedOn)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(a => CommentViewModel.Create(a, currentUser));

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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var albumToReturn = MusicAlbumViewModel.Create(album, currentUser);

            return this.Ok(albumToReturn);
        }

        [HttpGet]
        [Route("api/music/albums/{id}/likes")]
        [AllowAnonymous]
        public IHttpActionResult MusicAlbumLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var userLikes = album.UserLikes
                            .OrderByDescending(u => u.UserName)
                            .Skip(CurrentPage * PageSize)
                            .Take(PageSize)
                            .ToList()
                            .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(userLikes);
        }

        [HttpGet]
        [Route("api/music/albums/{id}/followers")]
        [AllowAnonymous]
        public IHttpActionResult MusicAlbumFollowers([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var followers = album.Followers
                            .OrderByDescending(u => u.UserName)
                            .Skip(CurrentPage * PageSize)
                            .Take(PageSize)
                            .ToList()
                            .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(followers);
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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var songToReturn = SongViewModel.Create(song, currentUser);

            return this.Ok(songToReturn);
        }

        [HttpGet]
        [Route("api/songs/{id}/likes")]
        [AllowAnonymous]
        public IHttpActionResult SongLikes([FromUri] int id, [FromUri] int CurrentPage = MainConstants.DefaultPage,
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var userLikes = song.UserLikes
                .OrderByDescending(u => u.UserName)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(u => UserPreviewViewModel.Create(u, currentUser));

            return this.Ok(userLikes);
        }

        //// ADD

        //// POST /api/music/albums
        [HttpPost]
        [Route("api/music/albums")]
        public IHttpActionResult AddMusicAlbum([FromBody] MusicAlbumBindingModel musicAlbum)
        {
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

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var newMusicAlbum = new MusicAlbum
                                    {
                                        Title = musicAlbum.Title,
                                        AuthorId = user.Id,
                                        IsPublic = musicAlbum.IsPublic,
                                        DateCreated = DateTime.Now,
                                        CoverImageData = musicAlbum.CoverImageData
                                    };

            if (
                this.Data.MusicAlbums.All()
                    .Any(a => a.Title == newMusicAlbum.Title && a.AuthorId == newMusicAlbum.AuthorId))
            {
                return this.BadRequest("This music album already exists.");
            }

            this.Data.MusicAlbums.Add(newMusicAlbum);
            this.Data.SaveChanges();

            var musicAlbumToReturn = MusicAlbumViewModel.Create(newMusicAlbum, user);

            return this.Ok(musicAlbumToReturn);
        }

        //// POST /api/music/albums/{id}/songs
        [HttpPost]
        [Route("api/music/albums/{id}/songs")]
        public IHttpActionResult AddSong([FromUri] int id, [FromBody] SongBindingModel song)
        {
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

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

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

            var metadataStart = song.FileDataUrl.IndexOf("data:audio/mp3;base64,", StringComparison.Ordinal);

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
                return this.BadRequest("This song already exists in album.");
            }

            this.Data.Songs.Add(newSong);
            this.Data.SaveChanges();

            var songToReturn = SongViewModel.Create(newSong, user);

            return this.Ok(songToReturn);
        }

        //// POST /api/music/albums/{id}/comments
        [HttpPost]
        [Route("api/music/albums/{id}/comments")]
        public IHttpActionResult AddMusicAlbumComment([FromUri] int id, [FromBody] CommentCreateBindingModel comment)
        {
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

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var newMusicAlbumComment = new Comment
                                           {
                                               Content = comment.Content,
                                               AuthorId = user.Id,
                                               PostedOn = DateTime.Now,
                                               MusicAlbumId = id
                                           };

            this.Data.Comments.Add(newMusicAlbumComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newMusicAlbumComment, user);

            return this.Ok(commentToReturn);
        }

        //// POST /api/songs/{id}/comments
        [HttpPost]
        [Route("api/songs/{id}/comments")]
        public IHttpActionResult AddSongComment([FromUri] int id, [FromBody] CommentCreateBindingModel comment)
        {
            if (comment == null)
            {
                return this.BadRequest("Cannot create an empty comment model.");
            }

            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var newSongComment = new Comment
                                     {
                                         Content = comment.Content,
                                         AuthorId = user.Id,
                                         PostedOn = DateTime.Now,
                                         SongId = id
                                     };

            this.Data.Comments.Add(newSongComment);
            this.Data.SaveChanges();

            var commentToReturn = CommentViewModel.Create(newSongComment, user);

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

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id != existingMusicAlbum.AuthorId)
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

            var musicAlbumToReturn = MusicAlbumViewModel.Create(existingMusicAlbum, currentUser);

            return this.Ok(musicAlbumToReturn);
        }

        //// PUT /api/songs/{id}
        [HttpPut]
        [Route("api/songs/{id}")]
        public IHttpActionResult UpdateSong([FromUri] int id, [FromBody] SongBindingModel song)
        {
            var existingSong = this.Data.Songs.Find(id);

            if (existingSong == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id != existingSong.UploaderId)
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

            var songToReturn = SongViewModel.Create(existingSong, currentUser);

            return this.Ok(songToReturn);
        }

        //// DELETE

        //// DELETE /api/music/albums/{id}
        [HttpDelete]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult DeleteMusicAlbum([FromUri] int id)
        {
            var existingMusicAlbum = this.Data.MusicAlbums.Find(id);

            if (existingMusicAlbum == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id != existingMusicAlbum.AuthorId)
            {
                return this.Unauthorized();
            }

            this.Data.MusicAlbums.Delete(existingMusicAlbum);
            this.Data.SaveChanges();

            return this.Ok();
        }

        //// DELETE /api/songs/{id}
        [HttpDelete]
        [Route("api/songs/{id}")]
        public IHttpActionResult DeleteSong([FromUri] int id)
        {
            var existingSong = this.Data.Songs.Find(id);

            if (existingSong == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id != existingSong.UploaderId)
            {
                return this.Unauthorized();
            }

            this.Data.Songs.Delete(existingSong);
            this.Data.SaveChanges();

            return this.Ok();
        }

        //// LIKE - UNLIKE

        //// POST /api/music/albums/{id}/likes
        [HttpPost]
        [Route("api/music/albums/{id}/likes")]
        public IHttpActionResult LikeMusicAlbum([FromUri] int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = album.UserLikes.Any(u => u.Id == currentUser.Id);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this music album.");
            }

            if (album.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot like your own music album.");
            }

            album.UserLikes.Add(currentUser);

            this.Data.SaveChanges();

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
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = album.UserLikes.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this music album.");
            }

            if (album.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot unlike your own music album.");
            }

            album.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();

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
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = song.UserLikes.Any(u => u.Id == currentUser.Id);

            if (isAlreadyLiked)
            {
                return this.BadRequest("You have already liked this song.");
            }

            if (song.UploaderId == currentUser.Id)
            {
                return this.BadRequest("Cannot like your own songs.");
            }

            song.UserLikes.Add(currentUser);

            this.Data.SaveChanges();

            return
                this.Ok(string.Format("{0}, uploaded by {1}, successfully liked.", song.Title, song.Uploader.UserName));
        }

        //// DELETE /api/songs/{id}/likes
        [HttpDelete]
        [Route("api/songs/{id}/likes")]
        public IHttpActionResult UnlikeSong([FromUri] int id)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyLiked = song.UserLikes.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyLiked)
            {
                return this.BadRequest("You have already unliked this song.");
            }

            if (song.UploaderId == currentUser.Id)
            {
                return this.BadRequest("Cannot unlike your own songs.");
            }

            song.UserLikes.Remove(currentUser);

            this.Data.SaveChanges();

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
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyFollowed = album.Followers.Any(u => u.Id == currentUser.Id);

            if (isAlreadyFollowed)
            {
                return this.BadRequest("You are currently following this music album.");
            }

            if (album.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot follow your own music album.");
            }

            album.Followers.Add(currentUser);

            this.Data.SaveChanges();

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
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            var isAlreadyFollowed = album.Followers.Any(u => u.Id == currentUser.Id);

            if (!isAlreadyFollowed)
            {
                return this.BadRequest("You are currently not following this music album.");
            }

            if (album.AuthorId == currentUser.Id)
            {
                return this.BadRequest("Cannot unfollow your own music album.");
            }

            album.Followers.Remove(currentUser);

            this.Data.SaveChanges();

            return
                this.Ok(
                    string.Format(
                        "Music Album {0}, created by {1}, successfully unfollowed.",
                        album.Title,
                        album.Author.UserName));
        }

        private bool ValidateAudioFileType(string fileDataUrl)
        {
            if (fileDataUrl.IndexOf("data:audio/mp3;base64,", StringComparison.Ordinal) == -1)
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
            MemoryStream stream = new MemoryStream(byteArray);

            var service = GoogleDriveService.Get();

            File body = new File
                            {
                                Title = fileName,
                                MimeType = AudioMimeType,
                                Parents =
                                    new List<ParentReference>
                                        {
                                            new ParentReference
                                                {
                                                    Id =
                                                        MusicConstants
                                                        .GoogleDriveBlastOFFMusicFolderId
                                                }
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
    }
}