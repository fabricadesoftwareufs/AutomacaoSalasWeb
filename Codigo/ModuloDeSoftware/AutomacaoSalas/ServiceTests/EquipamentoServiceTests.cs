using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.ViewModel;
using Model;
using Persistence;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Service.Tests
{
    [TestClass()]
    public class EquipamentoServiceTests
    {
        private SalasDBContext context;
        private EquipamentoService equipamentoService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase("automacaosalas")
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var equipamentos = new List<Equipamento>
            {
                new() { Id = 1, Modelo = "Modelo1", Marca = "Marca1", Descricao = "Descricao1", IdSala = 1, TipoEquipamento = "Tipo1", IdHardwareDeSala = 100 },
                new() { Id = 2, Modelo = "Modelo2", Marca = "Marca2", Descricao = "Descricao2", IdSala = 1, TipoEquipamento = "Tipo2", IdHardwareDeSala = 101 },
                new() { Id = 3, Modelo = "Modelo3", Marca = "Marca3", Descricao = "Descricao3", IdSala = 2, TipoEquipamento = "Tipo1", IdHardwareDeSala = 102 }
            };
            context.AddRange(equipamentos);
            context.SaveChanges();

            equipamentoService = new EquipamentoService(context);
        }


        [TestMethod()]
        public void GetByIdEquipamentoTest()
        {
            var equipamento = equipamentoService.GetByIdEquipamento(1);
            Assert.IsNotNull(equipamento);
            Assert.AreEqual(1, equipamento.Id);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest()
        {
            var equipamento = equipamentoService.GetByIdSalaAndTipoEquipamento(1, "Tipo1");
            Assert.IsNotNull(equipamento);
            Assert.AreEqual("Tipo1", equipamento.TipoEquipamento);
        }

        [TestMethod()]
        public void GetByIdSalaTest()
        {
            var equipamentos = equipamentoService.GetByIdSala(1);
            Assert.IsNotNull(equipamentos);
            Assert.AreEqual(2, equipamentos.Count);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var equipamentos = equipamentoService.GetAll();
            Assert.IsNotNull(equipamentos);
            Assert.AreEqual(3, equipamentos.Count);
        }

        [TestMethod()]
        public void InsertTest()
        {
            var novoEquipamento = new EquipamentoViewModel
            {
                EquipamentoModel = new EquipamentoModel { Id = 4, Modelo = "Modelo4", Marca = "Marca4", Descricao = "Descricao4", Sala = 2, TipoEquipamento = "Tipo3", HardwareDeSala = 103 },
                Codigos = new List<CodigoInfravermelhoViewModel>()
            };

            var resultado = equipamentoService.Insert(novoEquipamento);
            Assert.IsTrue(resultado);
            Assert.AreEqual(4, equipamentoService.GetAll().Count);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            var equipamentoExistente = context.Equipamentos.FirstOrDefault(e => e.Id == 1);
            Assert.IsNotNull(equipamentoExistente);

            context.Entry(equipamentoExistente).State = EntityState.Detached;

            var equipamentoAtualizado = new EquipamentoViewModel
            {
                EquipamentoModel = new EquipamentoModel
                {
                    Id = equipamentoExistente.Id,
                    Modelo = "NovoModelo1",
                    Marca = "NovaMarca1",
                    Descricao = "NovaDescricao1",
                    Sala = equipamentoExistente.IdSala,
                    TipoEquipamento = equipamentoExistente.TipoEquipamento,
                    HardwareDeSala = equipamentoExistente.IdHardwareDeSala
                },
                Codigos = new List<CodigoInfravermelhoViewModel>()
            };

            var resultado = equipamentoService.Update(equipamentoAtualizado);
            Assert.IsTrue(resultado);

            var equipamentoModificado = equipamentoService.GetByIdEquipamento(1);
            Assert.AreEqual("NovoModelo1", equipamentoModificado.Modelo);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var resultado = equipamentoService.Remove(1);
            Assert.IsTrue(resultado);
            Assert.IsNull(equipamentoService.GetByIdEquipamento(1));
        }

    }
}