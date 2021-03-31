using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CodigoInfravermelhoService : ICodigoInfravermelhoService
    {
        private readonly str_dbContext _context;
        public CodigoInfravermelhoService(str_dbContext context)
        {
            _context = context;
        }

        public CodigoInfravermelhoModel GetByIdSalaAndIdOperacao(int idSala, int operacao)
        {
            var _equipamentoService = new EquipamentoService(_context);
            var equipamento = _equipamentoService.GetByIdSalaAndTipoEquipamento(idSala, EquipamentoModel.TIPO_CONDICIONADOR);

            return GetByIdOperacaoAndIdEquipamento(equipamento.Id, operacao);
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

        public CodigoInfravermelhoModel GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao)
            => _context.Codigoinfravermelho
                   .Where(ir => ir.Equipamento == idEquipamento && ir.Operacao == idOperacao)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdEquipamento = ir.Equipamento,
                       IdOperacao = ir.Operacao,
                   }).FirstOrDefault();

        public List<CodigoInfravermelhoModel> GetAllByEquipamento(int idEquipamento)
        => _context.Codigoinfravermelho
        .Where(cs => cs.Equipamento == idEquipamento)
        .Select(cs => new CodigoInfravermelhoModel
        {
            Id = cs.Id,
            IdEquipamento = cs.Equipamento,
            Codigo = cs.Codigo,
            IdOperacao = cs.Operacao
        }).ToList();

        public bool AddAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels)
        {
            List<Codigoinfravermelho> codigos = new List<Codigoinfravermelho>();
            codigoInfravermelhoModels.ForEach(c => codigos.Add(SetEntity(c)));

            _context.AddRange(codigos);
            return _context.SaveChanges() == 1;
        }

        public bool UpdateAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels)
        {
            List<Codigoinfravermelho> codigos = new List<Codigoinfravermelho>();
            codigoInfravermelhoModels.ForEach(c => codigos.Add(SetEntity(c)));

            _context.UpdateRange(codigos);
            return _context.SaveChanges() == 1;
        }

        public bool RemoveAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels)
        {
            var codigos = new List<Codigoinfravermelho>();
            codigoInfravermelhoModels.ForEach(c => codigos.Add(SetEntity(c)));
            _context.RemoveRange(codigos);
            return _context.SaveChanges() == 1;
        }
        private static Codigoinfravermelho SetEntity(CodigoInfravermelhoModel codigo)
        => new Codigoinfravermelho
        {
            Id = codigo.Id,
            Codigo = codigo.Codigo,
            Equipamento = codigo.IdEquipamento,
            Operacao = codigo.IdOperacao
        };
    }
}
