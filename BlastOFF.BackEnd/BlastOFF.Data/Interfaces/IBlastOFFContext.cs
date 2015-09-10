namespace BlastOFF.Data.Interfaces
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IBlastOFFContext : IDisposable
    {
        //// Service methods
        void SaveChanges();

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        new void Dispose();
    }
}