using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IEquipamentoService
    {
        EquipamentoModel GetByIdSalaAndTipoEquipamento(int id,string tipo);  
    }
}
