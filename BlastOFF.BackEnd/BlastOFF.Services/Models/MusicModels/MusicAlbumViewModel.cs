namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Models.MusicModels;
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Models.CommentModels;

    public class MusicAlbumViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }


        public bool IsOwn { get; set; }

        public bool IsFollowed { get; set; }

        public bool IsLiked { get; set; }


        public int ViewsCount { get; set; }

        public string CoverImageData { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public int FollowersCount { get; set; }

        public int SongsCount { get; set; }

        // Songs
        public IEnumerable<SongViewModel> Songs { get; set; }

        // Comments
        public IEnumerable<CommentViewModel> Comments { get; set; }


        public bool AllSongsDisplayed { get; set; }

        public static MusicAlbumViewModel Create(MusicAlbum a, ApplicationUser user)
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

                           Songs = a.Songs.OrderBy(s => s.DateAdded).Select(s => SongViewModel.Create(s, user)).Take(3),
                           Comments = a.Comments.OrderBy(c => c.PostedOn).Select(c => CommentViewModel.Create(c, user)).Take(3),

                           IsOwn = a.Author == user,
                           IsFollowed = a.Author != user && a.Followers.Contains(user),
                           IsLiked = a.Author != user && a.UserLikes.Contains(user),

                           AllSongsDisplayed = false
                       };
        }
    }
}