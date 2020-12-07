using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class EquipamentoService : IEquipamentoService
    {
        private readonly STR_DBContext _context;

        public EquipamentoService(STR_DBContext context)
        {
            _context = context;
        }

        public EquipamentoModel GetByIdSalaAndTipoEquipamento(int idSala, string tipo)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala && eq.TipoEquipamento.ToUpper().Equals(tipo.ToUpper()))
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento
                   }).FirstOrDefault();


        public List<EquipamentoModel> GetByIdSala(int idSala)
       => _context.Equipamento
                   .Where(eq => eq.Sala == idSala)
                   .Select(eq => new EquipamentoModel
                   {
                       Id = eq.Id,
                       Modelo = eq.Modelo,
                       Marca = eq.Marca,
                       Descricao = eq.Descricao,
                       Sala = eq.Sala,
                       TipoEquipamento = eq.TipoEquipamento
                   }).ToList();
    }
}
