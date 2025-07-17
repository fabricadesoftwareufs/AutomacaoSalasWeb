using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Persistence;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore.InMemory;
namespace Service.Tests
{
    [TestClass()]
    public class OrganizacaoServiceTests
    {
        private SalasDBContext context;
        private IOrganizacaoService organizacaoService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase("automacaosalas")
                   .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var organizacoes = new List<Organizacao>
            {
                new() { Id = 1, Cnpj = "56287944000146", RazaoSocial = "UFS" },
                new() { Id = 2, Cnpj = "12345678000190", RazaoSocial = "UEFS" }
            };
            context.AddRange(organizacoes);

            var blocos = new List<Bloco>
            {
                new() { Id = 1, IdOrganizacao = 2, Titulo = "Bloco A" }
            };
            context.AddRange(blocos);

            var usuarioOrganizacao = new List<Usuarioorganizacao>
            {
                new() { IdUsuario = 1, IdOrganizacao = 2, IdTipoUsuario = 1 }
            };
            context.AddRange(usuarioOrganizacao);
           
            var tiposHardware = new List<Tipohardware>
            {
                new() { Id = 1, Descricao = "CONTROLADOR DE SALA", IdOrganizacao = 1 },
                new() { Id = 2, Descricao = "MODULO DE SENSORIAMENTO", IdOrganizacao = 1 },
                new() { Id = 3, Descricao = "MODULO DE DISPOSITIVO", IdOrganizacao = 1 },
                new() { Id = 4, Descricao = "CONTROLADOR DE SALA", IdOrganizacao = 2 },
                new() { Id = 5, Descricao = "MODULO DE SENSORIAMENTO", IdOrganizacao = 2 },
                new() { Id = 6, Descricao = "MODULO DE DISPOSITIVO", IdOrganizacao = 2 }
            };
            context.AddRange(tiposHardware);

            context.SaveChanges();
            organizacaoService = new OrganizacaoService(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaOrganizacoes = organizacaoService.GetAll();

            // Assert
            Assert.IsInstanceOfType(listaOrganizacoes, typeof(List<OrganizacaoModel>));
            Assert.IsNotNull(listaOrganizacoes);
            Assert.AreEqual(2, listaOrganizacoes.Count);
            Assert.IsTrue(listaOrganizacoes.Any(o => o.Cnpj == "56287944000146"));
        }

        [TestMethod()]
        public void GetInListTest()
        {
            // Arrange
            var ids = new List<uint> { 1, 2 };

            // Act
            var listaOrganizacoes = organizacaoService.GetInList(ids);

            // Assert
            Assert.IsNotNull(listaOrganizacoes);
            Assert.AreEqual(2, listaOrganizacoes.Count);
            Assert.IsTrue(listaOrganizacoes.All(o => ids.Contains(o.Id)));
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            // Act
            var resultado = organizacaoService.GetById(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual((uint)1, resultado.Id);
            Assert.AreEqual("56287944000146", resultado.Cnpj);
            Assert.AreEqual("UFS", resultado.RazaoSocial);
        }

        [TestMethod()]
        public void GetByCnpjTest()
        {
            // Act
            var resultado = organizacaoService.GetByCnpj("56287944000146");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual((uint)1, resultado.Id);
            Assert.AreEqual("56287944000146", resultado.Cnpj);
            Assert.AreEqual("UFS", resultado.RazaoSocial);
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novaOrganizacao = new OrganizacaoModel
            {
                Cnpj = "65432198000145",
                RazaoSocial = "Nova Universidade"
            };

            // Act
            var resultado = organizacaoService.Insert(novaOrganizacao);

            // Assert
            Assert.IsTrue(resultado);
            var organizacaoInserida = organizacaoService.GetByCnpj("65432198000145");
            Assert.IsNotNull(organizacaoInserida);
            Assert.AreEqual("Nova Universidade", organizacaoInserida.RazaoSocial);
            Assert.AreEqual("65432198000145", organizacaoInserida.Cnpj);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void InsertTest_DuplicateCnpj()
        {
            // Arrange
            var organizacaoDuplicada = new OrganizacaoModel
            {
                Cnpj = "56287944000146", // CNPJ já existente no setup
                RazaoSocial = "Organização Duplicada"
            };

            // Act
            organizacaoService.Insert(organizacaoDuplicada);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange 
            var novaOrganizacao = new OrganizacaoModel
            {
                Cnpj = "98765432000156",
                RazaoSocial = "Organização para Remoção"
            };

            organizacaoService.Insert(novaOrganizacao);
            var organizacaoInserida = organizacaoService.GetByCnpj("98765432000156");

            Assert.IsNotNull(organizacaoInserida);

            // Act - Remover a organização
            var resultado = organizacaoService.Remove(organizacaoInserida.Id);

            // Assert
            Assert.IsTrue(resultado);
            Assert.IsNull(organizacaoService.GetById(organizacaoInserida.Id));
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void RemoveTest_WithAssociatedData()
        {
            // Act - Tentar remover organização com dependências
            organizacaoService.Remove(2);
        }


        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            var organizacaoExistente = organizacaoService.GetById(1);
            organizacaoExistente.RazaoSocial = "UFS Atualizada";

            // Act
            var resultado = organizacaoService.Update(organizacaoExistente);

            // Assert
            Assert.IsTrue(resultado);
            var organizacaoAtualizada = organizacaoService.GetById(1);
            Assert.AreEqual("UFS Atualizada", organizacaoAtualizada.RazaoSocial);
        }

        [TestMethod()]
        [ExpectedException(typeof(ServiceException))]
        public void UpdateTest_DuplicateCnpj()
        {
            // Arrange
            var organizacaoExistente = organizacaoService.GetById(1);
            organizacaoExistente.Cnpj = "12345678000190"; 

            // Act
            organizacaoService.Update(organizacaoExistente);
        }

        [TestMethod()]
        public void GetByIdUsuarioTest()
        {
            // Act
            var resultado = organizacaoService.GetByIdUsuario(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual((uint)2, resultado.First().Id);
            Assert.AreEqual("12345678000190", resultado.First().Cnpj);
        }
    }
}