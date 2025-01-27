using Model;
using Model.AuxModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    /// <summary>
    /// Serviço para gerenciar operações relacionadas às salas.
    /// </summary>
    public class SalaService : ISalaService
    {
        private readonly SalasDBContext _context;

        /// <summary>
        /// Construtor que inicializa o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public SalaService(SalasDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as salas.
        /// </summary>
        /// <returns>Lista de modelos de sala.</returns>
        public List<SalaModel> GetAll() => _context.Salas.Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.IdBloco }).ToList();

        /// <summary>
        /// Obtém uma sala pelo seu ID.
        /// </summary>
        /// <param name="id">ID da sala.</param>
        /// <returns>Modelo de sala.</returns>
        public SalaModel GetById(uint id) => _context.Salas.Where(s => s.Id == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.IdBloco, BlocoTitulo = s.IdBlocoNavigation.Titulo }).FirstOrDefault();

        /// <summary>
        /// Obtém todas as salas de um determinado bloco.
        /// </summary>
        /// <param name="id">ID do bloco.</param>
        /// <returns>Lista de modelos de sala.</returns>
        public List<SalaModel> GetByIdBloco(uint id) => _context.Salas.Where(s => s.IdBloco == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.IdBloco }).ToList();

        /// <summary>
        /// Obtém uma sala pelo seu título.
        /// </summary>
        /// <param name="titulo">Título da sala.</param>
        /// <returns>Modelo de sala.</returns>
        public SalaModel GetByTitulo(string titulo) => _context.Salas.Where(s => s.Titulo.ToUpper().Equals(titulo.ToUpper())).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.IdBloco }).FirstOrDefault();

        /// <summary>
        /// Insere uma sala juntamente com seus hardwares associados.
        /// </summary>
        /// <param name="sala">Modelo auxiliar de sala contendo sala e hardwares.</param>
        /// <param name="idUsuario">ID do usuário.</param>
        /// <returns>Verdadeiro se a operação for bem-sucedida.</returns>
        public bool InsertSalaWithHardwares(SalaAuxModel sala, uint idUsuario)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var salaInserida = Insert(new SalaModel { Id = sala.Sala.Id, Titulo = sala.Sala.Titulo, BlocoId = sala.Sala.BlocoId });
                    if (salaInserida == null)
                        throw new ServiceException("Houve um problema ao cadastrar sala, tente novamente em alguns minutos!");

                    if (sala.HardwaresSala.Count > 0)
                    {
                        var _hardwareDeSalaService = new HardwareDeSalaService(_context);

                        foreach (var item in sala.HardwaresSala)
                        {

                            if (_hardwareDeSalaService.GetByMAC(item.MAC, idUsuario) != null)
                                throw new ServiceException("Já existe um dispositivo com o endereço MAC " + item.MAC + " informado, corrija e tente novamente!");

                            if (item.TipoHardwareId.Id == TipoHardwareModel.CONTROLADOR_DE_SALA && _hardwareDeSalaService.GetByIp(item.Ip, idUsuario) != null)
                                throw new ServiceException("Já existe um dispositivo com o endereço IP " + item.Ip + " informado, corrija e tente novamente!");

                            _hardwareDeSalaService.Insert(new HardwareDeSalaModel { MAC = item.MAC, SalaId = salaInserida.Id, TipoHardwareId = item.TipoHardwareId.Id, Ip = item.Ip }, idUsuario);
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Insere uma nova sala no banco de dados.
        /// </summary>
        /// <param name="salaModel">Modelo de sala a ser inserido.</param>
        /// <returns>Modelo de sala inserido ou nulo se falhar.</returns>
        public SalaModel Insert(SalaModel salaModel)
        {
            try
            {
                var sala = GetByTitulo(salaModel.Titulo);
                if (sala != null && sala.BlocoId == salaModel.BlocoId)
                    throw new ServiceException("Já existe uma sala com o mesmo título associada a este bloco.");

                var entity = new Sala();
                _context.Add(SetEntity(salaModel, entity));
                var save = _context.SaveChanges();

                if (save == 1)
                {
                    salaModel.Id = entity.Id; return salaModel;
                }
                else return null;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Remove uma sala pelo seu ID.
        /// </summary>
        /// <param name="id">ID da sala a ser removida.</param>
        /// <returns>Verdadeiro se a operação for bem-sucedida.</returns>
        public bool Remove(uint id)
        {
            var _hardwareSalaService = new HardwareDeSalaService(_context);
            var _minhaSalaService = new SalaParticularService(_context);
            var _horarioSalaService = new HorarioSalaService(_context);
            var _planejamentoService = new PlanejamentoService(_context);

            try
            {
                if (_hardwareSalaService.GetByIdSala(id).Count == 0 && _minhaSalaService.GetByIdSala(id).Count == 0 &&
                    _horarioSalaService.GetByIdSala(id).Count == 0 && _planejamentoService.GetByIdSala(id).Count == 0)
                {

                    var x = _context.Salas.Where(s => s.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1;
                    }
                }
                else throw new ServiceException("Esta sala não pode ser removida, pois existem outros registros associados a ela.");
            }
            catch (Exception e) { throw e; }

            return false;
        }

        /// <summary>
        /// Atualiza uma sala existente.
        /// </summary>
        /// <param name="entity">Modelo de sala a ser atualizado.</param>
        /// <returns>Verdadeiro se a operação for bem-sucedida.</returns>
        public bool Update(SalaModel entity)
        {
            try
            {
                var x = _context.Salas.Where(s => s.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e) { throw new ServiceException("Houve um problema ao atualizar registro, tente novamente em alguns minutos"); }

            return false;
        }

        /// <summary>
        /// Mapeia as propriedades do modelo de sala para a entidade de sala.
        /// </summary>
        /// <param name="model">Modelo de sala.</param>
        /// <param name="entity">Entidade de sala.</param>
        /// <returns>Entidade de sala com propriedades mapeadas.</returns>
        private static Sala SetEntity(SalaModel model, Sala entity)
        {
            entity.Id = model.Id;
            entity.Titulo = model.Titulo;
            entity.IdBloco = model.BlocoId;

            return entity;
        }

        /// <summary>
        /// Obtém uma lista de salas com seus títulos formatados.
        /// </summary>
        /// <returns>Lista de modelos de sala.</returns>
        public List<SalaModel> GetSelectedList()
            => _context.Salas.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} - {1}", s.Id, s.Titulo) }).ToList();

        /// <summary>
        /// Obtém todas as salas associadas a uma organização específica do usuário.
        /// </summary>
        /// <param name="idUsuario">ID do usuário.</param>
        /// <returns>Lista de modelos de sala.</returns>
        public List<SalaModel> GetAllByIdUsuarioOrganizacao(uint idUsuario)
        {
            var _blocoService = new BlocoService(_context);

            var todasSalas = GetAll();
            var blocos = _blocoService.GetAllByIdUsuarioOrganizacao(idUsuario);

            var query = (from sl in todasSalas
                         join bl in blocos on sl.BlocoId equals bl.Id
                         select new SalaModel
                         {
                             Id = sl.Id,
                             BlocoId = sl.BlocoId,
                             Titulo = sl.Titulo
                         }).ToList();

            return query;
        }
    }
}