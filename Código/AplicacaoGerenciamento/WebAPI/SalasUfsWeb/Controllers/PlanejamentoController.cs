using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class PlanejamentoController : Controller
    {
        private readonly IPlanejamentoService _planejamentoService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;


        public PlanejamentoController(IPlanejamentoService service,
                                      ISalaService salaService,
                                      IUsuarioService usuarioService,
                                      IUsuarioOrganizacaoService usuarioOrganizacaoService)
        {
            _planejamentoService = service;
            _salaService = salaService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
        }

        public IActionResult Index()
        {
            return View(GetAllPlanejamentosViewModels());
        }

        public IActionResult Create()
        {
            var id = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id;
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(id)
                .Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            
            ViewBag.usuarios = new SelectList(GetAllUsersByOrganizacao().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanejamentoModel planejamento)
        {
            var id = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id;
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(id)
                .Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(GetAllUsersByOrganizacao().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.InsertListHorariosPlanjamento(planejamento))
                    {
                        TempData["mensagemSucesso"] = "Planejamento cadastrado com sucesso!";
                        return View();
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir novo planejamento, tente novamente em alguns minutos.";
                }


            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(planejamento);
        }


        public IActionResult Edit(int id)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id;
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(idUsuario)
                        .Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(GetAllUsersByOrganizacao().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");


            return View(_planejamentoService.GetById(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PlanejamentoModel planejamento)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id;
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(idUsuario)
                          .Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.usuarios = new SelectList(GetAllUsersByOrganizacao().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.Update(planejamento))
                    {
                        TempData["mensagemSucesso"] = "Planejamento Atualizado com sucesso!";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir novo planejamento, tente novamente em alguns minutos.";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(planejamento);
        }


        public IActionResult Details(int id)
        {
            return View(GetByIdViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_planejamentoService.Remove(id))
                    TempData["mensagemSucesso"] = "Planejmento removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ou remover o Planejamento, tente novamente em alguns minutos";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private List<PlanejamentoViewModel> GetAllPlanejamentosViewModels()
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            var orgs = _usuarioOrganizacaoService.GetByIdUsuario(usuario.Id);

            var planejamentos = new List<PlanejamentoViewModel>();

            orgs.ForEach(e =>
                _planejamentoService.GetByIdOrganizacao(e.OrganizacaoId).ForEach(p =>
                        planejamentos.Add(Cast(p))
                )
            );

            return planejamentos;
        }

        private List<UsuarioModel> GetAllUsersByOrganizacao()
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            var orgs = _usuarioOrganizacaoService.GetByIdUsuario(usuario.Id);

            var usuariosOrganizacoes = new List<UsuarioModel>();
            orgs.ForEach(o => usuariosOrganizacoes.AddRange(_usuarioService.GetByIdOrganizacao(o.OrganizacaoId)));

            return usuariosOrganizacoes;
        }

        private PlanejamentoViewModel GetByIdViewModel(int id)
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
