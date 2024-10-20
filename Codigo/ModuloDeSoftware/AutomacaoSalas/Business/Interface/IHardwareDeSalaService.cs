using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IHardwareDeSalaService
    {
        List<HardwareDeSalaModel> GetAll();
        List<HardwareDeSalaModel> GetAllHardwaresSalaByUsuarioOrganizacao(uint idUsuario);
        HardwareDeSalaModel GetById(uint id);
        HardwareDeSalaModel GetByIdAndType(int id, int tipo);
        List<HardwareDeSalaModel> GetByIdSala(uint id);
        List<HardwareDeSalaModel> GetAtuadorByIdSala(uint id);
        List<HardwareDeSalaModel> GetAllAtuador();
        List<HardwareDeSalaModel> GetAtuadorNotUsed();
        List<HardwareDeSalaModel> GetByIdSalaAndTipoHardware(uint id, int tipo);
        List<HardwareDeSalaModel> GetSensorsAndActuactorsByIdSala(uint id);
        HardwareDeSalaModel GetByMAC(string mac, uint idUsuario);
        HardwareDeSalaModel GetByMAC(string mac);

        HardwareDeSalaModel GetByIp(string ip, uint idUsuario);
        HardwareDeSalaModel GetByIp(string ip);

        HardwareDeSalaModel GetByUuid(string uuid);

        HardwareDeSalaModel GetControladorByIdSala(uint idSala);

        bool Insert(HardwareDeSalaModel entity, uint idUsuario);
        bool Remove(int id);
        bool Update(HardwareDeSalaModel entity, uint idUsuario);
        bool Update(HardwareDeSalaModel entity);

    }
}
