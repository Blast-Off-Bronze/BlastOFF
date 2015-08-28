﻿namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using BlastOFF.Models;
    using BlastOFF.Models.MusicModels;
    using Data;
    using Data.Interfaces;
    using Models;
    using Models.MusicModels;

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

        //// ALL

        //// GET /api/music/albums
        [HttpGet]
        [Route("api/music/albums")]
        public IHttpActionResult AllMusicAlbums()
        {
            var albums = this.Data.MusicAlbums.All().Select(MusicAlbumViewModel.Get);

            this.Data.Dispose();

            return this.Ok(albums);
        }

        //// GET /api/music/albums/{id}/songs
        [HttpGet]
        [Route("api/music/albums/{id}/songs")]
        public IHttpActionResult AllSongs([FromUri]int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var songs = album.Songs.AsQueryable().Select(SongViewModel.Get);

            this.Data.Dispose();

            return this.Ok(songs);
        }

        //// GET /api/music/albums/{id}/comments
        [HttpGet]
        [Route("api/music/albums/{id}/comments")]
        public IHttpActionResult AllMusicAlbumComments([FromUri]int id)
        {
            var album = this.Data.MusicAlbums.Find(id);

            if (album == null)
            {
                return this.NotFound();
            }

            var comments = album.Comments.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// GET /api/songs/{id}/comments
        [HttpGet]
        [Route("api/songs/{id}/comments")]
        public IHttpActionResult AllSongComments([FromUri]int id)
        {
            var song = this.Data.Songs.Find(id);

            if (song == null)
            {
                return this.NotFound();
            }

            var comments = song.Comments.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(comments);
        }

        //// BY ID

        //// GET /api/music/albums/{id}
        [HttpGet]
        [Route("api/music/albums/{id}")]
        public IHttpActionResult FindMusicAlbumById([FromUri]int id)
        {
            var musicAlbumCollection = new List<MusicAlbum> { this.Data.MusicAlbums.Find(id) };

            var album = musicAlbumCollection.AsQueryable().Select(MusicAlbumViewModel.Get);

            if (album == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(album);
        }

        //// GET /api/songs/{id}
        [HttpGet]
        [Route("api/songs/{id}")]
        public IHttpActionResult FindSongById([FromUri]int id)
        {
            var songCollection = new List<Song> { this.Data.Songs.Find(id) };

            var song = songCollection.AsQueryable().Select(SongViewModel.Get);

            if (song == null)
            {
                return this.NotFound();
            }

            this.Data.Dispose();

            return this.Ok(song);
        }

        //// ADD

        //// POST /api/music/albums
        [HttpPost]
        [Route("api/music/albums")]
        [Authorize]
        public IHttpActionResult AddMusicAlbum([FromBody]MusicAlbumBindingModel musicAlbum)
        {
            string loggedUserId = this.User.Identity.GetUserId();

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

            if (this.Data.MusicAlbums.All().Any(a => a == newMusicAlbum))
            {
                return this.BadRequest(string.Format("This music album already exists."));
            }

            this.Data.MusicAlbums.Add(newMusicAlbum);
            this.Data.SaveChanges();

            musicAlbum.Id = newMusicAlbum.Id;

            var musicAlbumCollection = new List<MusicAlbum> { newMusicAlbum };

            var musicAlbumToReturn = musicAlbumCollection.AsQueryable().Select(MusicAlbumViewModel.Get);

            this.Data.Dispose();

            return this.Ok(musicAlbumToReturn);
        }

        //// POST /api/music/albums/{id}/songs
        [HttpPost]
        [Route("api/music/albums/{id}/songs")]
        [Authorize]
        public IHttpActionResult AddSong([FromUri]int id, [FromBody]SongBindingModel song)
        {
            // TODO: Upload song to Google Drive and acquire link

            string loggedUserId = this.User.Identity.GetUserId();

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
            this.Data.SaveChanges();

            song.Id = newSong.Id;

            var songCollection = new List<Song> { newSong };

            var songToReturn = songCollection.AsQueryable().Select(SongViewModel.Get);

            this.Data.Dispose();

            return this.Ok(songToReturn);
        }

        //// POST /api/music/albums/{id}/comments
        [HttpPost]
        [Route("api/music/albums/{id}/comments")]
        [Authorize]
        public IHttpActionResult AddMusicAlbumComment([FromUri]int id, [FromBody]CommentBindingModel comment)
        {
            string loggedUserId = this.User.Identity.GetUserId();

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
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                MusicAlbumId = id
            };

            this.Data.Comments.Add(newMusicAlbumComment);
            this.Data.SaveChanges();

            comment.Id = newMusicAlbumComment.Id;

            var commentCollection = new List<Comment> { newMusicAlbumComment };

            var commentToReturn = commentCollection.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// POST /api/songs/{id}/comments
        [HttpPost]
        [Route("api/songs/{id}/comments")]
        [Authorize]
        public IHttpActionResult AddSongComment([FromUri]int id, [FromBody]CommentBindingModel comment)
        {
            string loggedUserId = this.User.Identity.GetUserId();

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
                AuthorId = loggedUserId,
                PostedOn = DateTime.Now,
                MusicAlbumId = id
            };

            this.Data.Comments.Add(newSongComment);
            this.Data.SaveChanges();

            comment.Id = newSongComment.Id;

            var commentCollection = new List<Comment> { newSongComment };

            var commentToReturn = commentCollection.AsQueryable().Select(CommentViewModel.Get);

            this.Data.Dispose();

            return this.Ok(commentToReturn);
        }

        //// UPDATE

        //// PUT /api/music/albums/{id}
        [HttpPut]
        [Route("api/music/albums/{id}")]
        [Authorize]
        public IHttpActionResult UpdateMusicAlbum([FromUri]int id, [FromBody]MusicAlbumBindingModel musicAlbum)
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

            var musicAlbumCollection = new List<MusicAlbum> { existingMusicAlbum };

            var musicAlbumToReturn = musicAlbumCollection.AsQueryable().Select(MusicAlbumViewModel.Get);

            this.Data.Dispose();

            return this.Ok(musicAlbumToReturn);
        }

        //// PUT /api/songs/{id}
        [HttpPut]
        [Route("api/songs/{id}")]
        [Authorize]
        public IHttpActionResult UpdateSong([FromUri]int id, [FromBody]SongBindingModel song)
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
            existingSong.TrackNumber = song.TrackNumber;
            existingSong.OriginalAlbumTitle = song.OriginalAlbumTitle;
            existingSong.OriginalAlbumArtist = song.OriginalAlbumArtist;
            existingSong.OriginalDate = song.OriginalDate;
            existingSong.Genre = song.Genre;
            existingSong.Composer = song.Composer;
            existingSong.Publisher = song.Publisher;
            existingSong.Bpm = song.Bpm;

            this.Data.SaveChanges();

            var songCollection = new List<Song> { existingSong };

            var songToReturn = songCollection.AsQueryable().Select(SongViewModel.Get);

            this.Data.Dispose();

            return this.Ok(songToReturn);
        }

        //// DELETE

        //// DELETE /api/music/albums/{id}
        [HttpDelete]
        [Route("api/music/albums/{id}")]
        [Authorize]
        public IHttpActionResult DeleteMusicAlbum([FromUri]int id)
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
        [Authorize]
        public IHttpActionResult DeleteSong([FromUri]int id)
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
        [Authorize]
        public IHttpActionResult LikeMusicAlbum([FromUri]int id)
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

            return this.Ok(string.Format("Music Album {0}, created by {1}, successfully liked.", album.Title, album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/likes
        [HttpDelete]
        [Route("api/music/albums/{id}/likes")]
        [Authorize]
        public IHttpActionResult UnlikeMusicAlbum([FromUri]int id)
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

            return this.Ok(string.Format("Music Album {0}, created by {1}, successfully unliked.", album.Title, album.Author.UserName));
        }

        //// POST /api/songs/{id}/likes
        [HttpPost]
        [Route("api/songs/{id}/likes")]
        [Authorize]
        public IHttpActionResult LikeSong([FromUri]int id)
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

            return this.Ok(string.Format("{0}, uploaded by {1}, successfully liked.", song.Title, song.Uploader.UserName));
        }

        //// DELETE /api/songs/{id}/likes
        [HttpDelete]
        [Route("api/songs/{id}/likes")]
        [Authorize]
        public IHttpActionResult UnlikeSong([FromUri]int id)
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

            return this.Ok(string.Format("{0}, uploaded by {1}, successfully unliked.", song.Title, song.Uploader.UserName));
        }

        // FOLLOWERS

        //// POST /api/music/albums/{id}/follow
        [HttpPost]
        [Route("api/music/albums/{id}/follow")]
        [Authorize]
        public IHttpActionResult FollowMusicAlbum([FromUri]int id)
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

            return this.Ok(string.Format("Music Album {0}, created by {1}, successfully followed.", album.Title, album.Author.UserName));
        }

        //// DELETE /api/music/albums/{id}/follow
        [HttpDelete]
        [Route("api/music/albums/{id}/follow")]
        [Authorize]
        public IHttpActionResult UnfollowMusicAlbum([FromUri]int id)
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

            return this.Ok(string.Format("Music Album {0}, created by {1}, successfully unfollowed.", album.Title, album.Author.UserName));
        }
    }
}