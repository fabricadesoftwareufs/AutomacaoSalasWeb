using Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Service.Interface
{
    public interface IUsuarioService
    {
        List<UsuarioModel> GetAll();
        UsuarioModel GetById(int id);
        List<UsuarioModel> GetByIdOrganizacao(int id);
        UsuarioModel GetByLoginAndPass(string login, string senha);
        bool Insert(UsuarioModel entity);
        bool Remove(int id);
        bool Update(UsuarioModel entity);
        UsuarioModel RetornLoggedUser(ClaimsIdentity claimsIdentity);
    }
}
