using BlastOFF.Models.ChatModels;

namespace BlastOFF.Data
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
        private IRepository<Chat> chats;
        private IRepository<Comment> comments;
        private IRepository<Blast> blasts;
        private IRepository<ImageAlbum> imageAlbums;
        private IRepository<Image> images;
        private IRepository<MusicAlbum> musicAlbums;
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

        //// Chat
        public IRepository<Chat> Chats
        {
            get
            {
                if (this.chats == null)
                {
                    this.chats = new Repository<Chat>(this.context);
                }
                return this.chats;
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
        public IRepository<ImageAlbum> ImageAlbums
        {
            get
            {
                if (this.imageAlbums == null)
                {
                    this.imageAlbums = new Repository<ImageAlbum>(this.context);
                }

                return this.imageAlbums;
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
                if (this.musicAlbums == null)
                {
                    this.musicAlbums = new Repository<MusicAlbum>(this.context);
                }

                return this.musicAlbums;
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