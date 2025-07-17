using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Persistence;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Tests
{
    [TestClass()]
    public class TipoHardwareServiceTests
    {
        private SalasDBContext context;
        private TipoHardwareService tipoHardwareService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase("automacaosalas");
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Adicionando dados de exemplo com novos termos
            var tiposHardware = new List<Tipohardware>
            {
                new() { Id = 1, Descricao = "Controlador de sala" },
                new() { Id = 2, Descricao = "Modulo de sensoriamento" },
                new() { Id = 3, Descricao = "Modulo de dispositivo" }
            };
            context.AddRange(tiposHardware);

            context.SaveChanges();

            // Inicializando o serviço
            tipoHardwareService = new TipoHardwareService(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaTipos = tipoHardwareService.GetAll();

            // Assert
            Assert.IsNotNull(listaTipos);
            Assert.AreEqual<int>(3, listaTipos.Count);
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "Controlador de sala"));
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "Modulo de sensoriamento"));
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "Modulo de dispositivo"));
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            // Act
            var resultado = tipoHardwareService.GetById(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual<uint>(1, resultado.Id);
            Assert.AreEqual<string>("Controlador de sala", resultado.Descricao);
        }

        [TestMethod()]
        public void GetByIdOrganizacaoTest()
        {
            // Arrange
            uint organizacaoId = 1;
            context.Tipohardwares.RemoveRange(context.Tipohardwares);
            context.SaveChanges();

            var tiposHardware = new List<Tipohardware>
            {
                new() { Id = 1, Descricao = "Controlador de sala", IdOrganizacao = 1 },
                new() { Id = 2, Descricao = "Modulo de sensoriamento", IdOrganizacao = 1 },
                new() { Id = 3, Descricao = "Modulo de dispositivo", IdOrganizacao = 2 }
            };
            context.AddRange(tiposHardware);
            context.SaveChanges();

            // Act
            var resultado = tipoHardwareService.GetByIdOrganizacao(organizacaoId);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual<int>(2, resultado.Count);
            Assert.IsTrue(resultado.Any(t => t.Descricao == "Controlador de sala"));
            Assert.IsTrue(resultado.Any(t => t.Descricao == "Modulo de sensoriamento"));
            Assert.IsFalse(resultado.Any(t => t.Descricao == "Modulo de dispositivo"));
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novoTipoHardware = new TipoHardwareModel
            {
                Id = 4,
                Descricao = "Monitor de temperatura",
                IdOrganizacao = 1 
            };

            // Act
            var resultado = tipoHardwareService.Insert(novoTipoHardware);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoHardwareService.GetAll();
            Assert.AreEqual<int>(4, tipos.Count);
            Assert.IsTrue(tipos.Any(t => t.Descricao == "Monitor de temperatura"));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange
            var novoTipoHardware = new TipoHardwareModel
            {
                Id = 4,
                Descricao = "Sensor de presença",
                IdOrganizacao = 1 
            };
            tipoHardwareService.Insert(novoTipoHardware);

            // Act
            var resultado = tipoHardwareService.Remove(4);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoHardwareService.GetAll();
            Assert.AreEqual<int>(3, tipos.Count);
            Assert.IsFalse(tipos.Any(t => t.Descricao == "Sensor de presença"));
        }

        [TestMethod()]
        public void RemoveTest_NonExistentId()
        {
            // Act
            var resultado = tipoHardwareService.Remove(999);

            // Assert
            Assert.IsFalse(resultado);
            var tipos = tipoHardwareService.GetAll();
            Assert.AreEqual<int>(3, tipos.Count);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            var tipoHardwareExistente = new TipoHardwareModel
            {
                Id = 2,
                Descricao = "Modulo de sensoriamento avançado",
                IdOrganizacao = 1 // Adicionado IdOrganizacao
            };

            // Act
            var resultado = tipoHardwareService.Update(tipoHardwareExistente);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoHardwareService.GetAll();
            Assert.IsTrue(tipos.Any(t => t.Descricao == "Modulo de sensoriamento avançado"));
        }

        [TestMethod()]
        public void UpdateTest_NonExistentId()
        {
            // Arrange
            var tipoHardwareInexistente = new TipoHardwareModel
            {
                Id = 999,
                Descricao = "Controlador de iluminação"
            };

            // Act
            var resultado = tipoHardwareService.Update(tipoHardwareInexistente);

            // Assert
            Assert.IsFalse(resultado);
        }
    }
}
