using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CodigoInfravermelhoService : ICodigoInfravermelhoService
    {
        private readonly STR_DBContext _context;
        public CodigoInfravermelhoService(STR_DBContext context)
        {
            _context = context;
        }

        public CodigoInfravermelhoModel GetByIdSala(int idSala)
        {
            var _equipamentoService = new EquipamentoService(_context);
            var equipamentos = _equipamentoService.GetByIdSalaAndTipoEquipamento(idSala, EquipamentoModel.TIPO_CONDICIONADOR);

            return _context.Codigoinfravermelho
                 .Where(ir => ir.Equipamento == equipamentos.Id)
                 .Select(ir => new CodigoInfravermelhoModel
                 {
                     Id = ir.Id,
                     Codigo = ir.Codigo,
                     IdEquipamento = ir.Equipamento,
                     IdOperacao = ir.Operacao,
                 }).FirstOrDefault();
        }

        public CodigoInfravermelhoModel GetById(int id)
       => _context.Codigoinfravermelho
                   .Where(ir => ir.Id == id)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdEquipamento = ir.Equipamento,
                       IdOperacao = ir.Operacao,
                   }).FirstOrDefault();

        public List<CodigoInfravermelhoModel> GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao)
            => _context.Codigoinfravermelho
                   .Where(ir => ir.Equipamento == idEquipamento && ir.Operacao == idOperacao)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdEquipamento = ir.Equipamento,
                       IdOperacao = ir.Operacao,
                   }).ToList();
    }
}
