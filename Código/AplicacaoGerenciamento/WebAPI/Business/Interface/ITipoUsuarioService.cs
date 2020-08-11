using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ITipoUsuarioService
    {
        List<TipoUsuarioModel> GetAll();
        TipoUsuarioModel GetById(int id);
        bool Insert(TipoUsuarioModel entity);
        bool Remove(int id);
        bool Update(TipoUsuarioModel entity);
    }
}
