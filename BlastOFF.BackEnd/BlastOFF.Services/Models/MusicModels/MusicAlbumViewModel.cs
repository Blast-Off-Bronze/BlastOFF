namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Models.MusicModels;

    public class MusicAlbumViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int ViewsCount { get; set; }

        public string CoverImageData { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int FollowersCount { get; set; }

        public int SongsCount { get; set; }

        public IEnumerable<SongViewModel> Songs { get; set; }

        public static MusicAlbumViewModel Create(MusicAlbum a)
        {
            return new MusicAlbumViewModel
                       {
                           Id = a.Id,
                           Title = a.Title,
                           Author = a.Author.UserName,
                           DateCreated = a.DateCreated,
                           CoverImageData = a.CoverImageData,
                           ViewsCount = a.ViewsCount,
                           LikesCount = a.UserLikes.Count,
                           CommentsCount = a.Comments.Count,
                           FollowersCount = a.Followers.Count,
                           SongsCount = a.Songs.Count,
                           Songs = a.Songs.Select(SongViewModel.Create).Take(3)
                       };
        }
    }
}