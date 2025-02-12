using System.Collections.Generic;
using Model;

namespace Service.Interface
{
    public interface IConexaoInternetService
    {
        bool Insert(ConexaointernetModel conexao);
        bool Update(ConexaointernetModel conexao);
        bool Remove(uint id);
        ConexaointernetModel GetById(uint id);
        List<ConexaointernetModel> GetAll();
        List<ConexaointernetModel> GetByName(string name);
        List<ConexaointernetModel> GetByIdBloco(uint idBloco);
    }
}