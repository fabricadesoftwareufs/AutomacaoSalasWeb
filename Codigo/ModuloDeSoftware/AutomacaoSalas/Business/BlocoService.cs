using Model;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Persistence;

namespace Service
{
    public class BlocoService : IBlocoService
    {
        private readonly SalasDBContext _context;
        public BlocoService(SalasDBContext context)
        {
            _context = context;
        }
        public List<BlocoModel> GetAll() => _context.Blocos.Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).ToList();

        public BlocoModel GetById(uint id)
        {
            var blocoModel = (from b in _context.Blocos
                              join o in _context.Organizacaos on b.IdOrganizacao equals o.Id
                              where b.Id == id
                              select new BlocoModel
                              {
                                  Id = b.Id,
                                  OrganizacaoId = b.IdOrganizacao,
                                  Titulo = b.Titulo,
                                  NomeOrganizacao = o.RazaoSocial 
                              }).FirstOrDefault();

            return blocoModel;
        }


        public List<BlocoModel> GetByIdOrganizacao(uint id) => _context.Blocos.Where(b => b.IdOrganizacao == id).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).ToList();

        public BlocoModel GetByTitulo(string titulo, uint idOrganizacao) => _context.Blocos.Where(b => b.Titulo.ToUpper().Equals(titulo.ToUpper()) && b.IdOrganizacao == idOrganizacao).Select(b => new BlocoModel { Id = b.Id, OrganizacaoId = b.IdOrganizacao, Titulo = b.Titulo }).FirstOrDefault();

        public bool InsertBlocoWithHardware(BlocoModel blocoModel, uint idUsuario)
        {
            var blocoInserido = new BlocoModel();
            try
            {
                blocoInserido = Insert(new BlocoModel { Id = blocoModel.Id, OrganizacaoId = blocoModel.OrganizacaoId, Titulo = blocoModel.Titulo });
                if (blocoInserido == null) throw new ServiceException("Houve um problema ao cadastrar bloco, tente novamente em alguns minutos!");
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
        public BlocoModel Insert(BlocoModel blocoModel)
        {
            try
            {
                if (GetByTitulo(blocoModel.Titulo, blocoModel.OrganizacaoId) != null)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                var entity = new Bloco();
                _context.Add(SetEntity(blocoModel, entity));
                var save = _context.SaveChanges() == 1 ? true : false;

                if (save)
                {
                    blocoModel.Id = entity.Id; return blocoModel;
                }
                else return null;
            }
            catch (Exception e) { throw e; }
        }

        public bool Remove(uint id)
        {
            var _salaService = new SalaService(_context);
            try
            {
                if (_salaService.GetByIdBloco(id).Count == 0)
                {
                    var x = _context.Blocos.Where(b => b.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1;
                    }
                }
                else throw new ServiceException("Esse Bloco não pode ser removido pois possui hardwares e salas associados a ele!");

            }
            catch (Exception e) { throw e; }

            return false;
        }

        public bool Update(BlocoModel entity)
        {
            try
            {
                var bloco = GetByTitulo(entity.Titulo, entity.OrganizacaoId);
                if (bloco != null && bloco.Id != entity.Id)
                    throw new ServiceException("Essa organização já possui um bloco com esse nome");

                var x = _context.Blocos.Where(b => b.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e) { throw e; }

            return false;
        }

        private static Bloco SetEntity(BlocoModel model, Bloco entity)
        {
            entity.Id = model.Id;
            entity.IdOrganizacao = model.OrganizacaoId;
            entity.Titulo = model.Titulo;

            return entity;
        }


        public List<BlocoModel> GetAllByIdUsuarioOrganizacao(uint idUsuario)
        {
            var queryUser = from usuario in _context.Usuarioorganizacaos
                            where usuario.IdUsuario == idUsuario
                            select usuario;
            var usuarioorganizacao = queryUser.FirstOrDefault();

            var query = from bloco in _context.Blocos
                        where bloco.IdOrganizacao == usuarioorganizacao.IdOrganizacao
                        select new BlocoModel
                        {
                            Id = bloco.Id,
                            Titulo = bloco.Titulo,
                            OrganizacaoId = bloco.IdOrganizacao

                        };
            return query.ToList();
        }
    }
}
