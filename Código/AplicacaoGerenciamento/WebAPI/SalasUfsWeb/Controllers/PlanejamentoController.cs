using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
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
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(_usuarioService.GetSelectedList(), "Id", "Nome");

            return View(new PlanejamentoModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanejamentoModel planejamento)
        {

            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(_usuarioService.GetSelectedList(), "Id", "Nome");

            try
            {
                if (ModelState.IsValid)
                    _planejamentoService.Insert(planejamento);
                else
                    return View(planejamento);

                TempData["mensagemSucesso"] = "Planejamento cadastrado com sucesso!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(new PlanejamentoModel());
        }


        public IActionResult Edit(int id)
        {
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(_usuarioService.GetSelectedList(), "Id", "Nome");
         

            PlanejamentoModel planejamento = _planejamentoService.GetById(id);
            return View(planejamento);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PlanejamentoModel planejamento)
        {
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(_usuarioService.GetSelectedList(), "Id", "Nome");

            if (ModelState.IsValid)
            {
                if ((DateTime.Compare(planejamento.DataFim, planejamento.DataInicio) > 0 && TimeSpan.Compare(planejamento.HorarioFim, planejamento.HorarioInicio) == 1))
                {
                    if (_planejamentoService.Update(planejamento))
                        return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["aviso"] = "Sua Datas/Horarios possuem inconsistências, corrija e tente novamente";
                    return View(planejamento);
                }
            }

            return View(planejamento);
        }


        public IActionResult Details(int id)
        {
            return View(ReturnByIdViewModel(id));
        }

        public IActionResult Delete(int id)
        {
           
            return View(ReturnByIdViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            if (_planejamentoService.Remove(id))
                return RedirectToAction(nameof(Index));

            return View(ReturnByIdViewModel(id));
        }

        private List<PlanejamentoViewModel> ReturnAllViewModels()
        {
            List<PlanejamentoModel> pl = _planejamentoService.GetAll();
            List<PlanejamentoViewModel> plvm = new List<PlanejamentoViewModel>();
            foreach (var item in pl)
            {
                plvm.Add(Cast(item));
            }

            return plvm;
        }

        private PlanejamentoViewModel ReturnByIdViewModel(int id)
        {
            PlanejamentoModel pl = _planejamentoService.GetById(id);
           
            return Cast(pl);
        }

        private PlanejamentoViewModel Cast(PlanejamentoModel item)
        {
            PlanejamentoViewModel p = new PlanejamentoViewModel();

            p.Periodo = item.DataInicio.ToString("dd/MM/yyyy") + " à " + item.DataFim.ToString("dd/MM/yyyy");
            p.DiaSemana = item.DiaSemana;
            p.Horario = item.HorarioInicio + " às " + item.HorarioFim;
            p.Id = item.Id;
            p.UsuarioId = _usuarioService.GetById(item.UsuarioId);
            p.SalaId = _salaService.GetById(item.SalaId);
            p.Objetivo = item.Objetivo;



            return p;
        }
    }
}
