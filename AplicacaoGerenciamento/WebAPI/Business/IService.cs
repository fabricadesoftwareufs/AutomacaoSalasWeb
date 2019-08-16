using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
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
