using Model;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Service
{
    public class HardwareDeBlocoService : IHardwareDeBlocoService
    {
        private readonly STR_DBContext _context;
        public HardwareDeBlocoService(STR_DBContext context)
        {
            _context = context;
        }
        public List<HardwareDeBlocoModel> GetAll() => _context.Hardwaredebloco.Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).ToList();

        public HardwareDeBlocoModel GetById(int id) => _context.Hardwaredebloco.Where(h => h.Id == id).Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).FirstOrDefault();

        public List<HardwareDeBlocoModel> GetByIdBloco(int id) => _context.Hardwaredebloco.Where(h => h.Bloco == id).Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).ToList();
        public List<HardwareDeBlocoModel> GetAllHardwaresBlacoByUsuarioOrganizacao(int idUsuario)
        {
            var _blocoService = new BlocoService(_context);
            var hardwares = GetAll();

            var query = (from hr in hardwares
                         join sl in _blocoService.GetAllByIdUsuarioOrganizacao(idUsuario) on hr.BlocoId equals sl.Id
                         select new HardwareDeBlocoModel
                         {
                             Id = hr.Id,
                             MAC = hr.MAC,
                             BlocoId = hr.BlocoId,
                             TipoHardwareId = hr.TipoHardwareId,
                         }).ToList();

            return query;
        }

        public HardwareDeBlocoModel GetByMAC(string mac, int idUsuario)
        {
            var _usuarioOrganizacao = new UsuarioOrganizacaoService(_context);
            var _blocoService = new BlocoService(_context);

            var hardware = _context.Hardwaredebloco.Where(h => h.Mac.Equals(mac)).Select(h => new HardwareDeBlocoModel { Id = h.Id, MAC = h.Mac, BlocoId = h.Bloco, TipoHardwareId = h.TipoHardware }).FirstOrDefault();


            if (hardware != null)
            {
                var bloco = _blocoService.GetById(hardware.BlocoId);
                var orgs = _usuarioOrganizacao.GetByIdUsuario(idUsuario);

                foreach (var item in orgs)
                    if (bloco.OrganizacaoId == item.OrganizacaoId)
                        return hardware;
            }
            return null;
        }


        public bool Insert(HardwareDeBlocoModel entity, int idUsuario)
        {
            try
            {
                var hardware = GetByMAC(entity.MAC, idUsuario);
                if (hardware != null)
                    throw new ServiceException("Já existe outro hardware com esse endereço MAC!");

                _context.Add(SetEntity(entity, new Hardwaredebloco()));
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
                var x = _context.Hardwaredebloco.Where(tu => tu.Id == id).FirstOrDefault();
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

        public bool Update(HardwareDeBlocoModel entity, int idUsuario)
        {
            try
            {
                var hr = GetByMAC(entity.MAC, idUsuario);
                if (hr != null && hr.Id != entity.Id)
                    throw new ServiceException("Já existe outro hardware com esse endereço MAC!");

                var x = _context.Hardwaredebloco.Where(tu => tu.Id == entity.Id).FirstOrDefault();
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

        private static Hardwaredebloco SetEntity(HardwareDeBlocoModel model, Hardwaredebloco entity)
        {
            entity.Id = model.Id;
            entity.Mac = Methods.CleanString(model.MAC);
            entity.TipoHardware = model.TipoHardwareId;
            entity.Bloco = model.BlocoId;

            return entity;
        }
    }
}
