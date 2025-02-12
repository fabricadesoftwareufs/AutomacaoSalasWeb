using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IConexaoInternetSalaService
    {
        List<ConexaoInternetSalaModel> GetAll();
        ConexaoInternetSalaModel GetById(uint idConexaoInternet, uint idSala);
        List<ConexaoInternetSalaModel> GetByIdConexaoInternet(uint id);
        List<ConexaoInternetSalaModel> GetByIdSala(uint id);
        bool Insert(ConexaoInternetSalaModel entity);
        bool Remove(uint idConexaoInternet, uint idSala);
        bool RemoveByConexaoInternet(uint idConexaoInternet);
        bool Update(ConexaoInternetSalaModel entity);
        List<ConexaoInternetSalaModel> GetBySalaOrdenadoPorPrioridade(uint idSala);
        bool MoverPrioridade(uint idSala, uint idConexaoInternet, uint novaPosicao);
        bool TrocarPrioridade(uint idSala, uint idConexaoInternet1, uint idConexaoInternet2);
    }
}
