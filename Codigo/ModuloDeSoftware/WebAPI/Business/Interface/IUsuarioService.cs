using Model;
using Model.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace Service.Interface
{
    public interface IUsuarioService
    {
        List<UsuarioModel> GetAll();
        List<UsuarioModel> GetUserNotLog(int idLog);
        UsuarioModel GetById(int id);
        UsuarioModel GetByCpf(string cpf);
        List<UsuarioModel> GetByIdOrganizacao(int id);
        List<UsuarioModel> GetAllByIdsOrganizacao(List<int> ids);
        UsuarioModel GetByLoginAndPass(string login, string senha);
        UsuarioViewModel Insert(UsuarioViewModel entity);
        bool Remove(int id);
        bool Update(UsuarioModel entity);
        UsuarioViewModel RetornLoggedUser(ClaimsIdentity claimsIdentity);
    }
}
