using System.Collections.Generic;

namespace Service
{
    public interface IService<T>
    {
        bool Insert(T entity);
        bool Update(T entity);
        bool Remove(int id);
        T GetById(int id);
        List<T> GetAll();
    }
}
