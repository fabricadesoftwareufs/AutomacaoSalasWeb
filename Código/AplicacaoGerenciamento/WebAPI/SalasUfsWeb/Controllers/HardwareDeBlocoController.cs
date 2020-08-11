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
        private readonly IUsuarioService _usuarioService;

        public HardwareDeBlocoController(IHardwareDeBlocoService hardwareService, 
                                         ITipoHardwareService tipoHardwareService, 
                                         IBlocoService blocoService,
                                         IUsuarioService usuarioService)
        {
            _hardwareService = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _blocoService = blocoService;
            _usuarioService = usuarioService;
        }


        public IActionResult Index()
        {
            var hardwares = ReturnAllViewModels();
            return View(hardwares);
        }

        public IActionResult Create()
        {
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo"); 
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeBlocoModel hardware)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(usuario.Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_hardwareService.Insert(hardware, usuario.Id))
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
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            HardwareDeBlocoModel hardware = _hardwareService.GetById(id);
            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeBlocoModel hardware)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(usuario.Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");
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
