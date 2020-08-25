using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class HardwareDeBlocoController : Controller
    {
        private readonly IHardwareDeBlocoService _hardwareService;
        private readonly ITipoHardwareService _tipoHardwareService;
        private readonly IBlocoService _blocoService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioService _usuarioService;

        public HardwareDeBlocoController(IHardwareDeBlocoService hardwareService, 
                                         ITipoHardwareService tipoHardwareService, 
                                         IBlocoService blocoService,
                                         IUsuarioService usuarioService,
                                         ISalaService salaService,
                                         IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                         IOrganizacaoService organizacaoService)
        {
            _hardwareService     = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _blocoService        = blocoService;
            _usuarioService      = usuarioService;
            _salaService         = salaService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _organizacaoService        = organizacaoService;
        }


        public IActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        public IActionResult Create()
        {
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id);

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Blocos = organizacoes.Count > 0 ? _blocoService.GetByIdOrganizacao(organizacoes[0].Id) : new List<BlocoModel>();
            ViewBag.TipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeBlocoModel hardware)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id;
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(idUsuario);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(hardware.Organizacao);
            ViewBag.TipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_hardwareService.Insert(hardware, idUsuario))
                    {
                        TempData["mensagemSucesso"] = "Hardware adicionado com sucesso!";
                        return View();
                    }
                    else TempData["mensagemErro"] = "Houve um problem ao inserir hardware, tente novamente em alguns minutos!!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(hardware);
        }


        public IActionResult Edit(int id)
        {
            var hardware = _hardwareService.GetById(id);
            hardware.Organizacao = _blocoService.GetById(hardware.BlocoId).OrganizacaoId;

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(hardware.Organizacao);
            ViewBag.TipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");
            
            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeBlocoModel hardware)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(usuario.Id);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(hardware.Organizacao);
            ViewBag.TipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_hardwareService.Update(hardware, usuario.Id)) TempData["mensagemSucesso"] = "Hardware atualizado com sucesso!";
                    else TempData["mensagemErro"] = "Houve um problema ao atualizar Hardware, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(hardware);
        }


        public IActionResult Details(int id)
        {
            return View(ReturnByIdViewModel(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_hardwareService.Remove(id)) TempData["mensagemSucesso"] = "Hadware removido com sucesso!";
                else TempData["mensagemSucesso"] = "Houve um problema ao tentar remover o hardware, tente novamente em alguns minutos";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            
            return RedirectToAction(nameof(Index));
        }

        private List<HardwareDeBlocoViewModel> ReturnAllViewModels()
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            var hardwares = _hardwareService.GetAllHardwaresBlacoByUsuarioOrganizacao(usuario.Id);
            
            var hardwaresViewModel = new List<HardwareDeBlocoViewModel>();
            hardwares.ForEach(e => hardwaresViewModel.Add(Cast(e)));

            return hardwaresViewModel;
        }

        private HardwareDeBlocoViewModel ReturnByIdViewModel(int id)
        {
            HardwareDeBlocoModel h = _hardwareService.GetById(id);
            return Cast(h);
        }

        private HardwareDeBlocoViewModel Cast(HardwareDeBlocoModel item)
        {
            HardwareDeBlocoViewModel h = new HardwareDeBlocoViewModel();

            h.Id = item.Id;
            h.MAC = item.MAC;
            h.BlocoId = _blocoService.GetById(item.BlocoId);
            h.TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId);

            return h;
        }
    }
}
