namespace BlastOFF.Data.Interfaces
{
    using System;
    using System.Data.Entity;
    using BlastOFF.Models;
    using BlastOFF.Models.BlastModels;
    using BlastOFF.Models.ChatModels;
    using BlastOFF.Models.GalleryModels;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Models.UserModels;

    public interface IBlastOFFContext : IDisposable
    {
        IDbSet<Blast> Blasts { get; }

        IDbSet<Comment> Comments { get; }

        IDbSet<Message> Messages { get; }

        IDbSet<ImageAlbum> ImageAlbums { get; }

        IDbSet<Image> Images { get; }

        IDbSet<MusicAlbum> MusicAlbums { get; }

        IDbSet<Song> Songs { get; }

        IDbSet<UserSession> UserSessions { get; }
        IDbSet<Notification> Notifications { get; }
    }
}