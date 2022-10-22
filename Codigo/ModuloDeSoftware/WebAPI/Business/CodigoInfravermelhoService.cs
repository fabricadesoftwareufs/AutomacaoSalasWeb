﻿using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CodigoInfravermelhoService : ICodigoInfravermelhoService
    {
        private readonly SalasUfsDbContext _context;
        public CodigoInfravermelhoService(SalasUfsDbContext context)
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

        public CodigoInfravermelhoModel GetAllByUuidHardware(string uuid, int operacao)
         => _context.Codigoinfravermelho
          .Join(_context.Equipamento,
             codigo => codigo.Equipamento,
             equip => equip.Id,
             (codigo, equip) => new { Codigo = codigo, Equipamento = equip })
          .Join(_context.Hardwaredesala,
             eq => eq.Equipamento.HardwareDeSala,
             hd => hd.Id,
             (eq, hd) => new { Equipamento = eq, Hardware = hd })
          .Where(cs => !string.IsNullOrWhiteSpace(cs.Hardware.Uuid)
                    && cs.Hardware.Uuid.Trim().Equals(uuid.Trim()) && operacao == cs.Equipamento.Codigo.Operacao)
          .Select(cs => new CodigoInfravermelhoModel
          {
             Id = cs.Equipamento.Codigo.Id,
             IdEquipamento = cs.Equipamento.Codigo.Equipamento,
             Codigo = cs.Equipamento.Codigo.Codigo,
             IdOperacao = cs.Equipamento.Codigo.Operacao
          }).FirstOrDefault();

       
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
