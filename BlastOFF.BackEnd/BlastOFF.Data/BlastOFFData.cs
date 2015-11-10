namespace BlastOFF.Data
{
    using System;

    using Models;
    using Models.BlastModels;
    using Models.GalleryModels;
    using Models.MusicModels;
    using Models.UserModel;

    using Interfaces;

    using BlastOFF.Models.ChatModels;
    using BlastOFF.Models.UserModels;
    using System.Collections.Generic;
    using System.Data.Entity;

    public class BlastOFFData : IBlastOFFData//, IDisposable
    {
        //// Repositories

        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public BlastOFFData()
            : this(new BlastOFFContext())
        {
        }

        public BlastOFFData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        //// START - Repsotiories

        //// ApplicationUser
        public IRepository<ApplicationUser> Users
        {
            get
            {
                return this.GetRepository<ApplicationUser>();

            }
        }

        public IRepository<Notification> Notifications
        {
            get
            {
                return this.GetRepository<Notification>();
            }
        }

        //// Chat
        public IRepository<Message> Messages
        {
            get
            {
                return this.GetRepository<Message>();
            }
        }

        public IRepository<UserSession> UserSessions
        {
            get
            {
                return this.GetRepository<UserSession>();
            }
        }

        //// Comment
        public IRepository<Comment> Comments
        {
            get
            {
                return this.GetRepository<Comment>();
            }
        }

        //// Blasts
        public IRepository<Blast> Blasts
        {
            get
            {
                return this.GetRepository<Blast>();
            }
        }

        //// Gallery
        public IRepository<ImageAlbum> ImageAlbums
        {
            get
            {
                return this.GetRepository<ImageAlbum>();
            }
        }

        public IRepository<Image> Images
        {
            get
            {
                return this.GetRepository<Image>();
            }
        }

        //// Music
        public IRepository<MusicAlbum> MusicAlbums
        {
            get
            {
                return this.GetRepository<MusicAlbum>();
            }
        }

        public IRepository<Song> Songs
        {
            get
            {
                return this.GetRepository<Song>();
            }
        }

        //// END - Repsotiories

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(GenericRepository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }

        //// Service methods
        //public void Dispose()
        //{
        //    this.Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
    }
}