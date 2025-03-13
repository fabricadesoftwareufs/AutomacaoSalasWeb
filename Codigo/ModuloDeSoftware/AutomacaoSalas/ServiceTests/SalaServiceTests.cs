using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.AuxModel;
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
    public class SalaServiceTests
    {
        private SalasDBContext context;
        private ISalaService salaService;
        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase("automacaosalas")
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            AdicionarSalas();

            salaService = new SalaService(context);
        }

        private void AdicionarSalas()
        {
            // Adiciona os blocos
            var blocos = new List<Bloco>
            {
                new() { Id = 1, Titulo = "Bloco A" },
                new() { Id = 2, Titulo = "Bloco B" },
                new() { Id = 3, Titulo = "Bloco C" }
            };
            context.AddRange(blocos);
            // Adiciona as salas
            var salas = new List<Sala>
            {
                new() { Id = 1, Titulo = "Sala 001", IdBloco = 1 },
                new() { Id = 2, Titulo = "Sala 002", IdBloco = 2 },
                new() { Id = 3, Titulo = "Sala 003", IdBloco = 3 }
            };
            context.AddRange(salas);

            context.SaveChanges();
            salaService = new SalaService(context);
        }


        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaSalas = salaService.GetAll();
            // Assert
            Assert.IsInstanceOfType(listaSalas, typeof(List<SalaModel>));
            Assert.IsNotNull(listaSalas);
            Assert.AreEqual(3, listaSalas.Count());
            Assert.AreEqual((uint)1, listaSalas.First().Id);
            Assert.AreEqual("Sala 001", listaSalas.First().Titulo);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Act
            var resultado = salaService.GetById(1);

            // Assert
            Assert.IsNotNull(resultado, "O método GetById retornou null");
            Assert.AreEqual((uint)1, resultado.Id);
            Assert.AreEqual("Sala 001", resultado.Titulo);
            Assert.AreEqual((uint)1, resultado.BlocoId);
            Assert.AreEqual("Bloco A", resultado.BlocoTitulo);
        }

        [TestMethod]
        public void GetByIdTest_NotFound()
        {
            // Act
            var resultado = salaService.GetById(999);

            // Assert
            Assert.IsNull(resultado, "O método deveria retornar null para um ID inexistente");
        }

        [TestMethod()]
        public void GetByIdBlocoTest()
        {
            // Act
            var salasDoBloco = salaService.GetByIdBloco(1);

            // Assert
            Assert.IsNotNull(salasDoBloco);
            Assert.IsInstanceOfType(salasDoBloco, typeof(List<SalaModel>));
            Assert.AreEqual(1, salasDoBloco.Count);
            Assert.AreEqual((uint)1, salasDoBloco.First().Id);
            Assert.AreEqual("Sala 001", salasDoBloco.First().Titulo);
            Assert.AreEqual((uint)1, salasDoBloco.First().BlocoId);
        }

        [TestMethod()]
        public void GetByIdBlocoTest_NotFound()
        {
            // Act
            var salasDoBloco = salaService.GetByIdBloco(999);

            // Assert
            Assert.IsNotNull(salasDoBloco);
            Assert.AreEqual(0, salasDoBloco.Count);
        }

        [TestMethod()]
        public void GetByTituloTest()
        {
            // Act
            var sala = salaService.GetByTitulo("Sala 001");

            // Assert
            Assert.IsNotNull(sala);
            Assert.IsInstanceOfType(sala, typeof(SalaModel));
            Assert.AreEqual((uint)1, sala.Id); 
            Assert.AreEqual("Sala 001", sala.Titulo); 
            Assert.AreEqual((uint)1, sala.BlocoId); 
        }

        [TestMethod()]
        public void GetByTituloTest_NotFound()
        {
            // Act
            var sala = salaService.GetByTitulo("Sala Inexistente");

            // Assert
            Assert.IsNull(sala); 
        }

        [TestMethod]
        public void InsertSalaWithHardwaresTest()
        {
            // Arrange
            var salaAux = new SalaAuxModel
            {
                Sala = new SalaModel { Titulo = "Sala Nova", BlocoId = 1 },
                HardwaresSala = new List<HardwareAuxModel>
                {
                new HardwareAuxModel
                {
                    MAC = "00:11:22:33:44:55",
                    Ip = "192.168.1.1",
                    TipoHardwareId = new TipoUsuarioModel
                    {
                        Id = (uint)TipoHardwareModel.CONTROLADOR_DE_SALA
                    }
                }
                }
            };
            uint idUsuario = 1;

            // Act
            var result = salaService.InsertSalaWithHardwaresOrSalasWithPontosdeAcesso(salaAux, idUsuario);

            // Assert
            Assert.IsTrue(result);
            var insertedSala = salaService.GetByTitulo("Sala Nova");
            Assert.IsNotNull(insertedSala, "A sala não foi inserida corretamente");
            Assert.AreEqual("Sala Nova", insertedSala.Titulo);
            Assert.AreEqual((uint)1, insertedSala.BlocoId);

            // Verificar se o hardware foi inserido
            var hardwareSalaService = new HardwareDeSalaService(context);
            var hardwares = hardwareSalaService.GetByIdSala(insertedSala.Id);
            Assert.IsTrue(hardwares.Count > 0, "O hardware não foi inserido");
            Assert.AreEqual("00:11:22:33:44:55", hardwares[0].MAC);
            Assert.AreEqual("192.168.1.1", hardwares[0].Ip);
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novaSala = new SalaModel
            {
                Titulo = "Sala Test",
                BlocoId = 1
            };

            // Act
            var result = salaService.Insert(novaSala);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SalaModel));
            Assert.AreEqual("Sala Test", result.Titulo);
            Assert.AreEqual((uint)1, result.BlocoId);
            Assert.IsTrue(result.Id > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void InsertTest_DuplicateTitulo()
        {
            // Arrange
            var novaSala = new SalaModel
            {
                Titulo = "Sala 001", 
                BlocoId = 1
            };

            // Act
            salaService.Insert(novaSala); 
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Act
            var result = salaService.Remove(1);

            // Assert
            Assert.IsTrue(result);
            var removedSala = salaService.GetById(1);
            Assert.IsNull(removedSala);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void RemoveTest_WithHardwares()
        {
            // Arrange
            var salaAux = new SalaAuxModel
            {
                Sala = new SalaModel { Titulo = "Sala Com Hardware", BlocoId = 1 },
                HardwaresSala = new List<HardwareAuxModel>
                {
                    new HardwareAuxModel
                    {
                        MAC = "AA:BB:CC:DD:EE:FF",
                        Ip = "192.168.1.2",
                        TipoHardwareId = new TipoUsuarioModel
                        {
                            Id = (uint)TipoHardwareModel.CONTROLADOR_DE_SALA
                        }
                    }
                }
            };
            uint idUsuario = 1;

            // Insere a sala com hardware
            salaService.InsertSalaWithHardwaresOrSalasWithPontosdeAcesso(salaAux, idUsuario);

            // Recupera a sala inserida
            var salaInserida = salaService.GetByTitulo("Sala Com Hardware");

            // Act
            // Tenta remover a sala com hardware associado
            salaService.Remove(salaInserida.Id);

            // Assert
            // A exceção esperada (ServiceException) deve ser lançada
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            var salaAtualizada = new SalaModel
            {
                Id = 1,
                Titulo = "Sala Atualizada",
                BlocoId = 2
            };

            // Act
            var result = salaService.Update(salaAtualizada);
            var updatedSala = salaService.GetById(1);

            // Assert
            Assert.IsTrue(result, "A atualização não foi concluída com sucesso.");
            Assert.IsNotNull(updatedSala, "A sala atualizada não foi encontrada.");
            Assert.AreEqual("Sala Atualizada", updatedSala.Titulo, "O título da sala não foi atualizado corretamente.");
            Assert.AreEqual((uint)2, updatedSala.BlocoId, "O ID do bloco não foi atualizado corretamente.");
        }
    }
}