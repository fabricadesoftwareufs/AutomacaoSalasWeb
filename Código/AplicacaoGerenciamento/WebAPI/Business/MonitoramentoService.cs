using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class MonitoramentoService : IMonitoramentoService
    {
        private readonly STR_DBContext _context;
        public MonitoramentoService(STR_DBContext context)
        {
            _context = context;
        }

        public List<MonitoramentoModel> GetAll() => _context.Monitoramento.Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).ToList();

        public MonitoramentoModel GetById(int id) => _context.Monitoramento.Where(m => m.Id == id).Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).FirstOrDefault();

        public MonitoramentoModel GetByIdSala(int idSala) => _context.Monitoramento.Where(m => m.Sala == idSala).Select(m => new MonitoramentoModel { Id = m.Id, SalaId = m.Sala, ArCondicionado = Convert.ToBoolean(m.ArCondicionado), Luzes = Convert.ToBoolean(m.Luzes) }).FirstOrDefault();

        public bool Insert(MonitoramentoModel model)
        {
            try
            {
                if (GetByIdSala(model.SalaId) != null)
                    return true;

                _context.Add(SetEntity(model));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Update(MonitoramentoModel model)
        {
            try
            {
                _context.Update(SetEntity(model));
                return _context.SaveChanges() == 1 ? true : false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private Monitoramento SetEntity(MonitoramentoModel model)
        {
            return new Monitoramento
            { 
                Id =  model.Id,
                Luzes = Convert.ToByte(model.Luzes),
                ArCondicionado = Convert.ToByte(model.ArCondicionado),
                Sala = model.SalaId
            };
        }
    }
}
