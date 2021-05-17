using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IHardwareDeBlocoService
    {
        List<HardwareDeBlocoModel> GetAll();
        HardwareDeBlocoModel GetById(int id);
        List<HardwareDeBlocoModel> GetByIdBloco(int id);

        HardwareDeBlocoModel GetByMAC(string mac, int idUsuario);
        bool Insert(HardwareDeBlocoModel entity, int idUsuario);
        List<HardwareDeBlocoModel> GetAllHardwaresBlacoByUsuarioOrganizacao(int idUsuario);
        bool Remove(int id);
        bool Update(HardwareDeBlocoModel entity, int idUsuario);
    }
}
