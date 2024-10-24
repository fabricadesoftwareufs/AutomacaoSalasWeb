﻿using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ITipoUsuarioService
    {
        List<TipoUsuarioModel> GetAll();
        TipoUsuarioModel GetById(uint id);
        bool Insert(TipoUsuarioModel entity);
        bool Remove(int id);
        bool Update(TipoUsuarioModel entity);
    }
}
