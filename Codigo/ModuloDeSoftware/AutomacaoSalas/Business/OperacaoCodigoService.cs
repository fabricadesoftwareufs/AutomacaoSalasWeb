using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OperacaoCodigoService : IOperacaoCodigoService
    {
        private readonly SalasDBContext _context;
        public OperacaoCodigoService(SalasDBContext context)
        {
            _context = context;
        }
        public List<OperacaoModel> GetAll()
            => _context.Operacaos.Select(o => new OperacaoModel { Id = o.Id, Descricao = o.Descricao, Titulo = o.Titulo }).ToList();
        public OperacaoModel GetById(int id)
            => _context.Operacaos.Where(s => s.Id == id).Select(o => new OperacaoModel { Id = o.Id, Descricao = o.Descricao, Titulo = o.Titulo }).FirstOrDefault();
    }
}
