using Model;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IOperacaoCodigoService
    {
        List<OperacaoModel> GetAll();
        OperacaoModel GetById(int id);
    }
}
