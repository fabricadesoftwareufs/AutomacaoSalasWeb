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
    public class PlanejamentoServiceTests
    {
        private SalasDBContext context;
        private IPlanejamentoService planejamentoService;

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

            var planejamentos = new List<Planejamento>{new(){Id = 1, DataInicio = new DateTime(2024, 2, 1), DataFim = new DateTime(2024, 2, 15), HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(10, 0, 0), DiaSemana = "Segunda-feira", Objetivo = "Aula de matemática", IdUsuario = 1, IdSala = 1 }, new(){ Id = 2, DataInicio = new DateTime(2024, 2, 3), DataFim = new DateTime(2024, 2, 18), HorarioInicio = new TimeSpan(14, 0, 0), HorarioFim = new TimeSpan(16, 0, 0), DiaSemana = "Quarta-feira", Objetivo = "Reunião de equipe", IdUsuario = 2, IdSala = 1}};
            context.AddRange(planejamentos);
            context.SaveChanges();
            planejamentoService = new PlanejamentoService(context);
        }
        [TestMethod()]
        public void GetAllTest()
        {
            var resultado = planejamentoService.GetAll();
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count());
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            var resultado = planejamentoService.GetById(1);
            Assert.IsNotNull(resultado);
            Assert.AreEqual((uint)1, resultado.Id);
        }

        [TestMethod()]
        public void GetByIdSalaTest()
        {
            var resultado = planejamentoService.GetByIdSala(1);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod()]
        public void GetByIdUsuarioTest()
        {
            var resultado = planejamentoService.GetByIdUsuario(2);
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod()]
        public void GetByIdOrganizacaoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertPlanejamentoWithListHorariosTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InsertTest()
        {
            var planejamento = new PlanejamentoModel{ DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(5), HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(10, 0, 0), DiaSemana = "Terça-feira", Objetivo = "Workshop", UsuarioId = 4, SalaId = 2};
            context.Planejamentos.Add(new Planejamento{DataInicio = planejamento.DataInicio,DataFim = planejamento.DataFim, HorarioInicio = planejamento.HorarioInicio,HorarioFim = planejamento.HorarioFim,DiaSemana = planejamento.DiaSemana,Objetivo = planejamento.Objetivo,IdUsuario = planejamento.UsuarioId,IdSala = planejamento.SalaId});
            context.SaveChanges();
            var planejamentoInserido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");
            Assert.IsNotNull(planejamentoInserido);
            Assert.AreEqual("Workshop", planejamentoInserido.Objetivo);
        }
        [TestMethod()]
        public void RemoveTest()
        {
            var planejamento = new PlanejamentoModel{DataInicio = DateTime.Now,DataFim = DateTime.Now.AddDays(5),HorarioInicio = new TimeSpan(8, 0, 0),HorarioFim = new TimeSpan(10, 0, 0),DiaSemana = "Terça-feira",Objetivo = "Workshop",UsuarioId = 4,SalaId = 2};
            context.Planejamentos.Add(new Planejamento{DataInicio = planejamento.DataInicio,DataFim = planejamento.DataFim,HorarioInicio = planejamento.HorarioInicio,HorarioFim = planejamento.HorarioFim,DiaSemana = planejamento.DiaSemana,Objetivo = planejamento.Objetivo,IdUsuario = planejamento.UsuarioId,IdSala = planejamento.SalaId});
            context.SaveChanges();
            var planejamentoInserido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");
            Assert.IsNotNull(planejamentoInserido);
            context.Planejamentos.Remove(planejamentoInserido);
            context.SaveChanges();
            var planejamentoRemovido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");
            Assert.IsNull(planejamentoRemovido);
        }


        [TestMethod()]
        public void UpdateTest()
        {
            Assert.Fail();
        }


        [TestMethod()]
        public void RemoveByUsuarioTest()
        {
            Assert.Fail();
        }
    }
}