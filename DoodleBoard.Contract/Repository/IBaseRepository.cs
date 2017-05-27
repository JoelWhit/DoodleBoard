using System.Collections.Generic;

namespace DoodleBoard.Contract.Repository
{
    public interface IBaseRepository<T, TKey>
    {
        T Save(T entity);
        void Delete(T entity);
        List<T> GetAll();
        T GetById(TKey id);

        void SaveChanges();
    }
}
