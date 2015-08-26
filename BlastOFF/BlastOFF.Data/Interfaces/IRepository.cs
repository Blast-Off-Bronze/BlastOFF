namespace BlastOFF.Data.Interfaces
{
    using System.Linq;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        T Delete(int id);

        void Detach(T entity);

        void SaveChanges();
    }
}