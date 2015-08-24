namespace BlastOFF.Data.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using Models.GalleryModels;

    public interface IBlastOFFContext : IDisposable
    {
        //// Gallery Db sets
        IDbSet<Album> Albums { get; set; }

        IDbSet<Image> Images { get; set; }

        //// Service methods
        void SaveChanges();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        new void Dispose();
    }
}