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
        private readonly STR_DBContext _context;
        public HardwareDeSalaService(STR_DBContext context)
        {
            _context = context;
        }
        public List<HardwareDeSalaModel> GetAll() => _context.Hardwaredesala.Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).ToList();
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
                             TipoHardwareId = hr.TipoHardwareId
                         }).ToList();

            return query;
        }

        public HardwareDeSalaModel GetById(int id) => _context.Hardwaredesala.Where(h => h.Id == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

        public List<HardwareDeSalaModel> GetByIdSala(int id) => _context.Hardwaredesala.Where(h => h.Sala == id).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).ToList();
        public HardwareDeSalaModel GetByMAC(string mac, int idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);
            var _salaService = new SalaService(_context);

            var hardware = _context.Hardwaredesala.Where(h => h.Mac.Equals(mac)).Select(h => new HardwareDeSalaModel { Id = h.Id, MAC = h.Mac, SalaId = h.Sala, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

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


        public bool Insert(HardwareDeSalaModel entity, int idUsuario)
        {
            try
            {
                var hardware = GetByMAC(entity.MAC, idUsuario);

                if (hardware != null)
                    throw new ServiceException("Já existe um dispositivo com esse endereço MAC");

                _context.Add(SetEntity(entity, new Hardwaredesala()));
                return _context.SaveChanges() == 1 ? true : false;
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
                    return _context.SaveChanges() == 1 ? true : false;
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

                var x = _context.Hardwaredesala.Where(tu => tu.Id == entity.Id).FirstOrDefault();
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

        private static Hardwaredesala SetEntity(HardwareDeSalaModel model, Hardwaredesala entity)
        {
            entity.Id = model.Id;
            entity.Mac = Methods.CleanString(model.MAC);
            entity.TipoHardware = model.TipoHardwareId;
            entity.Sala = model.SalaId;

            return entity;
        }

    }
}
