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
    public class SalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IHardwareDeSalaService _hardwareDeSalaService;
        private readonly ITipoHardwareService _tipoHardwareService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IOrganizacaoService _organizacaoService;


        public SalaController(ISalaService salaService,
                              IBlocoService blocoService,
                              IHardwareDeSalaService hardwareDeSalaService,
                              ITipoHardwareService tipoHardwareService,
                              IUsuarioOrganizacaoService usuarioOrganizacaoService,
                              IUsuarioService usuarioService,
                              IOrganizacaoService organizacaoService)
        {
            _salaService = salaService;
            _blocoService = blocoService;
            _hardwareDeSalaService = hardwareDeSalaService;
            _tipoHardwareService = tipoHardwareService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
            _organizacaoService = organizacaoService;
        }
        // GET: Sala
        public ActionResult Index()
        {
            return View(GetAllSalasViewModel());
        }

        // GET: Sala/Details/5
        public ActionResult Details(uint id)
        {
            return View(GetSalaViewModel(id));
        }

        // GET: Sala/Create
        public ActionResult Create()
        {
            var orgs = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            ViewBag.Organizacoes = orgs;
            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(orgs.FirstOrDefault().Id);
            ViewBag.TipoHardware = _tipoHardwareService.GetAll();

            return View();
        }

        // POST: Sala/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaAuxModel salaModel)
        {
            var usuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity);
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(usuario.UsuarioModel.Id);
            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(salaModel.OrganizacaoId);
            ViewBag.TipoHardware = _tipoHardwareService.GetAll();

            try
            {
                if (ModelState.IsValid)
                {
                    bool salaInserida = _salaService.InsertSalaWithHardwares(salaModel, usuario.UsuarioModel.Id);

                    if (salaInserida)
                    {
                        TempData["mensagemSucesso"] = "Sala inserida com sucesso!";
                        return View();
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir a sala!";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaModel);
        }


        // GET: Sala/Edit/5
        public ActionResult Edit(uint id)
        {
            var salaModel = _salaService.GetById(id);
            var idOrganizacao = _blocoService.GetById(salaModel.BlocoId).OrganizacaoId;

            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(idOrganizacao);
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            return View(new SalaAuxModel { Sala = new SalaModel { Id = salaModel.Id, Titulo = salaModel.Titulo, BlocoId = salaModel.BlocoId }, OrganizacaoId = idOrganizacao });
        }

        // POST: Sala/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SalaAuxModel salaModel)
        {
            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(salaModel.OrganizacaoId);
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Update(new SalaModel { Id = salaModel.Sala.Id, BlocoId = salaModel.Sala.BlocoId, Titulo = salaModel.Sala.Titulo }))
                        TempData["mensagemSucesso"] = "Sala atualizada com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar sala, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaModel);
        }

        // GET: Sala/Delete/5
        public ActionResult Delete(uint id)
        {
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // POST: Sala/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            SalaModel salaModel = _salaService.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Remove(id))
                        TempData["mensagemSucesso"] = "Sala removida com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao remover a sala, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<SalaViewModel> GetAllSalasViewModel()
        {
            var idUser = uint.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var salasViewModel = new List<SalaViewModel>();

            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUser);
            salas.ForEach(s => salasViewModel.Add(new SalaViewModel { BlocoSala = _blocoService.GetById(s.BlocoId), Sala = _salaService.GetById(s.Id) }));


            return salasViewModel;
        }

        private SalaViewModel GetSalaViewModel(uint id)
        {
            var sala = _salaService.GetById(id);
            var hardwaresViewModel = new List<HardwareDeSalaViewModel>();

            foreach (var item in _hardwareDeSalaService.GetByIdSala(id))
                hardwaresViewModel.Add(new HardwareDeSalaViewModel { Id = item.Id, MAC = item.MAC, TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId) });

            return new SalaViewModel
            {
                Sala = sala,
                HardwaresSala = hardwaresViewModel,
                BlocoSala = _blocoService.GetById(sala.BlocoId)
            };
        }
    }
}