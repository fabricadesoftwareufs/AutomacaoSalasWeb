using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class OperacaoCodigoService : IOperacaoCodigoService
    {
        private readonly STR_DBContext _context;
        public OperacaoCodigoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<OperacaoModel> GetAll()
        => _context.Operacao.Select(o => new OperacaoModel { Id = o.Id, Descricao = o.Descricao, Titulo = o.Titulo }).ToList();
    }
}
