namespace BlastOFF.Data.Interfaces
{
    using System.Linq;

    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Find(object id);

        void Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        T Delete(object id);
    }
}