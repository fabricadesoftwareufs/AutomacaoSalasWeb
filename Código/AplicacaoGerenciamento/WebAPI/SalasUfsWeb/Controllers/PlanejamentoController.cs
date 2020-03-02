using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using SalasUfsWeb.Models;
using SalasUfsWeb.Models.ViewModels;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class PlanejamentoController : Controller
    {
        private readonly PlanejamentoService _planejamentoService;
        private readonly SalaService _salaService;
        private readonly UsuarioService _usuarioService;


        public PlanejamentoController(PlanejamentoService service, SalaService salaService, UsuarioService usuarioService)
        {
            _planejamentoService = service;
            _salaService = salaService;
            _usuarioService = usuarioService;
        }


        public IActionResult Index(string pesquisa)
        {
            var planejamentos = ReturnAllViewModels();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                planejamentos = planejamentos.Where(s => s.SalaId.Titulo.Contains(pesquisa)).ToList();
            }

            return View(planejamentos);
        }

        public IActionResult Create()
        {
            var salas = _salaService.GetAll().Select(x => new { Value = x.Id, Text = string.Format("{0} - {1}", x.Id, x.Titulo)});
            var users = _usuarioService.GetAll().Select(x => new { Value = x.Id, Text = string.Format("{0} - {1}", x.Id, x.Nome)});

            ViewBag.salas = new SelectList(salas, "Value", "Text");
            ViewBag.usuarios = new SelectList(users, "Value", "Text");
            ViewBag.dias = new SelectList(GetDays(), "Dia", "Dia");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List<PlanejamentoModel> planejamento)
        {
            foreach (var item in planejamento)
            {
                if (ModelState.IsValid)
                {
                    if (!_planejamentoService.Insert(item))
                        return View(planejamento);
                    else
                        planejamento.Remove(item);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private List<PlanejamentoViewModel> ReturnAllViewModels(){
            List<PlanejamentoModel> pl = _planejamentoService.GetAll();
            List<PlanejamentoViewModel> plvm = new List<PlanejamentoViewModel>();
            foreach (var item in pl)
            {
                plvm.Add(Cast(item));
            }

            return plvm;
        }

        private PlanejamentoViewModel Cast(PlanejamentoModel item)
        {
            PlanejamentoViewModel p = new PlanejamentoViewModel();

            p.Periodo = item.DataFim.ToString("dd/MM/yyyy") + " à " + item.DataInicio.ToString("dd/MM/yyyy");
            p.DiaSemana = item.DiaSemana;
            p.Horario = item.HorarioFim + " às " + item.HorarioInicio;
            p.Id = item.Id;
            p.UsuarioId = _usuarioService.GetById(item.UsuarioId);
            p.SalaId = _salaService.GetById(item.SalaId);


            return p;
        }

        private List<DiaDaSemanaModel> GetDays()
        {
            List<DiaDaSemanaModel> days = new List<DiaDaSemanaModel>();

            days.Add(new DiaDaSemanaModel { Id = 1, Dia = "SEG" });
            days.Add(new DiaDaSemanaModel { Id = 2, Dia = "TER" });
            days.Add(new DiaDaSemanaModel { Id = 3, Dia = "QUA" });
            days.Add(new DiaDaSemanaModel { Id = 4, Dia = "QUI" });
            days.Add(new DiaDaSemanaModel { Id = 5, Dia = "SEX" });
            days.Add(new DiaDaSemanaModel { Id = 6, Dia = "SAB" });
            days.Add(new DiaDaSemanaModel { Id = 7, Dia = "DOM" });

            return days;
        }

    }
}
