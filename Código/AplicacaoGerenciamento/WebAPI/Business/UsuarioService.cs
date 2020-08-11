using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Service
{
    public class UsuarioService : IUsuarioService
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


        public List<UsuarioModel> GetByIdOrganizacao(int id)
        {
            var _usuarioOrganizacaoService = new UsuarioOrganizacaoService(_context);
            
            var usuarioOrganizacao = _usuarioOrganizacaoService.GetByIdOrganizacao(id); 
            var todosUsuarios = GetAll();

            var query = (from usuario in todosUsuarios
                         join usuarioOrg in usuarioOrganizacao
                         on usuario.Id equals usuarioOrg.UsuarioId
                         select new UsuarioModel {
                            Id = usuario.Id,
                            Cpf = usuario.Cpf,
                            Nome = usuario.Nome,
                            DataNascimento = usuario.DataNascimento,
                            TipoUsuarioId = usuario.TipoUsuarioId
                         }).ToList();

            return query;
        }
        
        public UsuarioModel GetByLoginAndPass(string login, string senha)
            => _context.Usuario
                .Where(u => u.Cpf.Equals(login) && u.Senha.Equals(senha))
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

        public UsuarioModel RetornLoggedUser(ClaimsIdentity claimsIdentity)
        {
            var usuario = new UsuarioModel
            {
                Id = int.Parse(claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault()),
                Cpf = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.UserData).Select(s => s.Value).FirstOrDefault(),
                Nome = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault(),

            };

            return usuario;
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
