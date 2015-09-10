namespace BlastOFF.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Interfaces;
    using Migrations;
    using Models.GalleryModels;
    using Models.MusicModels;
    using Models.UserModel;
    using Models.BlastModels;

    using System.Data.Entity.ModelConfiguration.Conventions;
    using BlastOFF.Models;
    using BlastOFF.Models.ChatModels;
    using BlastOFF.Models.UserModels;

    public class BlastOFFContext : IdentityDbContext<ApplicationUser>, IBlastOFFContext
    {
        public BlastOFFContext()
            : base("BlastOFFContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlastOFFContext, Configuration>());
        }

        // START - DB SETS

        //// Blasts Db sets 
        public virtual IDbSet<Blast> Blasts { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        ////Chat Db set
        public virtual IDbSet<Message> Messages { get; set; }
        
        //// Gallery Db sets
        public virtual IDbSet<ImageAlbum> ImageAlbums { get; set; }

        public virtual IDbSet<Image> Images { get; set; }

        //// Music Db sets
        public virtual IDbSet<MusicAlbum> MusicAlbums { get; set; }

        public virtual IDbSet<Song> Songs { get; set; }

        public virtual IDbSet<UserSession> UserSessions { get; set; }
        public virtual IDbSet<Notification> Notifications { get; set; }

        // END - DB SETS

        //// Service methods
        public static BlastOFFContext Create()
        {
            return new BlastOFFContext();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Followers mapping

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedBy)
                .WithMany(u => u.FollowedUsers)
                .Map(m =>
                {
                    m.MapLeftKey("Follower_Id");
                    m.MapRightKey("FollowedBy_Id");
                    m.ToTable("UsersFollowers");
                });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedMusicAlbums)
                .WithMany(ma => ma.Followers)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("MusicAlbum_Id");
                    m.ToTable("MusicAbumsUserFollowers");
                });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedImageAlbums)
                .WithMany(ga => ga.Followers)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("ImageAlbum_Id");
                    m.ToTable("ImageAlbumsUserFollowers");
                });

            //Message mapping

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.SentMessages)
                .WithRequired(m => m.Sender);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ReceivedMessages)
                .WithRequired(m => m.Receiver);

            // Likes Mapping

            modelBuilder.Entity<Blast>()
                .HasMany(b => b.UserLikes)
                .WithMany(u => u.LikedBlasts)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Blast_Id");
                    m.ToTable("BlastsUserLikes");
                });

            modelBuilder.Entity<Comment>()
                .HasMany(c => c.LikedBy)
                .WithMany(u => u.LikedComments)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Comment_Id");
                    m.ToTable("CommentsUserLikes");
                });

            //Music mapping

            modelBuilder.Entity<MusicAlbum>()
                .HasMany(m => m.UserLikes)
                .WithMany(u => u.LikedMusicAlbums)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("MusicAlbum_Id");
                    m.ToTable("MusicAlbumsUserLikes");
                });

            modelBuilder.Entity<MusicAlbum>()
                .HasMany(a => a.Songs)
                .WithOptional(s => s.MusicAlbum)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Song>()
                .HasMany(s => s.UserLikes)
                .WithMany(u => u.LikedSongs)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Song_Id");
                    m.ToTable("SongsUserLikes");
                });

            modelBuilder.Entity<Song>()
                .HasMany(a => a.Comments)
                .WithOptional(c => c.Song)
                .WillCascadeOnDelete(true);

            //Gallery mapping

            modelBuilder.Entity<ImageAlbum>()
                .HasMany(ga => ga.UserLikes)
                .WithMany(u => u.LikedImageAlbums)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("ImageAlbum_Id");
                    m.ToTable("ImageAlbumsUserLikes");
                });


            modelBuilder.Entity<Image>()
                .HasMany(i => i.UserLikes)
                .WithMany(u => u.LikedImages)
                .Map(m =>
                {
                    m.MapLeftKey("User_Id");
                    m.MapRightKey("Image_Id");
                    m.ToTable("ImagesUserLikes");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}