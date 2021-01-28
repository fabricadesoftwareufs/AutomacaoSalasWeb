using Microsoft.EntityFrameworkCore;
using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly STR_DBContext _context;

        public EquipamentoService(STR_DBContext context)
        {
            _context = context;
        }

        public EquipamentoModel GetByIdEquipamento(int idEquipamento)
           => _context.Equipamento
                  .Where(eq => eq.Id == idEquipamento)
                  .Select(eq => new EquipamentoModel
                  {
                      Id = eq.Id,
                      Modelo = eq.Modelo,
                      Marca = eq.Marca,
                      Descricao = eq.Descricao,
                      Sala = eq.Sala,
                      TipoEquipamento = eq.TipoEquipamento
                  }).FirstOrDefault();


        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala && eq.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento
                   }).FirstOrDefault();


        public List<EquipamentoModel> GetByIdSala(int idSala)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala)
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento
                   }).ToList();

        public List<EquipamentoModel> GetAll() => _context.Equipamento.Select(e => new EquipamentoModel {Id = e.Id, Modelo = e.Modelo, Descricao = e.Descricao, TipoEquipamento = e.TipoEquipamento, Marca = e.Marca, Sala = e.Sala }).ToList();

        public bool Insert(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);
                _context.Add(equip);
                int inserted = _context.SaveChanges();
                _context.Entry(equip).GetDatabaseValues();

                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (inserted == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.AddAll(codigosEntity);
                }
                return Convert.ToBoolean(inserted);


            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private static Equipamento SetEntity(EquipamentoModel model)
        {
            Equipamento entity = new Equipamento {
                Id = model.Id,
                Descricao  = model.Descricao,
                Marca = model.Marca,
                Modelo = model.Modelo,
                TipoEquipamento = model.TipoEquipamento,
                Sala = model.Sala
            };
            return entity;
        }
    }
}
