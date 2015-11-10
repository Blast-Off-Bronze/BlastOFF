namespace BlastOFF.Data.Interfaces
{
    using Models.ChatModels;

    using Models;
    using Models.BlastModels;
    using Models.GalleryModels;
    using Models.MusicModels;
    using Models.UserModel;

    using BlastOFF.Models.UserModels;

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
        IRepository<ImageAlbum> ImageAlbums { get; }

        IRepository<Image> Images { get; }

        //// Music
        IRepository<MusicAlbum> MusicAlbums { get; }

        IRepository<Song> Songs { get; }

        //// Chat
        IRepository<Message> Messages { get; }

        IRepository<UserSession> UserSessions { get; }

        IRepository<Notification> Notifications { get; }

        //// END - Repositories

        int SaveChanges();
    }
}