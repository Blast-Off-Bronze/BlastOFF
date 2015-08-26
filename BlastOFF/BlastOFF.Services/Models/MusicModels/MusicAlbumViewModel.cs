namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using BlastOFF.Models.MusicModels;

    public class MusicAlbumViewModel
    {
        public static Expression<Func<MusicAlbum, MusicAlbumViewModel>> Get
        {
            get
            {
                return a => new MusicAlbumViewModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Author = a.Author.UserName,
                            AuthorId = a.Author.Id,
                            DateCreated = a.DateCreated,
                            ViewsCount = a.ViewsCount,
                            CoverImageData = a.CoverImageData,
                            LikesCount = a.UserLikes.Count,
                            CommentsCount = a.Comments.Count,
                            FollowersCount = a.Followers.Count,
                            SongsCount = a.Songs.Count,
                            Songs = a.Songs.Select(s => s.Title).Take(3)
                        };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int ViewsCount { get; set; }

        public string CoverImageData { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int FollowersCount { get; set; }

        public int SongsCount { get; set; }

        public IEnumerable<string> Songs { get; set; }
    }
}