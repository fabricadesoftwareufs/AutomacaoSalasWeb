using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Persistence;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class TipoUsuarioServiceTests
    {
        private SalasDBContext context;
        private ITipoUsuarioService tipoUsuarioService;

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

            var tiposUsuario = new List<Tipousuario>
            {
                new() { Id = 1, Descricao = "ADMIN" },
                new() { Id = 2, Descricao = "GESTOR" },
                new() { Id = 3, Descricao = "COLABORADOR" }
            };
            context.AddRange(tiposUsuario);

            var usuarioOrganizacao = new List<Usuarioorganizacao>
            {
                new() { IdUsuario = 1, IdOrganizacao = 1, IdTipoUsuario = 1 }, // ADMIN
                new() { IdUsuario = 2, IdOrganizacao = 1, IdTipoUsuario = 2 }, // GESTOR
                new() { IdUsuario = 3, IdOrganizacao = 1, IdTipoUsuario = 3 }  // COLABORADOR
            };
            context.AddRange(usuarioOrganizacao);

            context.SaveChanges();
            tipoUsuarioService = new TipoUsuarioService(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaTipos = tipoUsuarioService.GetAll();

            // Assert
            Assert.IsNotNull(listaTipos);
            Assert.AreEqual<int>(3, listaTipos.Count);
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "ADMIN"));
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "GESTOR"));
            Assert.IsTrue(listaTipos.Any(t => t.Descricao == "COLABORADOR"));
        }

        [TestMethod()]
        public void GetTipoUsuarioByUsuarioIdTest()
        {
            // Act
            var resultado = tipoUsuarioService.GetTipoUsuarioByUsuarioId(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual<uint>(1, resultado.Id);
            Assert.AreEqual<string>("ADMIN", resultado.Descricao);
        }

        [TestMethod()]
        public void GetTipoUsuarioByUsuarioIdTest_UserNotFound()
        {
            // Act
            var resultado = tipoUsuarioService.GetTipoUsuarioByUsuarioId(999);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var novoTipo = new TipoUsuarioModel
            {
                Id = 4,
                Descricao = "SUPERVISOR"
            };

            // Act
            var resultado = tipoUsuarioService.Insert(novoTipo);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoUsuarioService.GetAll();
            Assert.AreEqual<int>(4, tipos.Count);
            Assert.IsTrue(tipos.Any(t => t.Descricao == "SUPERVISOR"));
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange
            var novoTipo = new TipoUsuarioModel
            {
                Id = 4,
                Descricao = "TEMPORARIO"
            };
            tipoUsuarioService.Insert(novoTipo);

            // Act
            var resultado = tipoUsuarioService.Remove(4);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoUsuarioService.GetAll();
            Assert.AreEqual<int>(3, tipos.Count);
            Assert.IsFalse(tipos.Any(t => t.Descricao == "TEMPORARIO"));
        }

        [TestMethod()]
        public void RemoveTest_NonExistentId()
        {
            // Act
            var resultado = tipoUsuarioService.Remove(999); 

            // Assert
            Assert.IsFalse(resultado);
            var tipos = tipoUsuarioService.GetAll();
            Assert.AreEqual<int>(3, tipos.Count); // Certifica-se de que nada foi removido
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            var tipoExistente = new TipoUsuarioModel
            {
                Id = 2,
                Descricao = "GESTOR_SENIOR"
            };

            // Act
            var resultado = tipoUsuarioService.Update(tipoExistente);

            // Assert
            Assert.IsTrue(resultado);
            var tipos = tipoUsuarioService.GetAll();
            Assert.IsTrue(tipos.Any(t => t.Descricao == "GESTOR_SENIOR"));
        }

        [TestMethod()]
        public void UpdateTest_NonExistentType()
        {
            // Arrange
            var tipoInexistente = new TipoUsuarioModel
            {
                Id = 999,
                Descricao = "TIPO_INEXISTENTE"
            };

            // Act
            var resultado = tipoUsuarioService.Update(tipoInexistente);

            // Assert
            Assert.IsFalse(resultado);
        }
    }
}