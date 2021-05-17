using Model;
using System.Collections.Generic;

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
        bool RemoveByUsuario(int id);
        bool Update(UsuarioOrganizacaoModel entity);
    }
}
