using Model;
using Model.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace Service.Interface
{
    public interface IUsuarioService
    {
        List<UsuarioModel> GetAll();
        UsuarioModel GetById(int id);
        UsuarioModel GetByCpf(string cpf);
        List<UsuarioModel> GetByIdOrganizacao(int id);
        UsuarioModel GetByLoginAndPass(string login, string senha);
        UsuarioViewModel Insert(UsuarioViewModel entity);
        bool Remove(int id);
        bool Update(UsuarioModel entity);
        UsuarioViewModel RetornLoggedUser(ClaimsIdentity claimsIdentity);
    }
}
