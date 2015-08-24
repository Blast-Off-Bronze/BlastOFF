namespace BlastOFF.Data
{
    using System.Data.Entity;

    using Interfaces;
    using Migrations;
    using Models.GalleryModels;
    using Models.MusicModels;

    public class BlastOFFContext : DbContext, IBlastOFFContext
    {
        public BlastOFFContext()
            : base("BlastOFFContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlastOFFContext, Configuration>());
        }

        // START - DB SETS

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
    }
}