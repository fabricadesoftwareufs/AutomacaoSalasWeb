using Model;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class UsuarioService : IService<UsuarioModel>
    {
        private readonly STR_DBContext _context;
        public UsuarioService(STR_DBContext context)
        {
            _context = context;
        }
        public List<UsuarioModel> GetAll()
            => _context.Usuario
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento),
                    Senha = u.Senha,
                    TipoUsuarioId = u.TipoUsuario
                }).ToList();

        public UsuarioModel GetById(int id)
            => _context.Usuario
                .Where(u => u.Id == id)
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento),
                    Senha = u.Senha,
                    TipoUsuarioId = u.TipoUsuario
                }).FirstOrDefault();

        public bool Insert(UsuarioModel entity)
        {
            _context.Add(SetEntity(entity, new Usuario()));
            return _context.SaveChanges() == 1 ? true : false;
        }

        public bool Remove(int id)
        {
            var x = _context.Usuario.Where(u => u.Id == id).FirstOrDefault();
            if (x != null)
            {
                _context.Remove(x);
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public bool Update(UsuarioModel entity)
        {
            var x = _context.Usuario.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity, x));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        private static Usuario SetEntity(UsuarioModel model, Usuario entity)
        {
            entity.Id = model.Id;
            entity.Cpf = model.Cpf;
            entity.Nome = model.Nome;
            entity.DataNascimento = model.DataNascimento;
            entity.Senha = model.Senha;
            entity.TipoUsuario = model.TipoUsuarioId;

            return entity;
        }
    }
}
