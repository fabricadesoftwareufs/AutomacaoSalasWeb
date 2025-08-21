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
        private readonly SalasDBContext _context;
        public UsuarioService(SalasDBContext context)
        {
            _context = context;
        }
        public List<UsuarioModel> GetAll()
            => _context.Usuarios
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento)
                }).ToList();

        /// <summary>
        /// Obtém todos os usuários com execção do usuário logado
        /// </summary>
        /// <param name="idUser">Id do usuário logado</param>
        /// <returns>Uma lista de usuários</returns>
        public List<UsuarioModel> GetAllExceptAuthenticatedUser(uint idUser)
            => _context.Usuarios
                .Where(u => u.Id != idUser)
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento)
                }).ToList();

        public UsuarioModel GetById(uint id)
            => _context.Usuarios
                .Where(u => u.Id == id)
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento)
                }).FirstOrDefault();


        public List<UsuarioModel> GetByIdOrganizacao(uint id)
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

        /*public UsuarioModel GetByLoginAndPass(string login, string senha)
            => _context.Usuarios
                .Where(u => u.Cpf.Equals(login) && u.Senha.Equals(senha))
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento),
                    Senha = u.Senha
                }).FirstOrDefault();
        */
        public UsuarioViewModel Insert(UsuarioViewModel entity)
        {
            var usuario = GetByCpf(entity.UsuarioModel.Cpf);
            if (usuario != null)
                throw new ServiceException("Este CPF já está cadastrado no sistema.");

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
                            UsuarioId = entity.UsuarioModel.Id,
                            IdTipoUsuario = entity.UsuarioModel.TipoUsuarioId,
                            DataCadastro = DateTime.Now
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


            var x = _context.Usuarios.Where(u => u.Id == id).FirstOrDefault();
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
                new ServiceException("Houve um erro ao remover o usuário.");
            }
            return false;
        }

        public bool Update(UsuarioModel entity)
        {
            var usuario = _context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == entity.Id);
            if (usuario == null)
                return false;

            var _entity = SetEntity(entity);

            var userOrganizacao = _context.Usuarioorganizacaos.Where(x => x.IdUsuario == entity.Id);
            _context.Usuarioorganizacaos.RemoveRange(userOrganizacao);

            _entity.Usuarioorganizacaos = [ new Usuarioorganizacao
            {
                IdOrganizacao = entity.IdOrganizacao,
                IdTipoUsuario = entity.IdTipoUsuario,
                IdUsuario = entity.Id,
            }];

            _context.Usuarioorganizacaos.AddRange(_entity.Usuarioorganizacaos);

            var user = _context.Update(_entity);
            var n = _context.SaveChanges();
            return n > 0;
        }

        public UsuarioViewModel GetAuthenticatedUser(ClaimsIdentity claimsIdentity)
        {
            // Verificar se as claims necessárias existem
            var idClaim = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault();
            var cpfClaim = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.UserData).Select(s => s.Value).FirstOrDefault();
            var nomeClaim = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.NameIdentifier).Select(s => s.Value).FirstOrDefault();
            var roleClaim = claimsIdentity.Claims.Where(s => s.Type == ClaimTypes.Role).Select(s => s.Value).FirstOrDefault();

            // Se não tem o ID do sistema legado, tentar buscar pelo CPF
            if (string.IsNullOrEmpty(idClaim) && !string.IsNullOrEmpty(cpfClaim))
            {
                var usuarioLegado = GetByCpf(cpfClaim);
                if (usuarioLegado != null)
                {
                    var tipoUsuario = new TipoUsuarioService(_context).GetTipoUsuarioByUsuarioId(usuarioLegado.Id);
                    
                    return new UsuarioViewModel
                    {
                        UsuarioModel = usuarioLegado,
                        TipoUsuarioModel = tipoUsuario ?? new TipoUsuarioModel { Descricao = TipoUsuarioModel.ROLE_COLABORADOR }
                    };
                }
            }

            // Se tem todas as claims, usar elas
            if (!string.IsNullOrEmpty(idClaim) && uint.TryParse(idClaim, out uint userId))
            {
                var usuario = new UsuarioViewModel
                {
                    UsuarioModel = new UsuarioModel
                    {
                        Id = userId,
                        Cpf = cpfClaim ?? "",
                        Nome = nomeClaim ?? "",
                    },
                    TipoUsuarioModel = new TipoUsuarioModel
                    {
                        Descricao = roleClaim ?? TipoUsuarioModel.ROLE_COLABORADOR
                    }
                };

                return usuario;
            }

            // Fallback: tentar pelo nome do usuário (Identity)
            var identityName = claimsIdentity.Name;
            if (!string.IsNullOrEmpty(identityName))
            {
                var usuarioLegado = GetByCpf(identityName);
                if (usuarioLegado != null)
                {
                    var tipoUsuario = new TipoUsuarioService(_context).GetTipoUsuarioByUsuarioId(usuarioLegado.Id);
                    
                    return new UsuarioViewModel
                    {
                        UsuarioModel = usuarioLegado,
                        TipoUsuarioModel = tipoUsuario ?? new TipoUsuarioModel { Descricao = TipoUsuarioModel.ROLE_COLABORADOR }
                    };
                }
            }

            // Se chegou até aqui, criar um usuário temporário para evitar erros
            return new UsuarioViewModel
            {
                UsuarioModel = new UsuarioModel
                {
                    Id = 0,
                    Cpf = cpfClaim ?? "",
                    Nome = nomeClaim ?? "Usuário",
                },
                TipoUsuarioModel = new TipoUsuarioModel
                {
                    Descricao = TipoUsuarioModel.ROLE_COLABORADOR
                }
            };
        }

        private static Usuario SetEntity(UsuarioModel model)
        => new Usuario()
        {
            Id = model.Id,
            Cpf = model.Cpf,
            Nome = model.Nome,
            DataNascimento = model.DataNascimento
        };

        public UsuarioModel GetByCpf(string cpf)
         => _context.Usuarios
                .Where(u => u.Cpf.Equals(cpf))
                .Select(u => new UsuarioModel
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Nome = u.Nome,
                    DataNascimento = Convert.ToDateTime(u.DataNascimento)
                }).FirstOrDefault();

        public List<UsuarioModel> GetAllByIdsOrganizacao(List<uint> ids) => _context.Usuarios.Where(u => ids.Contains(u.Id)).Select(u => new UsuarioModel { Id = u.Id, Cpf = u.Cpf, DataNascimento = (DateTime)u.DataNascimento, Nome = u.Nome }).ToList();

    }
}
