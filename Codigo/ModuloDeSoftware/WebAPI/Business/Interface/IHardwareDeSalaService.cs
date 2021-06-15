using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IHardwareDeSalaService
    {
        List<HardwareDeSalaModel> GetAll();
        List<HardwareDeSalaModel> GetAllHardwaresSalaByUsuarioOrganizacao(int idUsuario);
        HardwareDeSalaModel GetById(int id);
        List<HardwareDeSalaModel> GetByIdSala(int id);
        List<HardwareDeSalaModel> GetAtuadorByIdSala(int id);
        List<HardwareDeSalaModel> GetAllAtuador();
        List<HardwareDeSalaModel> GetAtuadorNotUsed();
        List<HardwareDeSalaModel> GetByIdSalaAndTipoHardware(int id, int tipo);
        HardwareDeSalaModel GetByMAC(string mac, int idUsuario);
        HardwareDeSalaModel GetByMAC(string mac);

        HardwareDeSalaModel GetByIp(string ip, int idUsuario);
        HardwareDeSalaModel GetByIp(string ip);

        bool Insert(HardwareDeSalaModel entity, int idUsuario);
        bool Remove(int id);
        bool Update(HardwareDeSalaModel entity, int idUsuario);
        bool Update(HardwareDeSalaModel entity);

    }
}
