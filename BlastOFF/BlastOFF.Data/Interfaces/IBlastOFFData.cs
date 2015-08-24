namespace BlastOFF.Data.Interfaces
{
    public interface IBlastOFFData
    {
        //// START - Repositories

        //// IRepository<T> Entities { get; }

        //// END - Repositories

        void SaveChanges();
    }
}