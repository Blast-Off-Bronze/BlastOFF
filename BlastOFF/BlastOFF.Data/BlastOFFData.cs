namespace BlastOFF.Data
{
    using System;

    using Interfaces;

    public class BlastOFFData : IBlastOFFData, IDisposable
    {
        //// Repositories
        //// Place repositories here
        //// e.g. private IRepository<T> entities;

        private IBlastOFFContext context;
        private bool disposed = false;

        public BlastOFFData()
            : this(new BlastOFFContext())
        {
        }

        public BlastOFFData(IBlastOFFContext context)
        {
            this.context = context;
        }

        // START - Repsotiories

        ////public IRepository<T> entities
        ////{
        ////    get
        ////    {
        ////        if (this.entities == null)
        ////        {
        ////            this.entities = new Repository<T>(this.context);
        ////        }
        ////
        ////        return this.entities;
        ////    }
        ////}

        // END - Repsotiories

        //// Service methods
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}