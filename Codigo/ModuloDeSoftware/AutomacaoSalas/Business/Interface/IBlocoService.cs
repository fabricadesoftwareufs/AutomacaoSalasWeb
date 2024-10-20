using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IBlocoService
    {
        List<BlocoModel> GetAll();
        BlocoModel GetById(uint id);
        List<BlocoModel> GetByIdOrganizacao(uint id);
        List<BlocoModel> GetAllByIdUsuarioOrganizacao(uint idUsuario);
        BlocoModel GetByTitulo(string titulo, uint idOrganizacao);
        bool InsertBlocoWithHardware(BlocoModel entity, uint idUsuario);
        BlocoModel Insert(BlocoModel entity);
        bool Remove(uint id);
        bool Update(BlocoModel entity);
    }
}
