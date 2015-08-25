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

        public virtual IDbSet<BlastComment> BlastComments { get; set; }
        
        //// Gallery Db sets
        public virtual IDbSet<Album> Albums { get; set; }

        public virtual IDbSet<Image> Images { get; set; }

        //// Music Db sets
        public virtual IDbSet<MusicAlbum> MusicAlbums { get; set; }

        public virtual IDbSet<Song> Songs { get; set; }

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
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.FollowedBy)
                .WithMany(u => u.FollowedUsers)
                .Map(m =>
                {
                    m.MapLeftKey("Follower_Id");
                    m.MapRightKey("FollowedBy_Id");
                    m.ToTable("UsersFollowers");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}