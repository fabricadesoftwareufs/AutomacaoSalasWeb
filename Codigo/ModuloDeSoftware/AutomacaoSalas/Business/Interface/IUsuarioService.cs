using Model;
using Model.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace Service.Interface
{
    public interface IUsuarioService
    {
        List<UsuarioModel> GetAll();
        List<UsuarioModel> GetAllExceptAuthenticatedUser(uint idUser);
        UsuarioModel GetById(uint id);
        UsuarioModel GetByCpf(string cpf);
        List<UsuarioModel> GetByIdOrganizacao(uint id);
        List<UsuarioModel> GetAllByIdsOrganizacao(List<uint> ids);
        //UsuarioModel GetByLoginAndPass(string login, string senha);
        UsuarioViewModel Insert(UsuarioViewModel entity);
        bool Remove(int id);
        bool Update(UsuarioModel entity);
        UsuarioViewModel GetAuthenticatedUser(ClaimsIdentity claimsIdentity);
    }
}
