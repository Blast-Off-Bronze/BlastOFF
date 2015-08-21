namespace BlastOFFApp.Data
{
    using System.Data.Entity;

    using BlastOFFApp.Data.Migrations;

    using BlastOFFApp.Models.GalleryModels;

    public class BlastOFFContext : DbContext
    {
        public BlastOFFContext()
            : base("name=BlastOFFContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlastOFFContext, Configuration>());
        }

        //Gallery Db sets
        public virtual DbSet<Album> Albums { get; set; }

        public virtual DbSet<Image> Images { get; set; }
    }
}