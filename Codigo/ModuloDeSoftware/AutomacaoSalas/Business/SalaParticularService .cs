﻿using Model;
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
        private readonly SalasDBContext _context;
        public SalaParticularService(SalasDBContext context)
        {
            _context = context;
        }

        public List<SalaParticularModel> GetAll()
            => _context.Salaparticulars
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.IdUsuario,
                    SalaId = sp.IdSala

                }).ToList();

        public SalaParticularModel GetById(uint id)
            => _context.Salaparticulars
                .Where(sp => sp.Id == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.IdUsuario,
                    SalaId = sp.IdSala
                }).FirstOrDefault();

        public List<SalaParticularModel> GetByIdSala(uint id)
            => _context.Salaparticulars
                .Where(sp => sp.IdSala == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.IdUsuario,
                    SalaId = sp.IdSala
                }).ToList();

        public SalaParticularModel GetByIdUsuarioAndIdSala(uint idUsuario, uint idSala)
           => _context.Salaparticulars
               .Where(sp => sp.IdUsuario == idUsuario && sp.IdSala == idSala)
               .Select(sp => new SalaParticularModel
               {
                   Id = sp.Id,
                   UsuarioId = sp.IdUsuario,
                   SalaId = sp.IdSala
               }).FirstOrDefault();

        public bool InsertListSalasParticulares(SalaParticularAuxModel entity)
        {
            if (entity.Responsaveis.Count == 0)
                throw new ServiceException("Você não adicionou nenhum responsável da sala.");

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

        public bool VerificaSalaExclusivaExistente(uint? idSalaExclusiva, uint idUsuario, uint idSala)
        {
            var salaExclusiva = GetByIdUsuarioAndIdSala(idUsuario, idSala);

            if (salaExclusiva == null)
                return false;
            else if (idSalaExclusiva != null)
                return salaExclusiva.Id == idSalaExclusiva ? false : true;

            return true;
        }

        public bool Remove(uint id)
        {
            try
            {
                var x = _context.Salaparticulars.Where(th => th.Id == id).FirstOrDefault();
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
                throw new ServiceException("A atualização não pode ser concluída, pois este usuário já está associado a esta sala em outro registro.");

            try
            {
                var x = _context.Salaparticulars.Where(th => th.Id == entity.Id).FirstOrDefault();
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
            entity.IdSala = model.SalaId;
            entity.IdUsuario = model.UsuarioId;

            return entity;
        }

        public List<SalaParticularModel> GetByIdUsuario(uint id)
         => _context.Salaparticulars
                .Where(sp => sp.IdUsuario == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.IdUsuario,
                    SalaId = sp.IdSala
                }).ToList();

        public List<SalaParticularModel> GetByIdOrganizacao(uint idOrganizacao)
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

        public bool RemoveByUsuario(uint id)
        {
            try
            {
                var x = _context.Salaparticulars.Where(th => th.IdUsuario == id);
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
