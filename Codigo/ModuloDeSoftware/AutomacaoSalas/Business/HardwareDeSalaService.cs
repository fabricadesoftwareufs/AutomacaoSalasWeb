using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using static Model.HardwareDeSalaModel;

namespace Service
{
    public class HardwareDeSalaService : IHardwareDeSalaService
    {
        private readonly SalasDBContext _context;
        public HardwareDeSalaService(SalasDBContext context)
        {
            _context = context;
        }

        public List<HardwareDeSalaModel> GetAll() => _context.Hardwaredesalas.Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Token = h.Token }).ToList();
        public List<HardwareDeSalaModel> GetAllHardwaresSalaByUsuarioOrganizacao(uint idUsuario)
        {
            var _salaService = new SalaService(_context);
            var hardwares = GetAll();

            var query = (from sl in _salaService.GetAllByIdUsuarioOrganizacao(idUsuario)
                         join hr in hardwares on sl.Id equals hr.SalaId
                         select new HardwareDeSalaModel
                         {
                             Id = hr.Id,
                             MAC = hr.MAC,
                             SalaId = hr.SalaId,
                             TipoHardwareId = hr.TipoHardwareId,
                             Ip = hr.Ip,
                             Uuid = hr.Uuid,
                             Token = hr.Token
                         }).ToList();

            return query;
        }

        public HardwareDeSalaModel GetById(uint id) => _context.Hardwaredesalas.Where(h => h.Id == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

        public HardwareDeSalaModel GetByIdAndType(int id, int tipo) => _context.Hardwaredesalas.Where(h => h.Id == id && h.IdTipoHardware == tipo).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token, Registrado = Convert.ToBoolean(h.Registrado) }).FirstOrDefault();


        public List<HardwareDeSalaModel> GetByIdSala(uint id)
        {
            var hardware = _context.Hardwaredesalas
                                  .Where(hs => hs.IdSala == id)
                                    .Select(h => 
                                        new HardwareDeSalaModel 
                                        { 
                                            Id = h.Id, 
                                            MAC = h.Mac, 
                                            SalaId = h.IdSala, 
                                            TipoHardwareId = h.IdTipoHardware, 
                                            Ip = h.Ip, 
                                            Uuid = h.Uuid, 
                                            Token = h.Token,
                                        }
                                      ).ToList();


            return hardware; 
        }

        public List<HardwareDeSalaModel> GetSensorsAndActuactorsByIdSala(uint id)
        {
            var hardwares = _context.Hardwaredesalas
                                  .Join(_context.Equipamentos,
                                     hard => hard.Id,
                                     equip => equip.IdHardwareDeSala,
                                     (hard, equip) => new { Hadware = hard, Equipamento = equip })
                                  .Where(hs => hs.Equipamento.IdHardwareDeSala == hs.Hadware.Id && hs.Hadware.IdSala == id && hs.Hadware.IdTipoHardware  == (int)HardwareDeSalaModel.TIPO.MODULO_ATUADOR)
                                    .Select(h =>
                                        new HardwareDeSalaModel
                                        {
                                            Id = h.Hadware.Id,
                                            MAC = h.Hadware.Mac,
                                            SalaId = h.Hadware.IdSala,
                                            TipoHardwareId = h.Hadware.IdTipoHardware,
                                            Ip = h.Hadware.Ip,
                                            Uuid = h.Hadware.Uuid,
                                            Token = h.Hadware.Token,
                                            TipoEquipamento = h.Equipamento.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR) ? 1 : 0
                                        }
                                      ).Union(
                                         _context.Hardwaredesalas
                                        .Where(hs => hs.IdSala == id && hs.IdTipoHardware == (int)HardwareDeSalaModel.TIPO.MODULO_SENSOR)
                                          .Select(h =>
                                              new HardwareDeSalaModel
                                              {
                                                  Id = h.Id,
                                                  MAC = h.Mac,
                                                  SalaId = h.IdSala,
                                                  TipoHardwareId = h.IdTipoHardware,
                                                  Ip = h.Ip,
                                                  Uuid = h.Uuid,
                                                  Token = h.Token,
                                                  TipoEquipamento = 0
                                              }
                                            )
                                    );

            return hardwares.ToList();
        }



        public HardwareDeSalaModel GetByMAC(string mac)
        {
            var hardware = _context.Hardwaredesalas.Where(h => h.Mac.ToLower().Equals(mac.ToLower())).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetByUuid(string uuid)
        {
            var hardware = _context.Hardwaredesalas.Where(h => h.Uuid.Equals(uuid)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetHardwaresByIdSala(uint idSala)
        {
            var hardware = _context
                            .Hardwaredesalas
                            .Where(h => h.Uuid.Equals(idSala)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetByMAC(string mac, uint idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesalas.Where(h => h.Mac.ToUpper().Equals(mac.ToUpper())).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            if (hardware != null)
            {
                var bloco = _blocoService.GetById(_salaService.GetById(hardware.SalaId).BlocoId);
                var orgs = _usuarioOrganizacao.GetByIdUsuario(idUsuario);

                foreach (var item in orgs)
                    if (bloco.OrganizacaoId == item.OrganizacaoId)
                        return hardware;
            }

            return null;
        }

        public HardwareDeSalaModel GetByIp(string ip, uint idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesalas.Where(h => h.Ip == ip).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            if (hardware != null)
            {
                var bloco = _blocoService.GetById(_salaService.GetById(hardware.SalaId).BlocoId);
                var orgs = _usuarioOrganizacao.GetByIdUsuario(idUsuario);

                foreach (var item in orgs)
                    if (bloco.OrganizacaoId == item.OrganizacaoId)
                        return hardware;
            }

            return null;
        }

        public HardwareDeSalaModel GetByIp(string ip)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesalas.Where(h => h.Ip == ip).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public bool Insert(HardwareDeSalaModel entity, uint idUsuario)
        {

            try
            {
                var hardware = GetByMAC(entity.MAC, idUsuario);
                if (hardware != null)
                    throw new ServiceException("Já existe um dispositivo com esse endereço MAC");

                if (entity.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA && GetByIp(entity.Ip, idUsuario) != null)
                        throw new ServiceException("Já existe um dispositivo com esse endereço IP");

                string newUUID = Methods.GenerateUUID();

                entity.Uuid = newUUID;

                entity.Token = Methods.HashSHA256(Methods.RandomStr(64));
                _context.Add(SetEntity(entity, new Hardwaredesala()));
                return _context.SaveChanges() == 1;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool Remove(int id)
        {
            try
            {
                var x = _context.Hardwaredesalas.Where(tu => tu.Id == id).FirstOrDefault();
                if (x != null)
                {
                    _context.Remove(x);
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        }

        public bool Update(HardwareDeSalaModel entity, uint idUsuario)
        {
            try
            {
                var hardware = GetByMAC(entity.MAC, idUsuario);
                if (hardware != null && hardware.Id != entity.Id)
                    throw new ServiceException("Já existe um dispositivo com esse endereço MAC");
                if (entity.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                {
                    hardware = GetByIp(entity.Ip, idUsuario);
                    if (hardware != null && hardware.Id != entity.Id)
                        throw new ServiceException("Já existe um dispositivo com esse endereço IP");
                }

                var hardwareExistente = _context.Hardwaredesalas.Where(tu => tu.Id == entity.Id).FirstOrDefault();
                if (hardwareExistente != null)
                {
                    // Preserva o token e UUID existentes
                    entity.Token = hardwareExistente.Token;
                    entity.Uuid = hardwareExistente.Uuid;

                    _context.Update(SetEntity(entity, hardwareExistente));
                    return _context.SaveChanges() == 1;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        private static Hardwaredesala SetEntity(HardwareDeSalaModel model, Hardwaredesala entity)
        {
            entity.Id = model.Id;
            entity.Mac = model.MAC;
            entity.IdTipoHardware = model.TipoHardwareId;            
            entity.Registrado = Convert.ToSByte(model.Registrado);
            entity.IdSala = model.SalaId;
            entity.Ip = model.Ip;
            entity.Uuid = model.Uuid;
            entity.Token = model.Token;
            return entity;
        }

        public List<HardwareDeSalaModel> GetByIdSalaAndTipoHardware(uint id, int tipo)
        => _context.Hardwaredesalas.Where(h => h.IdSala == id && h.IdTipoHardware == tipo).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).ToList();

        public List<HardwareDeSalaModel> GetAtuadorByIdSala(uint id)
            => _context.Hardwaredesalas.Where(h => h.IdSala == id && h.IdTipoHardware == (int)TIPO.MODULO_ATUADOR).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Token = h.Token }).ToList();

        public HardwareDeSalaModel GetControladorByIdSala(uint idSala)
            => _context.Hardwaredesalas.Where(h => h.IdSala == idSala && h.IdTipoHardware == (int)TIPO.CONTROLADOR_SALA).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Token = h.Token }).FirstOrDefault();

        /// <summary>
        /// Remove da lista os atuadores que estão sendo usados em outros equipamentos, pois só pode haver um atuador vinculo a um equipamento
        /// </summary>
        /// <param name="hardwares">Lista de Todos os hardwares de uma sala especifica</param>
        private List<HardwareDeSalaModel> RemoveHardwaresInUse(List<HardwareDeSalaModel> hardwares)
        {
            var equipamentos = _context.Equipamentos.Where(e => e.IdHardwareDeSala != null).Select(e => new EquipamentoModel
            {
                Id = e.Id,
                Descricao = e.Descricao,
                Marca = e.Marca,
                TipoEquipamento = e.TipoEquipamento,
                Modelo = e.Modelo,
                Sala = e.IdSala,
                HardwareDeSala = (uint)e.IdHardwareDeSala
            }).ToList();

            if (equipamentos == null)
                return new List<HardwareDeSalaModel>();

            var hardwaresNotInUse = hardwares.Where(h => equipamentos.All(e => e.HardwareDeSala != h.Id)).Select(hd => new HardwareDeSalaModel
            {
                Id = hd.Id,
                MAC = hd.MAC,
                Ip = hd.Ip,
                SalaId = hd.SalaId,
                TipoHardwareId = hd.SalaId,
                Token = hd.Token
            }).ToList();

            return hardwaresNotInUse;
        }

        public List<HardwareDeSalaModel> GetAtuadorNotUsed()
        {
            var hardwares = GetAllAtuador();
            var hardwaresNotInUse = RemoveHardwaresInUse(hardwares);
            return hardwaresNotInUse;
        }

        public List<HardwareDeSalaModel> GetAllAtuador()
            => _context.Hardwaredesalas.Where(h => h.IdTipoHardware == TipoHardwareModel.CONTROLADOR_DE_DISPOSITIVO).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.IdSala, TipoHardwareId = h.IdTipoHardware, Ip = h.Ip, Token = h.Token }).ToList();

        public bool Update(HardwareDeSalaModel entity)
        {
            try
            {
                var hardware = GetByMAC(entity.MAC);

                if (hardware != null && hardware.Id != entity.Id)
                    throw new ServiceException("Já existe um dispositivo com esse endereço MAC");

                if (entity.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                {
                    hardware = GetByIp(entity.Ip);
                    if (hardware != null && hardware.Id != entity.Id)
                        throw new ServiceException("Já existe um dispositivo com esse endereço IP");
                }

                var x = _context.Hardwaredesalas.Where(tu => tu.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1;
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
