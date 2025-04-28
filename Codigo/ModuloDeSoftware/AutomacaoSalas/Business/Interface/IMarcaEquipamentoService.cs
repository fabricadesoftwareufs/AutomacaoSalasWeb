using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IMarcaEquipamentoService
    {
        bool Insert(MarcaEquipamentoModel marca);
        bool Update(MarcaEquipamentoModel marca);
        bool Remove(uint id);
        MarcaEquipamentoModel GetById(uint id);
        List<MarcaEquipamentoModel> GetAll();
        List<MarcaEquipamentoModel> GetByName(string name);
    }
}
