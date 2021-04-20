using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class OperacaoCodigoService : IOperacaoCodigoService
    {
        private readonly str_dbContext _context;
        public OperacaoCodigoService(str_dbContext context)
        {
            _context = context;
        }
        public List<OperacaoModel> GetAll()
            => _context.Operacao.Select(o => new OperacaoModel { Id = o.Id, Descricao = o.Descricao, Titulo = o.Titulo }).ToList();
        public OperacaoModel GetById(int id)
            => _context.Operacao.Where(s => s.Id == id).Select(o => new OperacaoModel { Id = o.Id, Descricao = o.Descricao, Titulo = o.Titulo }).FirstOrDefault();
    }
}
