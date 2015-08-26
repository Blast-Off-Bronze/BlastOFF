namespace BlastOFF.Data.Interfaces
{
    using Models;
    using Models.BlastModels;
    using Models.GalleryModels;
    using Models.MusicModels;
    using Models.UserModel;

    public interface IBlastOFFData
    {
        //// START - Repositories

        //// ApplicationUser
        IRepository<ApplicationUser> Users { get; }

        //// Comments
        IRepository<Comment> Comments { get; }

        //// Blasts
        IRepository<Blast> Blasts { get; }

        //// Gallery
        IRepository<GalleryAlbum> GalleryAlbums { get; }

        IRepository<Image> Images { get; }

        //// Music
        IRepository<MusicAlbum> MusicAlbums { get; }

        IRepository<Song> Songs { get; }

        //// END - Repositories

        void SaveChanges();

        void Dispose();
    }
}