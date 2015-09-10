namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BlastOFF.Models.MusicModels;
    using BlastOFF.Models.UserModel;
    using BlastOFF.Services.Models.CommentModels;

    public class SongViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string FilePath { get; set; }

        public DateTime DateAdded { get; set; }

        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsOwn { get; set; }

        public bool IsLiked { get; set; }

        // Comments
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public bool AllCommentsDisplayed { get; set; }

        //// OPTIONAL
        public int? TrackNumber { get; set; }

        public string OriginalAlbumTitle { get; set; }

        public string OriginalAlbumArtist { get; set; }

        public DateTime? OriginalDate { get; set; }

        public string Genre { get; set; }

        public string Composer { get; set; }

        public string Publisher { get; set; }

        public int? Bpm { get; set; }

        public static SongViewModel Create(Song s, ApplicationUser user)
        {
            return new SongViewModel
                       {
                           Id = s.Id, 
                           Title = s.Title, 
                           Artist = s.Artist, 
                           FilePath = s.FilePath, 
                           DateAdded = s.DateAdded,
                           ViewsCount = s.ViewsCount, 
                           LikesCount = s.UserLikes.Count, 
                           CommentsCount = s.Comments.Count,

                           Comments = s.Comments.OrderByDescending(c => c.PostedOn).ToList().Select(c => CommentViewModel.Create(c, user)).Take(3),
                           AllCommentsDisplayed = false,

                           //Optional
                           TrackNumber = s.TrackNumber, 
                           OriginalAlbumTitle = s.OriginalAlbumTitle, 
                           OriginalAlbumArtist = s.OriginalAlbumArtist, 
                           OriginalDate = s.OriginalDate, 
                           Genre = s.Genre, 
                           Composer = s.Composer, 
                           Publisher = s.Publisher, 
                           Bpm = s.Bpm,

                           IsOwn = s.Uploader == user,
                           IsLiked = s.Uploader != user && s.UserLikes.Contains(user)
                       };
        }
    }
}