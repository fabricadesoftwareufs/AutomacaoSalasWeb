using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;
using Service;
using System;
using System.Collections.Generic;

namespace Service.Tests
{
    [TestClass()]
    public class MonitoramentoServiceTests
    {
        private SalasDBContext? context;
        private MonitoramentoService? monitoramentoService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<SalasDBContext>();
            builder.UseInMemoryDatabase($"automacaosalas-monitoramento-{Guid.NewGuid()}")
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            context = new SalasDBContext(builder.Options);
            context.Database.EnsureCreated();

            context.Equipamentos.AddRange(new List<Equipamento>
            {
                new()
                {
                    Id = 1,
                    IdSala = 10,
                    TipoEquipamento = "LUZES",
                    Status = "D"
                },
                new()
                {
                    Id = 2,
                    IdSala = 10,
                    TipoEquipamento = null!,
                    Status = "D"
                }
            });

            context.Monitoramentos.AddRange(new List<Monitoramento>
            {
                new()
                {
                    Id = 1,
                    IdEquipamento = 1,
                    IdOperacao = 1,
                    IdUsuario = 1,
                    DataHora = DateTime.Now,
                    Estado = 1
                },
                new()
                {
                    Id = 2,
                    IdEquipamento = 2,
                    IdOperacao = 1,
                    IdUsuario = 1,
                    DataHora = DateTime.Now,
                    Estado = 0
                }
            });

            context.SaveChanges();
            monitoramentoService = new MonitoramentoService(context);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest_DeveIgnorarTipoNuloNoBanco()
        {
            var monitoramento = monitoramentoService!.GetByIdSalaAndTipoEquipamento(10, " luzes ");

            Assert.IsNotNull(monitoramento);
            Assert.AreEqual(1, monitoramento.Id);
            Assert.AreEqual(1, monitoramento.IdEquipamento);
            Assert.IsTrue(monitoramento.Estado);
        }

        [TestMethod()]
        public void GetByIdSalaAndTipoEquipamentoTest_DeveRetornarNullQuandoTipoForInvalido()
        {
            var monitoramento = monitoramentoService!.GetByIdSalaAndTipoEquipamento(10, " ");

            Assert.IsNull(monitoramento);
        }
    }
}
