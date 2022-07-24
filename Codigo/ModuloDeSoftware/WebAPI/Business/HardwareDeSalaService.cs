using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Service
{
    public class HardwareDeSalaService : IHardwareDeSalaService
    {
        private readonly SalasUfsDbContext _context;
        public HardwareDeSalaService(SalasUfsDbContext context)
        {
            _context = context;
        }

        public List<HardwareDeSalaModel> GetAll() => _context.Hardwaredesala.Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Token = h.Token }).ToList();
        public List<HardwareDeSalaModel> GetAllHardwaresSalaByUsuarioOrganizacao(int idUsuario)
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

        public HardwareDeSalaModel GetById(int id) => _context.Hardwaredesala.Where(h => h.Id == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

        public HardwareDeSalaModel GetByIdAndType(int id, int tipo) => _context.Hardwaredesala.Where(h => h.Id == id && h.TipoHardware == tipo).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token, Registrado = Convert.ToBoolean(h.Registrado) }).FirstOrDefault();


        public List<HardwareDeSalaModel> GetByIdSala(int id)
        {
            var hardware = _context.Hardwaredesala
                                  .Where(hs => hs.Sala == id)
                                    .Select(h => 
                                        new HardwareDeSalaModel 
                                        { 
                                            Id = h.Id, 
                                            MAC = h.Mac, 
                                            SalaId = h.Sala, 
                                            TipoHardwareId = h.TipoHardware, 
                                            Ip = h.Ip, 
                                            Uuid = h.Uuid, 
                                            Token = h.Token,
                                        }
                                      ).ToList();


            return hardware; 
        }

        public List<HardwareDeSalaModel> GetSensorsAndActuactorsByIdSala(int id)
        {
            var hardwares = _context.Hardwaredesala
                                  .Join(_context.Equipamento,
                                     hard => hard.Id,
                                     equip => equip.HardwareDeSala,
                                     (hard, equip) => new { Hadware = hard, Equipamento = equip })
                                  .Where(hs => hs.Hadware.Sala == id && hs.Hadware.TipoHardware  == (int)HardwareDeSalaModel.TIPO.MODULO_ATUADOR)
                                    .Select(h =>
                                        new HardwareDeSalaModel
                                        {
                                            Id = h.Hadware.Id,
                                            MAC = h.Hadware.Mac,
                                            SalaId = h.Hadware.Sala,
                                            TipoHardwareId = h.Hadware.TipoHardware,
                                            Ip = h.Hadware.Ip,
                                            Uuid = h.Hadware.Uuid,
                                            Token = h.Hadware.Token,
                                            TipoEquipamento = h.Equipamento.TipoEquipamento.Equals(EquipamentoModel.TIPO_CONDICIONADOR) ? 1 : 0
                                        }
                                      ).Union(
                                         _context.Hardwaredesala
                                        .Where(hs => hs.Sala == id && hs.TipoHardware == (int)HardwareDeSalaModel.TIPO.MODULO_SENSOR)
                                          .Select(h =>
                                              new HardwareDeSalaModel
                                              {
                                                  Id = h.Id,
                                                  MAC = h.Mac,
                                                  SalaId = h.Sala,
                                                  TipoHardwareId = h.TipoHardware,
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
            var hardware = _context.Hardwaredesala.Where(h => h.Mac.ToLower().Equals(mac.ToLower())).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetByUuid(string uuid)
        {
            var hardware = _context.Hardwaredesala.Where(h => h.Uuid.Equals(uuid)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetHardwaresByIdSala(int idSala)
        {
            var hardware = _context
                            .Hardwaredesala
                            .Where(h => h.Uuid.Equals(idSala)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public HardwareDeSalaModel GetByMAC(string mac, int idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesala.Where(h => h.Mac.ToUpper().Equals(mac.ToUpper())).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

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

        public HardwareDeSalaModel GetByIp(string ip, int idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesala.Where(h => h.Ip == ip).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

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

            var hardware = _context.Hardwaredesala.Where(h => h.Ip == ip).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).FirstOrDefault();

            return hardware != null ? hardware : null;
        }

        public bool Insert(HardwareDeSalaModel entity, int idUsuario)
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
                var x = _context.Hardwaredesala.Where(tu => tu.Id == id).FirstOrDefault();
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

        public bool Update(HardwareDeSalaModel entity, int idUsuario)
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

                var x = _context.Hardwaredesala.Where(tu => tu.Id == entity.Id).FirstOrDefault();
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

        private static Hardwaredesala SetEntity(HardwareDeSalaModel model, Hardwaredesala entity)
        {
            entity.Id = model.Id;
            entity.Mac = model.MAC;
            entity.TipoHardware = model.TipoHardwareId;            
            entity.Registrado = Convert.ToByte(model.Registrado);
            entity.Sala = model.SalaId;
            entity.Ip = model.Ip;
            entity.Uuid = model.Uuid;
            entity.Token = model.Token;
            return entity;
        }

        public List<HardwareDeSalaModel> GetByIdSalaAndTipoHardware(int id, int tipo)
        => _context.Hardwaredesala.Where(h => h.Sala == id && h.TipoHardware == tipo).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Uuid = h.Uuid, Token = h.Token }).ToList();


        // TIPO 1 MODULO ATUADOR
        public List<HardwareDeSalaModel> GetAtuadorByIdSala(int id)
            => _context.Hardwaredesala.Where(h => h.Sala == id && h.TipoHardware == 1).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Token = h.Token }).ToList();

        /// <summary>
        /// Remove da lista os atuadores que estão sendo usados em outros equipamentos, pois só pode haver um atuador vinculo a um equipamento
        /// </summary>
        /// <param name="hardwares">Lista de Todos os hardwares de uma sala especifica</param>
        private List<HardwareDeSalaModel> RemoveHardwaresInUse(List<HardwareDeSalaModel> hardwares)
        {
            var equipamentos = _context.Equipamento.Where(e => e.HardwareDeSala != null).Select(e => new EquipamentoModel
            {
                Id = e.Id,
                Descricao = e.Descricao,
                Marca = e.Marca,
                TipoEquipamento = e.TipoEquipamento,
                Modelo = e.Modelo,
                Sala = e.Sala,
                HardwareDeSala = (int)e.HardwareDeSala
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
            => _context.Hardwaredesala.Where(h => h.TipoHardware == 1).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware, Ip = h.Ip, Token = h.Token }).ToList();

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

                var x = _context.Hardwaredesala.Where(tu => tu.Id == entity.Id).FirstOrDefault();
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
