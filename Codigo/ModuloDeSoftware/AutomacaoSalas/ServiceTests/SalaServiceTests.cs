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
    public class SalaServiceTests
    {
        private SalasDBContext context;
        private ISalaService salaService;

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
            var salas = new List<Sala>
                {
                    new() { Id = 1, Titulo = "Sala 001", IdBloco = 1 },
                    new() { Id = 2, Titulo = "Sala 002", IdBloco = 2},
                    new() { Id = 3, Titulo = "Sala 003", IdBloco = 3},
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

        [TestMethod()]
        public void GetByIdTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetByIdBlocoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetByTituloTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertSalaWithHardwaresTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSelectedListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllByIdUsuarioOrganizacaoTest()
        {
            Assert.Fail();
        }
    }
}