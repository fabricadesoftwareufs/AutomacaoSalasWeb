using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IUsuarioOrganizacaoService
    {
        List<UsuarioOrganizacaoModel> GetAll();
        UsuarioOrganizacaoModel GetById(uint id);
        List<UsuarioOrganizacaoModel> GetByIdUsuario(uint id);
        List<UsuarioOrganizacaoModel> GetByIdOrganizacao(uint id);
        bool Insert(UsuarioOrganizacaoModel entity);

        bool Remove(uint id);
        bool RemoveByUsuario(uint id);
        bool Update(UsuarioOrganizacaoModel entity);
    }
}
