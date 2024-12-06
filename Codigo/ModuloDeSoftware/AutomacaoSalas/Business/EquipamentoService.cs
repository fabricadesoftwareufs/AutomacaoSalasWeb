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
        private readonly SalasDBContext _context;

        public EquipamentoService(SalasDBContext context)
        {
            _context = context;
        }

        public EquipamentoModel GetByIdEquipamento(int idEquipamento)
           => _context.Equipamentos
                  .Where(eq => eq.Id == idEquipamento)
                  .Select(eq => new EquipamentoModel
                  {
                      Id = eq.Id,
                      Modelo = eq.Modelo,
                      Marca = eq.Marca,
                      Descricao = eq.Descricao,
                      Sala = eq.Sala,
                      TipoEquipamento = eq.TipoEquipamento,
                      HardwareDeSala = (uint)eq.HardwareDeSala
                  }).FirstOrDefault();


        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
       => _context.Equipamentos
                   .Where(eq => eq.Sala == idSala && eq.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento,
                       HardwareDeSala = (uint)eq.HardwareDeSala
                   }).FirstOrDefault();


        public List<EquipamentoModel> GetByIdSala(int idSala)
       => _context.Equipamentos
                   .Where(eq => eq.Sala == idSala)
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento,
                       HardwareDeSala = (uint)eq.HardwareDeSala
                   }).ToList();

        public List<EquipamentoModel> GetAll() => _context.Equipamentos.Select(e => new EquipamentoModel { Id = e.Id, Modelo = e.Modelo, Descricao = e.Descricao, TipoEquipamento = e.TipoEquipamento, Marca = e.Marca, Sala = e.Sala, HardwareDeSala = e.HardwareDeSala != null ? (uint)e.HardwareDeSala : 0 }).ToList();

        public bool Insert(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);
                IMonitoramentoService monitoramentoService = new MonitoramentoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);

                _context.Add(equip);
                int inserted = _context.SaveChanges();
                _context.Entry(equip).Reload();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (inserted == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.AddAll(codigosEntity);

                    monitoramentoService.Insert(new MonitoramentoModel { Estado = false, EquipamentoId = equip.Id });;
                }
                return Convert.ToBoolean(inserted);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public bool Update(EquipamentoViewModel entity)
        {
            try
            {
                ICodigoInfravermelhoService codigoInfravermelhoService = new CodigoInfravermelhoService(_context);

                var equip = SetEntity(entity.EquipamentoModel);
                _context.Equipamentos.Update(equip);
                int updated = _context.SaveChanges();
                var codigosEntity = new List<CodigoInfravermelhoModel>();
                if (updated == 1)
                {
                    entity.Codigos.ForEach(c => codigosEntity.Add(new CodigoInfravermelhoModel { Codigo = c.Codigo, IdEquipamento = equip.Id, IdOperacao = c.IdOperacao }));
                    codigoInfravermelhoService.UpdateAll(codigosEntity);
                }
                return Convert.ToBoolean(updated);


            }
            catch (Exception e)
            {

                throw e;
            }

        }

        private static Equipamento SetEntity(EquipamentoModel model)
        {
            Equipamento entity = new Equipamento
            {
                Id = model.Id,
                Descricao = model.Descricao,
                Marca = model.Marca,
                Modelo = model.Modelo,
                TipoEquipamento = model.TipoEquipamento,
                Sala = model.Sala,
                HardwareDeSala = model.HardwareDeSala,
            };
            return entity;
        }

        public bool Remove(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var equipamento = _context.Equipamentos.FirstOrDefault(e => e.Id == id);

                    if (equipamento == null)
                    {
                        throw new ServiceException("Equipamento não encontrado.");
                    }

                    var monitoramentos = _context.Monitoramentos.Where(m => m.Equipamento == id).ToList();

                    if (monitoramentos.Any())
                    {
                        _context.Monitoramentos.RemoveRange(monitoramentos);
                        _context.SaveChanges(); 
                    }

                    var codigosInfravermelho = _context.Codigoinfravermelhos.Where(ci => ci.Equipamento == id).ToList();

                    if (codigosInfravermelho.Any())
                    {
                        _context.Codigoinfravermelhos.RemoveRange(codigosInfravermelho);
                        _context.SaveChanges(); 
                    }

                    _context.Equipamentos.Remove(equipamento);
                    var save = _context.SaveChanges() > 0;

                    transaction.Commit();
                    return save;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }
    }
}
