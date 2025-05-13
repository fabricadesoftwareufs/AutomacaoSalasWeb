using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.ViewModel;

namespace Service.Interface
{
    public interface IModeloEquipamentoService
    {
        bool Insert(ModeloEquipamentoViewModel modelo);
        bool Update(ModeloEquipamentoViewModel modelo);
        bool Remove(uint id);
        ModeloEquipamentoModel GetById(uint id);
        List<ModeloEquipamentoModel> GetAll();
        List<ModeloEquipamentoModel> GetByName(string name);
        List<ModeloEquipamentoModel> GetByMarca(uint idMarca);
    }
}
