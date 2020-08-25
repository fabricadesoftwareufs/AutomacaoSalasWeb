using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
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
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id);
            var blocos = organizacoes.Count > 0 ? _blocoService.GetByIdOrganizacao(organizacoes[0].Id) : new List<BlocoModel>();

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios     = _usuarioService.GetByIdOrganizacao(organizacoes[0].Id);
            ViewBag.Salas        = blocos.Count > 0 ? _salaService.GetByIdBloco(blocos[0].Id) : new List<SalaModel>();
            ViewBag.Blocos       = blocos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PlanejamentoAuxModel planejamentoModel)
        {
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id); ;
            ViewBag.Usuarios     = _usuarioService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            ViewBag.Salas        = _salaService.GetByIdBloco(planejamentoModel.Bloco);
            ViewBag.Blocos       = _blocoService.GetByIdOrganizacao(planejamentoModel.Organizacao);

            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.InsertListHorariosPlanjamento(planejamentoModel))
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

            return View(planejamentoModel);
        }


        public IActionResult Edit(int id)
        {
            var planejamento = _planejamentoService.GetById(id);
            var bloco = _blocoService.GetById(_salaService.GetById(planejamento.SalaId).BlocoId);

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id); ;
            ViewBag.Usuarios     = _usuarioService.GetByIdOrganizacao(bloco.OrganizacaoId);
            ViewBag.Salas        = _salaService.GetByIdBloco(bloco.Id);
            ViewBag.Blocos       = _blocoService.GetByIdOrganizacao(bloco.OrganizacaoId);

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
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id); ;
            ViewBag.Usuarios     = _usuarioService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            ViewBag.Salas        = _salaService.GetByIdBloco(planejamentoModel.Bloco);
            ViewBag.Blocos       = _blocoService.GetByIdOrganizacao(planejamentoModel.Organizacao);
            try
            {
                if (ModelState.IsValid)
                {
                    if (_planejamentoService.Update(planejamentoModel.Planejamento))
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

            return View(planejamentoModel);
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
