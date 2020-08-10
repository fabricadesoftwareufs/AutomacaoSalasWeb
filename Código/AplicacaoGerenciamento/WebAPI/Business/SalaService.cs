using Model;
using Model.ViewModel;
using Persistence;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class SalaService : ISalaService
    {
        private readonly STR_DBContext _context;
        public SalaService(STR_DBContext context)
        {
            _context = context;
        }
        public List<SalaModel> GetAll() => _context.Sala.Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).ToList();

        public SalaModel GetById(int id) => _context.Sala.Where(s => s.Id == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).FirstOrDefault();

        public List<SalaModel> GetByIdBloco(int id) => _context.Sala.Where(s => s.Bloco == id).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).ToList();
        public SalaModel GetByTitulo(string titulo) => _context.Sala.Where(s => s.Titulo.ToUpper().Equals(titulo.ToUpper())).Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo, BlocoId = s.Bloco }).FirstOrDefault();


        public bool InsertSalaWithHardwares(SalaModel sala) 
        {
            var salaInserida = new SalaModel();
            try
            {
                salaInserida = Insert(new SalaModel { Id = sala.Id, Titulo = sala.Titulo, BlocoId = sala.BlocoId });
                if (salaInserida == null) throw new ServiceException("Houve um problema ao cadastrar sala, tente novamente em alguns minutos!");
            }
            catch (Exception e)
            {
                throw e;
            }

            if (sala.HardwaresSala.Count > 0)
            {
                var _hardwareDeSalaService = new HardwareDeSalaService(_context);
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in sala.HardwaresSala)
                            if (_hardwareDeSalaService.GetByMAC(item.MAC) != null)
                                throw new ServiceException("Já existe um dispositivos com o endereço MAC informado, corrija e tente novamente!");

                        foreach (var item in sala.HardwaresSala)
                            _hardwareDeSalaService.Insert(new HardwareDeSalaModel { MAC = item.MAC, SalaId = salaInserida.Id, TipoHardwareId = item.TipoHardwareId.Id });

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }

            return true;
        }

        public SalaModel Insert(SalaModel salaModel)
        {
            try
            {
                if (GetByTitulo(salaModel.Titulo) != null)
                    throw new ServiceException("Já existe um sala cadastrada com este nome!");

                var entity = new Sala();
                _context.Add(SetEntity(salaModel,entity));
                var save = _context.SaveChanges();

                if (save == 1)
                {
                    salaModel.Id = entity.Id; return salaModel;
                }
                else return null;
            }
            catch (Exception e) { throw e; }
        }

        public bool Remove(int id)
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

                    var x = _context.Sala.Where(s => s.Id == id).FirstOrDefault();
                    if (x != null)
                    {
                        _context.Remove(x);
                        return _context.SaveChanges() == 1 ? true : false;
                    }
                }
                else throw new ServiceException("Essa sala nao pode ser removida pois existem outros registros associados a ela!");
            }
            catch (Exception e) { throw e;}

            return false;
        }

        public bool Update(SalaModel entity)
        {
            try
            {
                var x = _context.Sala.Where(s => s.Id == entity.Id).FirstOrDefault();
                if (x != null)
                {
                    _context.Update(SetEntity(entity, x));
                    return _context.SaveChanges() == 1 ? true : false;
                }
            }
            catch (Exception e) { throw new ServiceException("Houve um problema ao atualizar registro, tente novamente em alguns minutos");}

            return false;
        }

        private static Sala SetEntity(SalaModel model, Sala entity)
        {
            entity.Id = model.Id;
            entity.Titulo = model.Titulo;
            entity.Bloco = model.BlocoId;

            return entity;
        }

        public List<SalaModel> GetSelectedList()
            => _context.Sala.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} - {1}", s.Id, s.Titulo) }).ToList();
    }
}
