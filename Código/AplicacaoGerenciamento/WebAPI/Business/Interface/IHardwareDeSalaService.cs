using Model;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IHardwareDeSalaService
    {
        List<HardwareDeSalaModel> GetAll();
        List<HardwareDeSalaModel> GetAllHardwaresSalaByUsuarioOrganizacao(int idUsuario);
        HardwareDeSalaModel GetById(int id);
        List<HardwareDeSalaModel> GetByIdSala(int id);
        HardwareDeSalaModel GetByMAC(string mac, int idUsuario);
        bool Insert(HardwareDeSalaModel entity, int idUsuario);
        bool Remove(int id);
        bool Update(HardwareDeSalaModel entity, int idUsuario);
    }
}
