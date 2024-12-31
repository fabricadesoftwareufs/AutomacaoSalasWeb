using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CodigoInfravermelhoService : ICodigoInfravermelhoService
    {
        private readonly SalasDBContext _context;
        public CodigoInfravermelhoService(SalasDBContext context)
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
       => _context.Codigoinfravermelhos
                   .Where(ir => ir.Id == id)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdEquipamento = ir.IdEquipamento,
                       IdOperacao = ir.IdOperacao,
                   }).FirstOrDefault();

        public CodigoInfravermelhoModel GetByIdOperacaoAndIdEquipamento(int idEquipamento, int idOperacao)
            => _context.Codigoinfravermelhos
                   .Where(ir => ir.IdEquipamento == idEquipamento && ir.IdOperacao == idOperacao)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdEquipamento = ir.IdEquipamento,
                       IdOperacao = ir.IdOperacao,
                   }).FirstOrDefault();

        public List<CodigoInfravermelhoModel> GetAllByEquipamento(int idEquipamento)
        => _context.Codigoinfravermelhos
        .Where(cs => cs.IdEquipamento == idEquipamento)
        .Select(cs => new CodigoInfravermelhoModel
        {
            Id = cs.Id,
            IdEquipamento = cs.IdEquipamento,
            Codigo = cs.Codigo,
            IdOperacao = cs.IdOperacao
        }).ToList();

        public List<CodigoInfravermelhoModel> GetAllByUuidHardware(string uuid)
         => _context.Codigoinfravermelhos
          .Join(_context.Equipamentos,
             codigo => codigo.IdEquipamento,
             equip => equip.Id,
             (codigo, equip) => new { Codigo = codigo, Equipamento = equip })
          .Join(_context.Hardwaredesalas,
             eq => eq.Equipamento.IdHardwareDeSala,
             hd => hd.Id,
             (eq, hd) => new { Equipamento = eq, Hardware = hd })
          .Where(cs => !string.IsNullOrWhiteSpace(cs.Hardware.Uuid)
                    && cs.Hardware.Uuid.Trim().Equals(uuid.Trim()))
          .Select(cs => new CodigoInfravermelhoModel
          {
             Id = cs.Equipamento.Codigo.Id,
             IdEquipamento = cs.Equipamento.Codigo.IdEquipamento,
             Codigo = cs.Equipamento.Codigo.Codigo,
             IdOperacao = cs.Equipamento.Codigo.IdOperacao
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
            IdEquipamento = codigo.IdEquipamento,
            IdOperacao = codigo.IdOperacao
        };
    }
}
