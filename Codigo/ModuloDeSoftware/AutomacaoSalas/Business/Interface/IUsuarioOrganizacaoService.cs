using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IUsuarioOrganizacaoService
    {
        List<UsuarioOrganizacaoModel> GetAll();
        UsuarioOrganizacaoModel GetById(uint idUsuario, uint idOrganicao);
        List<UsuarioOrganizacaoModel> GetByIdUsuario(uint id);
        List<UsuarioOrganizacaoModel> GetByIdOrganizacao(uint id);
        bool Insert(UsuarioOrganizacaoModel entity);

        bool Remove(uint idUsuario, uint idOrganizacao);
        bool RemoveByUsuario(uint idUsuario);
        bool Update(UsuarioOrganizacaoModel entity);
    }
}
