using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.ViewModel;
using Model;
using Persistence;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Service.Tests
{
    [TestClass()]
    public class EquipamentoServiceTests
    {
        private SalasDBContext? context;
        private EquipamentoService? equipamentoService;
        private UsuarioModel? usuarioModel;

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

            // Primeiro, adicionar as marcas
            var marcaEquipamentos = new List<Marcaequipamento>
            {
                new() { Id = 1, Nome = "Marca1" },
                new() { Id = 2, Nome = "Marca2" }
            };
            context.AddRange(marcaEquipamentos);
            context.SaveChanges();

            // Depois, adicionar os modelos
            var modeloEquipamentos = new List<Modeloequipamento>
            {
                new() { Id = 1, Nome = "Modelo1", IdMarcaEquipamento = 1 },
                new() { Id = 2, Nome = "Modelo2", IdMarcaEquipamento = 1 },
                new() { Id = 3, Nome = "Modelo3", IdMarcaEquipamento = 2 }
            };
            context.AddRange(modeloEquipamentos);
            context.SaveChanges();

            // Por último, adicionar os equipamentos com Status obrigatório
            var equipamentos = new List<Equipamento>
            {
                new() {
                    Id = 1,
                    Descricao = "Descricao1",
                    IdSala = 1,
                    TipoEquipamento = "Tipo1",
                    IdHardwareDeSala = 100,
                    IdModeloEquipamento = 1,
                    Status = "A" // Adicionando Status obrigatório
                },
                new() {
                    Id = 2,
                    Descricao = "Descricao2",
                    IdSala = 1,
                    TipoEquipamento = "Tipo2",
                    IdHardwareDeSala = 101,
                    IdModeloEquipamento = 2,
                    Status = "A"
                },
                new() {
                    Id = 3,
                    Descricao = "Descricao3",
                    IdSala = 2,
                    TipoEquipamento = "Tipo1",
                    IdHardwareDeSala = 102,
                    IdModeloEquipamento = 3,
                    Status = "A"
                }
            };
            context.AddRange(equipamentos);
            context.SaveChanges();

            equipamentoService = new EquipamentoService(context);
            usuarioModel = new UsuarioModel
            {
                Id = 1,
                Cpf = "12345678901",
                Nome = "Usuario1",
                DataNascimento = DateTime.Now,
                Senha = "Senha1",
                TipoUsuarioId = 1,
                IdOrganizacao = 1,
                IdTipoUsuario = 1
            };
        }

        [TestMethod()]
        public void GetByIdEquipamentoTest()
        {
            // Act
            var equipamento = equipamentoService!.GetByIdEquipamento(1);

            // Assert
            Assert.IsNotNull(equipamento);
            Assert.AreEqual(1, equipamento.Id);
            Assert.AreEqual("Descricao1", equipamento.Descricao);
            Assert.AreEqual("Modelo1", equipamento.Modelo);
            Assert.AreEqual("Marca1", equipamento.Marca);
            Assert.AreEqual<uint>(1, equipamento.Sala);
            Assert.AreEqual("Tipo1", equipamento.TipoEquipamento);
            Assert.AreEqual((uint)100, equipamento.HardwareDeSala);
        }

        [TestMethod()]
        public void GetByIdEquipamentoTest_NotFound()
        {
            // Act
            var equipamento = equipamentoService!.GetByIdEquipamento(999);

            // Assert
            Assert.IsNull(equipamento);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest()
        {
            // Act
            var equipamento = equipamentoService!.GetByIdSalaAndTipoEquipamento(1, "Tipo1");

            // Assert
            Assert.IsNotNull(equipamento);
            Assert.AreEqual("Tipo1", equipamento.TipoEquipamento);
            Assert.AreEqual<uint>(1, equipamento.Sala);
            Assert.AreEqual("Modelo1", equipamento.Modelo);
            Assert.AreEqual("Marca1", equipamento.Marca);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest_CaseInsensitive()
        {
            // Act
            var equipamento = equipamentoService!.GetByIdSalaAndTipoEquipamento(1, "tipo1");

            // Assert
            Assert.IsNotNull(equipamento);
            Assert.AreEqual("Tipo1", equipamento.TipoEquipamento);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest_NotFound()
        {
            // Act
            var equipamento = equipamentoService!.GetByIdSalaAndTipoEquipamento(1, "TipoInexistente");

            // Assert
            Assert.IsNull(equipamento);
        }

        [TestMethod()]
        public void GetByIdSalaTest()
        {
            // Act
            var equipamentos = equipamentoService!.GetByIdSala(1);

            // Assert
            Assert.IsNotNull(equipamentos);
            Assert.AreEqual(2, equipamentos.Count);
            Assert.IsTrue(equipamentos.All(e => e.Sala == 1));
        }

        [TestMethod()]
        public void GetByIdSalaTest_EmptyResult()
        {
            // Act
            var equipamentos = equipamentoService!.GetByIdSala(999);

            // Assert
            Assert.IsNotNull(equipamentos);
            Assert.AreEqual(0, equipamentos.Count);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var equipamentos = equipamentoService!.GetAll();

            // Assert
            Assert.IsNotNull(equipamentos);
            Assert.AreEqual(3, equipamentos.Count);

            // Verificar se todos os equipamentos têm dados preenchidos
            foreach (var eq in equipamentos)
            {
                Assert.IsTrue(eq.Id > 0);
                Assert.IsNotNull(eq.Descricao);
                Assert.IsNotNull(eq.Modelo);
                Assert.IsNotNull(eq.Marca);
                Assert.IsNotNull(eq.TipoEquipamento);
            }
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novoEquipamento = new EquipamentoViewModel
            {
                EquipamentoModel = new EquipamentoModel
                {
                    Id = 0, // Para inserção, ID deve ser 0
                    Modelo = "Modelo1", // Usar modelo existente
                    Marca = "Marca1",   // Usar marca existente
                    Descricao = "Descricao4",
                    Sala = 2,
                    TipoEquipamento = "Tipo3",
                    HardwareDeSala = 103,
                    IdModeloEquipamento = 1 // Usar modelo existente
                },
                Codigos = new List<CodigoInfravermelhoViewModel>()
            };

            // Act
            var resultado = equipamentoService!.Insert(novoEquipamento, usuarioModel!.Id);

            // Assert
            Assert.IsTrue(resultado);
            Assert.AreEqual(4, equipamentoService.GetAll().Count);

            // Verificar se o equipamento foi inserido corretamente
            var equipamentos = equipamentoService.GetAll();
            var equipamentoInserido = equipamentos.FirstOrDefault(e => e.Descricao == "Descricao4");
            Assert.IsNotNull(equipamentoInserido);
            Assert.AreEqual("Tipo3", equipamentoInserido.TipoEquipamento);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange - Primeiro limpar o contexto para evitar conflitos de rastreamento
            context!.ChangeTracker.Clear();

            var equipamentoAtualizado = new EquipamentoViewModel
            {
                EquipamentoModel = new EquipamentoModel
                {
                    Id = 1,
                    Modelo = "Modelo2", // Mudando para modelo existente
                    Marca = "Marca1",
                    Descricao = "NovaDescricao1",
                    Sala = 1, // Usar valor conhecido
                    TipoEquipamento = "NovoTipo1",
                    HardwareDeSala = 100, // Usar valor conhecido
                    IdModeloEquipamento = 2 // Mudando para modelo 2
                },
                Codigos = new List<CodigoInfravermelhoViewModel>()
            };

            // Act
            var resultado = equipamentoService!.Update(equipamentoAtualizado);

            // Assert
            Assert.IsTrue(resultado);

            // Limpar o contexto novamente antes de verificar
            context.ChangeTracker.Clear();

            // Verificar se o equipamento foi atualizado
            var equipamentoModificado = equipamentoService.GetByIdEquipamento(1);
            Assert.IsNotNull(equipamentoModificado);
            Assert.AreEqual("NovaDescricao1", equipamentoModificado.Descricao);
            Assert.AreEqual("NovoTipo1", equipamentoModificado.TipoEquipamento);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange
            var equipamentoAntes = equipamentoService!.GetByIdEquipamento(1);
            Assert.IsNotNull(equipamentoAntes);

            // Act
            var resultado = equipamentoService.Remove(1);

            // Assert
            Assert.IsTrue(resultado);

            // Verificar se o equipamento foi removido
            var equipamentoDepois = equipamentoService.GetByIdEquipamento(1);
            Assert.IsNull(equipamentoDepois);

            // Verificar se o total de equipamentos diminuiu
            Assert.AreEqual(2, equipamentoService.GetAll().Count);
        }

        [TestMethod()]
        public void RemoveTest_EquipamentoInexistente()
        {
            // Act & Assert
            var exception = Assert.ThrowsException<ServiceException>(() =>
                equipamentoService!.Remove(999));

            Assert.AreEqual("Equipamento não encontrado.", exception.Message);
        }

        [TestCleanup]
        public void Cleanup()
        {
            context?.Dispose();
        }
    }
}