using Model;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IHardwareDeBlocoService
    {
        List<HardwareDeBlocoModel> GetAll();
        HardwareDeBlocoModel GetById(int id);
        List<HardwareDeBlocoModel> GetByIdBloco(int id);

        HardwareDeBlocoModel GetByMAC(string mac, int idUsuario);
        bool Insert(HardwareDeBlocoModel entity, int idUsuario);
        List<HardwareDeBlocoModel> GetAllHardwaresSalaByUsuarioOrganizacao(int idUsuario);
        bool Remove(int id);
        bool Update(HardwareDeBlocoModel entity, int idUsuario);
    }
}
