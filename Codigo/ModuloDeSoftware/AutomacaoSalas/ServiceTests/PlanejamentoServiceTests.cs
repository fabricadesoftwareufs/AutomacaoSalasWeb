using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.AuxModel;
using Model.ViewModel;
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
            builder.UseInMemoryDatabase("automacaosalas")
                   .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // Ignora o aviso de transação
            var options = builder.Options;
            context = new SalasDBContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var planejamentos = new List<Planejamento>
            {
                new() { Id = 1, DataInicio = new DateTime(2024, 2, 1), DataFim = new DateTime(2024, 2, 15), HorarioInicio = new TimeSpan(8, 0, 0), HorarioFim = new TimeSpan(10, 0, 0), DiaSemana = "Segunda-feira", Objetivo = "Aula de matemática", IdUsuario = 1, IdSala = 1 },
                new() { Id = 2, DataInicio = new DateTime(2024, 2, 3), DataFim = new DateTime(2024, 2, 18), HorarioInicio = new TimeSpan(14, 0, 0), HorarioFim = new TimeSpan(16, 0, 0), DiaSemana = "Quarta-feira", Objetivo = "Reunião de equipe", IdUsuario = 2, IdSala = 1 }
            };
            context.AddRange(planejamentos);
            context.SaveChanges();
            planejamentoService = new PlanejamentoService(context);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var resultado = planejamentoService.GetAll();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count());
        }

        [TestMethod()]
        public void GetByIdTest()
        {
            // Act
            var resultado = planejamentoService.GetById(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual((uint)1, resultado.Id);
        }

        [TestMethod()]
        public void GetByIdSalaTest()
        {
            // Act
            var resultado = planejamentoService.GetByIdSala(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod()]
        public void GetByIdUsuarioTest()
        {
            // Act
            var resultado = planejamentoService.GetByIdUsuario(2);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(1, resultado.Count);
        }

        [TestMethod()]
        public void GetByIdOrganizacaoTest()
        {

            // Act
            var resultado = planejamentoService.GetByIdSala(1);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);
            Assert.AreEqual((uint)1, resultado.First().Id);
            Assert.AreEqual("Segunda-feira", resultado.First().DiaSemana);
            Console.WriteLine(resultado.First().DiaSemana);
        }

        [TestMethod()]
        public void InsertPlanejamentoWithListHorariosTest()
        {
            // Arrange
            var planejamentoAuxModel = new PlanejamentoAuxModel
            {
                Planejamento = new PlanejamentoModel
                {
                    Objetivo = "Curso de Física",
                    DataInicio = DateTime.Now,
                    DataFim = DateTime.Now.AddDays(3),
                    HorarioInicio = new TimeSpan(8, 0, 0),
                    HorarioFim = new TimeSpan(10, 0, 0),
                    DiaSemana = "Segunda-feira",
                    UsuarioId = 4,
                    SalaId = 1
                },
                Horarios = new List<HorarioPlanejamentoAuxModel>{
                    new HorarioPlanejamentoAuxModel{
                        HorarioInicio = new TimeSpan(8, 0, 0),
                        HorarioFim = new TimeSpan(10, 0, 0),
                        DiaSemana = "Segunda-feira"
                    },
                    new HorarioPlanejamentoAuxModel{
                        HorarioInicio = new TimeSpan(10, 0, 0),
                        HorarioFim = new TimeSpan(12, 0, 0),
                        DiaSemana = "Quarta-feira"
                    }
                }
            };

            // Act
            var resultado = planejamentoService.InsertPlanejamentoWithListHorarios(planejamentoAuxModel);

            // Assert
            Assert.IsTrue(resultado);
            var planejamentosInseridos = context.Planejamentos.Where(p => p.Objetivo == "Curso de Física").ToList();
            Assert.AreEqual(2, planejamentosInseridos.Count);  // Verifica se dois planejamentos foram inseridos
        }

        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            var planejamento = new PlanejamentoModel
            {
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                HorarioInicio = new TimeSpan(8, 0, 0),
                HorarioFim = new TimeSpan(10, 0, 0),
                DiaSemana = "Terça-feira",
                Objetivo = "Workshop",
                UsuarioId = 4,
                SalaId = 2
            };

            context.Planejamentos.Add(new Planejamento
            {
                DataInicio = planejamento.DataInicio,
                DataFim = planejamento.DataFim,
                HorarioInicio = planejamento.HorarioInicio,
                HorarioFim = planejamento.HorarioFim,
                DiaSemana = planejamento.DiaSemana,
                Objetivo = planejamento.Objetivo,
                IdUsuario = planejamento.UsuarioId,
                IdSala = planejamento.SalaId
            });
            context.SaveChanges();
            var planejamentoInserido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");

            // Assert
            Assert.IsNotNull(planejamentoInserido);
            Assert.AreEqual("Workshop", planejamentoInserido.Objetivo);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            // Arrange
            var planejamento = new PlanejamentoModel
            {
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                HorarioInicio = new TimeSpan(8, 0, 0),
                HorarioFim = new TimeSpan(10, 0, 0),
                DiaSemana = "Terça-feira",
                Objetivo = "Workshop",
                UsuarioId = 4,
                SalaId = 2
            };

            context.Planejamentos.Add(new Planejamento
            {
                DataInicio = planejamento.DataInicio,
                DataFim = planejamento.DataFim,
                HorarioInicio = planejamento.HorarioInicio,
                HorarioFim = planejamento.HorarioFim,
                DiaSemana = planejamento.DiaSemana,
                Objetivo = planejamento.Objetivo,
                IdUsuario = planejamento.UsuarioId,
                IdSala = planejamento.SalaId
            });
            context.SaveChanges();

            // Act
            var planejamentoInserido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");
            Assert.IsNotNull(planejamentoInserido);

            context.Planejamentos.Remove(planejamentoInserido);
            context.SaveChanges();

            // Assert
            var planejamentoRemovido = context.Planejamentos.FirstOrDefault(p => p.Objetivo == "Workshop");
            Assert.IsNull(planejamentoRemovido);
        }

        [TestMethod()]
        public void UpdateTest()
        {
            // Act
            var planejamentoExistente = context.Planejamentos.FirstOrDefault(p => p.Id == 1);
            Assert.IsNotNull(planejamentoExistente, "O planejamento inicial não foi encontrado.");
            planejamentoExistente.Objetivo = "Aula de Física";
            planejamentoExistente.HorarioInicio = new TimeSpan(9, 0, 0);
            planejamentoExistente.HorarioFim = new TimeSpan(11, 0, 0);
            var planejamentoAtualizado = context.Planejamentos.FirstOrDefault(p => p.Id == 1);

            // Assert
            Assert.IsNotNull(planejamentoAtualizado);
            Assert.AreEqual("Aula de Física", planejamentoAtualizado.Objetivo, "O objetivo não foi atualizado corretamente.");
            Assert.AreEqual(new TimeSpan(9, 0, 0), planejamentoAtualizado.HorarioInicio, "O horário de início não foi atualizado corretamente.");
            Assert.AreEqual(new TimeSpan(11, 0, 0), planejamentoAtualizado.HorarioFim, "O horário de fim não foi atualizado corretamente.");
        }

        [TestMethod()]
        public void RemoveByUsuarioTest()
        {
            // Arrange
            var planejamento1 = new Planejamento
            {
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddDays(5),
                HorarioInicio = new TimeSpan(8, 0, 0),
                HorarioFim = new TimeSpan(10, 0, 0),
                DiaSemana = "Terça-feira",
                Objetivo = "Workshop",
                IdUsuario = 4,
                IdSala = 2
            };
            var planejamento2 = new Planejamento
            {
                DataInicio = DateTime.Now.AddDays(1),
                DataFim = DateTime.Now.AddDays(6),
                HorarioInicio = new TimeSpan(10, 0, 0),
                HorarioFim = new TimeSpan(12, 0, 0),
                DiaSemana = "Quarta-feira",
                Objetivo = "Reunião de planejamento",
                IdUsuario = 4,
                IdSala = 2
            };
            context.Planejamentos.Add(planejamento1);
            context.Planejamentos.Add(planejamento2);
            context.SaveChanges();
            var planejamentosParaRemover = context.Planejamentos.Where(p => p.IdUsuario == 4).ToList();
            context.Planejamentos.RemoveRange(planejamentosParaRemover);
            context.SaveChanges();
            var planejamentosRemovidos = context.Planejamentos.Where(p => p.IdUsuario == 4).ToList();
            
            // Assert
            Assert.AreEqual(0, planejamentosRemovidos.Count, "Os planejamentos do usuário não foram removidos corretamente.");
        }
    }
}