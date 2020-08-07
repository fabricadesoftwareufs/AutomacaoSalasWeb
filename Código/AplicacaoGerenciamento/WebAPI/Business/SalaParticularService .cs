using Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SalaParticularService : IService<SalaParticularModel>
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
        public int Id { get; set; }

        public SalaParticularModel GetById(int id)
            => _context.Salaparticular
                .Where(sp => sp.Id == id)
                .Select(sp => new SalaParticularModel
                {
                    Id = sp.Id,
                    UsuarioId = sp.Usuario,
                    SalaId = sp.Sala
                }).FirstOrDefault();

        public SalaParticularModel GetByIdUsuarioAndIdSala(int idUsuario, int idSala)
           => _context.Salaparticular
               .Where(sp => sp.Usuario == idUsuario && sp.Sala == idSala)
               .Select(sp => new SalaParticularModel
               {
                   Id = sp.Id,
                   UsuarioId = sp.Usuario,
                   SalaId = sp.Sala
               }).FirstOrDefault();

        public bool InsertListSalasParticulares(SalaParticularViewModel entity)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in entity.Responsaveis)
                    {
                        Insert(new SalaParticularModel
                        {
                            Id = entity.Id,
                            SalaId = entity.SalaId.Id,
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
            if (VerificaSalaExclusivaExistente(entity.UsuarioId,entity.SalaId)) {
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

        public bool VerificaSalaExclusivaExistente(int idUsuario, int idSala) 
        {
            return GetByIdUsuarioAndIdSala(idUsuario, idSala) == null ? false : true;
        }

        public bool Remove(int id)
        {
            var x = _context.Salaparticular.Where(th => th.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(SalaParticularModel entity)
        {
            var x = _context.Salaparticular.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
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

        public List<SalaParticularModel> GetSelectedList()
        {
            throw new System.NotImplementedException();
        }
    }
}
