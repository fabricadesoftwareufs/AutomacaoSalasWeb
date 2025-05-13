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

            // Precisamos pegar o IdModeloEquipamento do equipamento
            return GetByIdOperacaoAndIdModeloEquipamento((int)equipamento.IdModeloEquipamento, operacao);
        }

        public CodigoInfravermelhoModel GetById(int id)
        => _context.Codigoinfravermelhos
                   .Where(ir => ir.Id == id)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdModeloEquipamento = ir.IdModeloEquipamento,
                       IdOperacao = ir.IdOperacao,
                   }).FirstOrDefault();

        public CodigoInfravermelhoModel GetByIdOperacaoAndIdModeloEquipamento(int idModeloEquipamento, int idOperacao)
            => _context.Codigoinfravermelhos
                   .Where(ir => ir.IdModeloEquipamento == idModeloEquipamento && ir.IdOperacao == idOperacao)
                   .Select(ir => new CodigoInfravermelhoModel
                   {
                       Id = ir.Id,
                       Codigo = ir.Codigo,
                       IdModeloEquipamento = ir.IdModeloEquipamento,
                       IdOperacao = ir.IdOperacao,
                   }).FirstOrDefault();

        public List<CodigoInfravermelhoModel> GetAllByEquipamento(int idEquipamento)
        {
            // Precisamos obter o modelo do equipamento primeiro
            var modeloEquipamento = _context.Modeloequipamentos
                .Where(e => e.Id == idEquipamento)
                .Select(e => e.Id)
                .FirstOrDefault();

            return _context.Codigoinfravermelhos
                .Where(cs => cs.IdModeloEquipamento == modeloEquipamento)
                .Select(cs => new CodigoInfravermelhoModel
                {
                    Id = cs.Id,
                    IdModeloEquipamento = cs.IdModeloEquipamento,
                    Codigo = cs.Codigo,
                    IdOperacao = cs.IdOperacao
                }).ToList();
        }
        //possivelmente tá errado 
        public List<CodigoInfravermelhoModel> GetAllByUuidHardware(string uuid)
         => _context.Codigoinfravermelhos
          .Join(_context.Modeloequipamentos,
             codigo => codigo.IdModeloEquipamento,
             modelo => modelo.Id,
             (codigo, modelo) => new { Codigo = codigo, Modelo = modelo })
          .Join(_context.Equipamentos,
             cm => cm.Modelo.Id,
             equip => equip.IdModeloEquipamento,
             (cm, equip) => new { CodigoModelo = cm, Equipamento = equip })
          .Join(_context.Hardwaredesalas,
             eq => eq.Equipamento.IdHardwareDeSala,
             hd => hd.Id,
             (eq, hd) => new { EquipamentoCodigo = eq, Hardware = hd })
          .Where(cs => !string.IsNullOrWhiteSpace(cs.Hardware.Uuid)
                    && cs.Hardware.Uuid.Trim().Equals(uuid.Trim()))
          .Select(cs => new CodigoInfravermelhoModel
          {
              Id = cs.EquipamentoCodigo.CodigoModelo.Codigo.Id,
              IdModeloEquipamento = cs.EquipamentoCodigo.CodigoModelo.Codigo.IdModeloEquipamento,
              Codigo = cs.EquipamentoCodigo.CodigoModelo.Codigo.Codigo,
              IdOperacao = cs.EquipamentoCodigo.CodigoModelo.Codigo.IdOperacao
          }).ToList();

        public bool AddAll(List<CodigoInfravermelhoModel> codigoInfravermelhoModels)
        {
            if (codigoInfravermelhoModels == null || !codigoInfravermelhoModels.Any())
                return true; // Nada para adicionar, consideramos sucesso

            List<Codigoinfravermelho> codigos = new List<Codigoinfravermelho>();
            codigoInfravermelhoModels.ForEach(c => codigos.Add(SetEntity(c)));

            _context.AddRange(codigos);
            return _context.SaveChanges() > 0; // Se salvou pelo menos um, é sucesso
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
            IdModeloEquipamento = codigo.IdModeloEquipamento,
            IdOperacao = codigo.IdOperacao
        };

        public CodigoInfravermelhoModel Insert(CodigoInfravermelhoModel entity)
        {
            var codigo = SetEntity(entity);
            _context.Add(codigo);
            _context.SaveChanges();
            return entity;
        }
    }
}