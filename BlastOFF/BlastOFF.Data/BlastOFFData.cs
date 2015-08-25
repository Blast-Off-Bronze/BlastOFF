﻿namespace BlastOFF.Data
{
    using System;

    using Models;
    using Models.BlastModels;
    using Models.GalleryModels;
    using Models.MusicModels;
    using Models.UserModel;

    using Interfaces;

    public class BlastOFFData : IBlastOFFData, IDisposable
    {
        //// Repositories
        private IRepository<ApplicationUser> users;
        private IRepository<Comment> comments;
        private IRepository<Blast> blasts;
        private IRepository<GalleryAlbum> galleryAlbums;
        private IRepository<Image> images;
        private IRepository<MusicAlbum> musicyAlbums;
        private IRepository<Song> songs;

        private IBlastOFFContext context;
        private bool disposed = false;

        public BlastOFFData()
            : this(new BlastOFFContext())
        {
        }

        public BlastOFFData(IBlastOFFContext context)
        {
            this.context = context;
        }

        //// START - Repsotiories

        //// ApplicationUser
        public IRepository<ApplicationUser> Users
        {
            get
            {
                if (this.users == null)
                {
                    this.users = new Repository<ApplicationUser>(this.context);
                }

                return this.users;
            }
        }

        //// Comment
        public IRepository<Comment> Comments
        {
            get
            {
                if (this.comments == null)
                {
                    this.comments = new Repository<Comment>(this.context);
                }

                return this.comments;
            }
        }

        //// Blasts
        public IRepository<Blast> Blasts
        {
            get
            {
                if (this.blasts == null)
                {
                    this.blasts = new Repository<Blast>(this.context);
                }

                return this.blasts;
            }
        }

        //// Gallery
        public IRepository<GalleryAlbum> GalleryAlbums
        {
            get
            {
                if (this.galleryAlbums == null)
                {
                    this.galleryAlbums = new Repository<GalleryAlbum>(this.context);
                }

                return this.galleryAlbums;
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                if (this.images == null)
                {
                    this.images = new Repository<Image>(this.context);
                }

                return this.images;
            }
        }

        //// Music
        public IRepository<MusicAlbum> MusicAlbums
        {
            get
            {
                if (this.musicyAlbums == null)
                {
                    this.musicyAlbums = new Repository<MusicAlbum>(this.context);
                }

                return this.musicyAlbums;
            }
        }

        public IRepository<Song> Songs
        {
            get
            {
                if (this.songs == null)
                {
                    this.songs = new Repository<Song>(this.context);
                }

                return this.songs;
            }
        }

        //// END - Repsotiories

        //// Service methods
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}