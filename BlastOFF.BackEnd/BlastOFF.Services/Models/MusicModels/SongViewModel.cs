namespace BlastOFF.Services.Models.MusicModels
{
    using System;
    using System.Linq.Expressions;

    using BlastOFF.Models.MusicModels;

    public class SongViewModel
    {
        public static Expression<Func<Song, SongViewModel>> Get
        {
            get
            {
                return s => new SongViewModel
                {
                    Id = s.Id,
                    Title = s.Title,
                    Artist = s.Artist,
                    FilePath = s.FilePath,
                    MusicAlbumId = s.MusicAlbumId,
                    MusicAlbum = s.MusicAlbum.Title,
                    DateAdded = s.DateAdded,
                    ViewsCount = s.ViewsCount,
                    LikesCount = s.UserLikes.Count,
                    CommentsCount = s.Comments.Count,
                    Details = new
                    {
                        s.TrackNumber,
                        s.OriginalAlbumTitle,
                        s.OriginalAlbumArtist,
                        s.OriginalDate,
                        s.Genre,
                        s.Composer,
                        s.Publisher,
                        s.Bpm
                    }
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string FilePath { get; set; }

        public int MusicAlbumId { get; set; }

        public string MusicAlbum { get; set; }

        public DateTime DateAdded { get; set; }

        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public object Details { get; set; }
    }
}