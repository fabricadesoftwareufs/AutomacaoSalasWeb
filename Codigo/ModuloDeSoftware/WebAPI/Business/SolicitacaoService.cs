using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SolicitacacaoService : ISolicitacaoService
    {
        private readonly SalasDBContext _context;
        public SolicitacacaoService(SalasDBContext context)
        {
            _context = context;
        }

        public List<SolicitacaoModel> GetAll()
            => _context.Solicitacao
                .Select(s => new SolicitacaoModel
                {
                    Id = s.Id,
                    IdHardware = s.IdHardware,
                    IdHardwareAtuador = s.IdHardwareAtuador,
                    Payload = s.Payload,
                    DataSolicitacao = s.DataSolicitacao,
                    DataFinalizacao = s.DataFinalizacao,
                    TipoSolicitacao = s.TipoSolicitacao

                }).ToList();

        public SolicitacaoModel GetById(int id)
            => _context.Solicitacao
                .Where(s => s.Id == id)
                .Select(s => new SolicitacaoModel
                {
                    Id = s.Id,
                    IdHardware = s.IdHardware,
                    IdHardwareAtuador = s.IdHardwareAtuador,
                    Payload = s.Payload,
                    DataSolicitacao = s.DataSolicitacao,
                    DataFinalizacao = s.DataFinalizacao,
                    TipoSolicitacao = s.TipoSolicitacao

                }).FirstOrDefault();

        private static Solicitacao SetEntity(SolicitacaoModel model)
            => new Solicitacao
            {
                Id = model.Id,
                Payload = model.Payload,
                IdHardware = model.IdHardware,
                IdHardwareAtuador = model.IdHardwareAtuador,
                DataFinalizacao = model.DataFinalizacao,
                DataSolicitacao = model.DataSolicitacao,
                TipoSolicitacao = model.TipoSolicitacao
            };

        public List<SolicitacaoModel> GetByIdHardware(int idHardware, string tipo, bool todos = false)
        {
            var query = todos ?
                _context.Solicitacao.Where(s => s.IdHardware == idHardware && s.TipoSolicitacao.Contains(tipo)) :
                _context.Solicitacao.Where(s => s.IdHardware == idHardware && s.TipoSolicitacao.Contains(tipo) && !s.DataFinalizacao.HasValue);

            return query.Select(s => new SolicitacaoModel
            {
                Id = s.Id,
                IdHardware = s.IdHardware,
                IdHardwareAtuador = s.IdHardwareAtuador,
                Payload = s.Payload,
                DataSolicitacao = s.DataSolicitacao,
                DataFinalizacao = s.DataFinalizacao,
                TipoSolicitacao = s.TipoSolicitacao
            }).ToList();
        }

        public List<SolicitacaoModel> GetByIdHardwareAtuador(int idHardwareAtuador)
        => _context.Solicitacao
                .Where(s => s.IdHardwareAtuador == idHardwareAtuador && !s.DataFinalizacao.HasValue)
                .Select(s => new SolicitacaoModel
                {
                    Id = s.Id,
                    IdHardware = s.IdHardware,
                    IdHardwareAtuador = s.IdHardwareAtuador,
                    Payload = s.Payload,
                    DataSolicitacao = s.DataSolicitacao,
                    DataFinalizacao = s.DataFinalizacao,
                    TipoSolicitacao = s.TipoSolicitacao
                }).ToList();

        public bool Insert(SolicitacaoModel entity)
        {
            try
            {
                _context.Solicitacao.Add(SetEntity(entity));
                return _context.SaveChanges() == 1;
            } catch(Exception)
            {
                return false;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                var solicitacao = _context.Solicitacao.Where(s => s.Id == id);
                if (solicitacao != null)
                {
                    _context.Remove(solicitacao);
                    return _context.SaveChanges() == 1;
                }
            } catch(Exception)
            {
                return false;
            }
           
            return false;
        }

        public bool Update(SolicitacaoModel entity)
        {
            try
            {
                _context.Solicitacao.Update(SetEntity(entity));
                return _context.SaveChanges() == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
