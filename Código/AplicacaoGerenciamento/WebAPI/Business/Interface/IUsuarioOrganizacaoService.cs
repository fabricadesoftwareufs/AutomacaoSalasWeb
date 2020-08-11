using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IUsuarioOrganizacaoService
    {
        List<UsuarioOrganizacaoModel> GetAll();
        UsuarioOrganizacaoModel GetById(int id);
        List<UsuarioOrganizacaoModel> GetByIdUsuario(int id);
        List<UsuarioOrganizacaoModel> GetByIdOrganizacao(int id);
        bool Insert(UsuarioOrganizacaoModel entity);

        bool Remove(int id);
        bool Update(UsuarioOrganizacaoModel entity);
    }
}
