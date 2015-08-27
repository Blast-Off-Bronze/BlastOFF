﻿namespace BlastOFF.Services.Models
{
    using System;
    using System.Linq.Expressions;

    using BlastOFF.Models;

    public class CommentViewModel
    {
        public static Expression<Func<Comment, CommentViewModel>> Get
        {
            get
            {
                return c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    PostedOn = c.PostedOn,
                    AuthorId = c.AuthorId,
                    Author = c.Author.UserName,
                    BlastId = c.BlastId,
                    ImageAlbumId = c.ImageAlbumId,
                    ImageId = c.ImageId,
                    MusicAlbumId = c.MusicAlbumId,
                    SongId = c.SongId,
                    LikesCount = c.LikedBy.Count
                };
            }
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedOn { get; set; }

        public string AuthorId { get; set; }

        public string Author { get; set; }

        public int? SongId { get; set; }

        public int? ImageId { get; set; }

        public int? BlastId { get; set; }

        public int? ImageAlbumId { get; set; }

        public int? MusicAlbumId { get; set; }

        public int LikesCount { get; set; }
    }
}