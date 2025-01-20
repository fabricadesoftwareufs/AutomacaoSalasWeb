using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Persistence;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class BlocoServiceTests
    {
        private SalasDBContext context;
        private IBlocoService blocoService;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase("automacaosalas");
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var blocos = new List<Bloco>
            {
                new() { Id = 1, IdOrganizacao = 1, Titulo = "Bloco A" },
                new() { Id = 2, IdOrganizacao = 1, Titulo = "Bloco B" },
                new() { Id = 3, IdOrganizacao = 1, Titulo = "Bloco C" }
            };

            context.AddRange(blocos);
            var organizacao = new List<Organizacao>
            {
                new() { Id = 1, Cnpj = "56287944000146", RazaoSocial = "UFS" }
            };
            context.AddRange(organizacao);

            var usuarioOrganizacao = new List<Usuarioorganizacao>
            {
                new() {IdUsuario = 1, IdOrganizacao = 1,IdTipoUsuario=1 }
            };
            context.AddRange(usuarioOrganizacao);

            context.SaveChanges();
            blocoService = new BlocoService(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaSalas = blocoService.GetAll();
            // Assert
            Assert.IsInstanceOfType(listaSalas, typeof(List<BlocoModel>));
            Assert.IsNotNull(listaSalas);
            Assert.AreEqual(3, listaSalas.Count());
            Assert.AreEqual((uint)1, listaSalas.First().Id);
            Assert.AreEqual("Bloco A", listaSalas.First().Titulo);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Act
            var resultado = blocoService.GetById(1);
            // Assert
            Assert.IsNotNull(resultado, "O método GetById retornou null");
            Assert.AreEqual((uint)1, resultado.Id);
            Assert.AreEqual("Bloco A", resultado.Titulo);
            Assert.AreEqual((uint)1, resultado.OrganizacaoId);
            Assert.AreEqual("UFS", resultado.NomeOrganizacao);
        }

        [TestMethod()]
        public void GetByIdOrganizacaoTest()
        {
            // Act
            var resultado = blocoService.GetByIdOrganizacao(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Count);
            Assert.IsTrue(resultado.Any(b => b.Titulo == "Bloco A"));
            Assert.IsTrue(resultado.All(b => b.OrganizacaoId == 1));
        }

        [TestMethod()]
        public void GetByTituloTest()
        {
            // Act
            var blocoExistente = blocoService.GetByTitulo("Bloco A", 1);
            var blocoInexistente = blocoService.GetByTitulo("Bloco X", 1);

            // Assert
            Assert.IsNotNull(blocoExistente);
            Assert.AreEqual("Bloco A", blocoExistente.Titulo);
            Assert.AreEqual((uint)1, blocoExistente.OrganizacaoId);
            Assert.IsNull(blocoInexistente);
        }

        [TestMethod()]
        public void InsertBlocoWithHardwareTest()
        {
            // Arrange
            var novoBloco = new BlocoModel { OrganizacaoId = 1, Titulo = "Bloco D" };

            // Act
            var resultado = blocoService.InsertBlocoWithHardware(novoBloco, 1);

            // Assert
            Assert.IsTrue(resultado);
            var blocoInserido = blocoService.GetByTitulo("Bloco D", 1);
            Assert.IsNotNull(blocoInserido);
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novoBloco = new BlocoModel { OrganizacaoId = 1, Titulo = "Bloco E" };

            // Act
            var resultado = blocoService.Insert(novoBloco);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Bloco E", resultado.Titulo);
            Assert.AreEqual((uint)1, resultado.OrganizacaoId);
            Assert.IsTrue(resultado.Id > 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void InsertTest_DuplicateTitle()
        {
            // Arrange
            var novoBloco = new BlocoModel { OrganizacaoId = 1, Titulo = "Bloco A" };

            // Act
            blocoService.Insert(novoBloco);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange
            var novoBloco = new BlocoModel { OrganizacaoId = 1, Titulo = "Bloco Temp" };
            var blocoInserido = blocoService.Insert(novoBloco);

            // Act
            var resultado = blocoService.Remove(blocoInserido.Id);

            // Assert
            Assert.IsTrue(resultado);
            Assert.IsNull(blocoService.GetById(blocoInserido.Id));
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            var blocoExistente = blocoService.GetById(1);
            blocoExistente.Titulo = "Bloco A Atualizado";

            // Act
            var resultado = blocoService.Update(blocoExistente);

            // Assert
            Assert.IsTrue(resultado);
            var blocoAtualizado = blocoService.GetById(1);
            Assert.AreEqual("Bloco A Atualizado", blocoAtualizado.Titulo);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void UpdateTest_DuplicateTitle()
        {
            // Arrange
            var blocoExistente = blocoService.GetById(1);
            blocoExistente.Titulo = "Bloco B"; 

            // Act
            blocoService.Update(blocoExistente);
        }

        [TestMethod()]
        public void GetAllByIdUsuarioOrganizacaoTest()
        {
            // Act
            var resultado = blocoService.GetAllByIdUsuarioOrganizacao(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(3, resultado.Count);
            Assert.IsTrue(resultado.All(b => b.OrganizacaoId == 1));
        }
    }
}