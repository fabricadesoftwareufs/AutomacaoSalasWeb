using Microsoft.EntityFrameworkCore;
using Model;
using Model.ViewModel;
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
        private readonly SalasUfsDbContext _context;
        public UsuarioService(SalasUfsDbContext context)
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
                         select new UsuarioModel
                         {
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

        public UsuarioViewModel Insert(UsuarioViewModel entity)
        {
            var usuario = GetByCpf(entity.UsuarioModel.Cpf);
            if (usuario != null)
                throw new ServiceException("Já existe um cpf cadastrado no sistema.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    entity.UsuarioModel.TipoUsuarioId = entity.TipoUsuarioModel.Id;

                    var usuarioAdd = SetEntity(entity.UsuarioModel);
                    _context.Add(usuarioAdd);
                    _context.SaveChanges();
                    _context.Entry(usuarioAdd).State = EntityState.Detached;
                    entity.UsuarioModel.Id = usuarioAdd.Id;

                    var usuarioOrgService = new UsuarioOrganizacaoService(_context);

                    usuarioOrgService.Insert
                    (
                        new UsuarioOrganizacaoModel
                        {
                            OrganizacaoId = entity.OrganizacaoModel.Id,
                            UsuarioId = entity.UsuarioModel.Id
                        }
                     );

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
            return entity;
        }

        public bool Remove(int id)
        {

            var plan = new PlanejamentoService(_context);
            var particular = new SalaParticularService(_context);
            var horarios = new HorarioSalaService(_context);
            var usuarioOrg = new UsuarioOrganizacaoService(_context);


            var x = _context.Usuario.Where(u => u.Id == id).FirstOrDefault();
            if (x != null)
            {
                //atualizar depois para escolher da qual org
                //removendo tudo associado ao usuario, pois na tela de remover irá aparecer se tem itens associados
                // se o usuario concordar, obviamente irá excluir tudo

                //planejamentos
                plan.RemoveByUsuario(x.Id);

                //salas particulares
                particular.RemoveByUsuario(x.Id);

                //reservas
                horarios.RemoveByUsuario(x.Id);

                // associacao 
                usuarioOrg.RemoveByUsuario(x.Id);
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Remove(x);
                        transaction.Commit();
                        return _context.SaveChanges() == 1 ? true : false;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
            else
            {
                new ServiceException("Houve um erro ao remover o usuário!");
            }
            return false;
        }

        public bool Update(UsuarioModel entity)
        {
            var x = _context.Usuario.Where(th => th.Id == entity.Id).FirstOrDefault();
            if (x != null)
            {
                _context.Update(SetEntity(entity));
                return _context.SaveChanges() == 1 ? true : false;
            }

            return false;
        }

        public UsuarioViewModel RetornLoggedUser(ClaimsIdentity claimsIdentity)
        {
            var usuario = new UsuarioViewModel
            {
                UsuarioModel = new UsuarioModel
                {
                    Id = int.Parse(claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault()),
                    Cpf = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.UserData).Select(s => s.Value).FirstOrDefault(),
                    Nome = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault(),
                },

                TipoUsuarioModel = new TipoUsuarioModel
                {
                    Descricao = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.Role).Select(s => s.Value).FirstOrDefault()
                }
            };

            return usuario;
        }

        private static Usuario SetEntity(UsuarioModel model)
        => new Usuario()
        {
            Id = model.Id,
            Cpf = model.Cpf,
            Nome = model.Nome,
            DataNascimento = model.DataNascimento,
            Senha = model.Senha,
            TipoUsuario = model.TipoUsuarioId
        };

        public UsuarioModel GetByCpf(string cpf)
         => _context.Usuario
                .Where(u => u.Cpf.Equals(cpf))
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento),
                    Senha = u.Senha,
                    TipoUsuarioId = u.TipoUsuario
                }).FirstOrDefault();

        public List<UsuarioModel> GetAllByIdsOrganizacao(List<int> ids) => _context.Usuario.Where(u => ids.Contains(u.Id)).Select(u => new UsuarioModel { Id = u.Id, Cpf = u.Cpf, DataNascimento = (DateTime)u.DataNascimento, Nome = u.Nome, TipoUsuarioId = u.TipoUsuario, Senha = u.Senha }).ToList();
    }
}
