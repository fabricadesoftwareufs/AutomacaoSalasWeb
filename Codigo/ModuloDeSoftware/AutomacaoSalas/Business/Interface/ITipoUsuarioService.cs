﻿using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ITipoUsuarioService
    {
        List<TipoUsuarioModel> GetAll();
        TipoUsuarioModel GetTipoUsuarioByUsuarioId(uint idUsuario); 
        bool Insert(TipoUsuarioModel entity);
        bool Remove(int id);
        bool Update(TipoUsuarioModel entity);
    }
}