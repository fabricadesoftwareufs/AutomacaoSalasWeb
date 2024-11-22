using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ADMINISTRATIVE_ROLES)]
    public class PlanejamentoController : Controller
    {
        private readonly IPlanejamentoService _planejamentoService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;

        public PlanejamentoController(IPlanejamentoService service,
                                      ISalaService salaService,
                                      IUsuarioService usuarioService,
                                      IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                      IBlocoService blocoService,
                                      IOrganizacaoService organizacaoService)
        {
            _planejamentoService = service;
            _salaService = salaService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
        }

        public IActionResult Index()
        {
            return View(GetAllPlanejamentosViewModels());
        }

        public IActionResult Create()
        {
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanejamentoAuxModel planejamentoModel)
        {
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id); ;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(planejamentoModel.Bloco);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(planejamentoModel.Organizacao);

            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.InsertPlanejamentoWithListHorarios(planejamentoModel))
                    {
                        TempData["mensagemSucesso"] = "Planejamento cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir novo planejamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(planejamentoModel);
        }


        public IActionResult Edit(int id)
        {
            var planejamento = _planejamentoService.GetById(id);
            var bloco = _blocoService.GetById(_salaService.GetById(planejamento.SalaId).BlocoId);

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(bloco.OrganizacaoId);
            ViewBag.Salas = _salaService.GetByIdBloco(bloco.Id);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(bloco.OrganizacaoId);

            return View(new PlanejamentoAuxModel
            {
                Planejamento = _planejamentoService.GetById(id),
                Organizacao = bloco.OrganizacaoId,
                Bloco = bloco.Id
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PlanejamentoAuxModel planejamentoModel)
        {
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id); ;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(planejamentoModel.Bloco);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.Update(planejamentoModel.Planejamento))
                    {
                        TempData["mensagemSucesso"] = "Planejamento Atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao inserir novo planejamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(planejamentoModel);
        }


        public IActionResult Details(int id)
        {
            return View(GetByIdViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, bool excluirReservas, IFormCollection collection)
        {
            try
            {
                if (_planejamentoService.Remove(id, excluirReservas))
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
            var usuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity);
            var orgs = _usuarioOrganizacaoService.GetByIdUsuario(usuario.UsuarioModel.Id);

            var planejamentos = new List<PlanejamentoViewModel>();

            orgs.ForEach(e =>
                    _planejamentoService.GetByIdOrganizacao(e.OrganizacaoId).ForEach(p =>
                         planejamentos.Add(Cast(p))
                )
            );

            return planejamentos;
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
