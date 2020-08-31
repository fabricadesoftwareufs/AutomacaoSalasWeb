using Model;
using Model.AuxModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SalaParticularService : ISalaParticularService
    {
        private readonly STR_DBContext _context;
        public SalaParticularService(STR_DBContext context)
        {
            _context = context;
        }

        public List<SalaParticularModel> GetAll()
            => _context.Salaparticular
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.Usuario,
                    SalaId = sp.Sala

                }).ToList();

        public SalaParticularModel GetById(int id)
            => _context.Salaparticular
                .Where(sp => sp.Id == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.Usuario,
                    SalaId = sp.Sala
                }).FirstOrDefault();

        public List<SalaParticularModel> GetByIdSala(int id)
            => _context.Salaparticular
                .Where(sp => sp.Sala == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.Usuario,
                    SalaId = sp.Sala
                }).ToList();

        public SalaParticularModel GetByIdUsuarioAndIdSala(int idUsuario, int idSala)
           => _context.Salaparticular
               .Where(sp => sp.Usuario == idUsuario && sp.Sala == idSala)
               .Select(sp => new SalaParticularModel
               {
                   Id = sp.Id,
                   UsuarioId = sp.Usuario,
                   SalaId = sp.Sala
               }).FirstOrDefault();

        public bool InsertListSalasParticulares(SalaParticularAuxModel entity)
        {
            if (entity.Responsaveis.Count == 0)
                throw new ServiceException("Você não adicionou nenhum responsável da sala!.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in entity.Responsaveis)
                    {
                        Insert(new SalaParticularModel
                        {
                            Id = entity.SalaParticular.Id,
                            SalaId = entity.SalaParticular.SalaId,
                            UsuarioId = item.Id
                        });
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        public bool Insert(SalaParticularModel entity)
        {

            if (!VerificaSalaExclusivaExistente(null, entity.UsuarioId, entity.SalaId))
            {
                try
                {
                    _context.Add(SetEntity(entity, new Salaparticular()));
                    var save = _context.SaveChanges() == 1 ? true : false;
                    return save;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return true;
        }

        public bool VerificaSalaExclusivaExistente(int? idSalaExclusiva, int idUsuario, int idSala)
        {
            var salaExclusiva = GetByIdUsuarioAndIdSala(idUsuario, idSala);

            if (salaExclusiva == null)
                return false;
            else if (idSalaExclusiva != null)
                return salaExclusiva.Id == idSalaExclusiva ? false : true;

            return true;
        }

        public bool Remove(int id)
        {
            try
            {
                var x = _context.Salaparticular.Where(th => th.Id == id).FirstOrDefault();
                if (x != null)
                {
                    _context.Remove(x);
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        public bool Update(SalaParticularModel entity)
        {
            if (VerificaSalaExclusivaExistente(entity.Id, entity.UsuarioId, entity.SalaId))
                throw new ServiceException("Atualização não pode ser concluida pois este usuário já esta associado a essa sala em outro registro!.");

            try
            {
                var x = _context.Salaparticular.Where(th => th.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        private static Salaparticular SetEntity(SalaParticularModel model, Salaparticular entity)
        {
            entity.Id = model.Id;
            entity.Sala = model.SalaId;
            entity.Usuario = model.UsuarioId;

            return entity;
        }

        public List<SalaParticularModel> GetByIdUsuario(int id)
         => _context.Salaparticular
                .Where(sp => sp.Usuario == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.Usuario,
                    SalaId = sp.Sala
                }).ToList();

        public List<SalaParticularModel> GetByIdOrganizacao(int idOrganizacao)
        {
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var query = (from sp in GetAll()
                         join sl in _salaService.GetAll() on sp.SalaId equals sl.Id
                         join bl in _blocoService.GetByIdOrganizacao(idOrganizacao) on sl.BlocoId equals bl.Id
                         select new SalaParticularModel
                         {
                             Id = sp.Id,
                             UsuarioId = sp.UsuarioId,
                             SalaId = sp.SalaId
                         }).ToList();

            return query;
        }

        public bool RemoveByUsuario(int id)
        {
            try
            {
                var x = _context.Salaparticular.Where(th => th.Usuario == id);
                if (x != null)
                {
                    _context.RemoveRange(x);
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }
    }
}
