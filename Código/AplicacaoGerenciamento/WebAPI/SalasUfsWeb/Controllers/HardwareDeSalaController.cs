using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Service;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class HardwareDeSalaController : Controller
    {
        private readonly IHardwareDeSalaService _hardwareService;
        private readonly ITipoHardwareService _tipoHardwareService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;


        public HardwareDeSalaController(IHardwareDeSalaService hardwareService, 
                                        ITipoHardwareService tipoHardwareService, 
                                        ISalaService salaService,
                                        IUsuarioService usuarioService)
        {
            _hardwareService = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _salaService = salaService;
            _usuarioService = usuarioService;
        }


        public IActionResult Index()
        {
            var hardwares = GetAllViewModels();
            return View(hardwares);
        }

        public IActionResult Create()
        {
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeSalaModel hardware)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(usuario.Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_hardwareService.Insert(hardware,usuario.Id))
                    {
                        TempData["mensagemSucesso"] = "Hardware adicionado com sucesso!";
                        return View();
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao tentar inserir o hardware, tente novamente em alguns minutos!";
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
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            HardwareDeSalaModel hardware = _hardwareService.GetById(id);
            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeSalaModel hardware)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(usuario.Id), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_hardwareService.Update(hardware,usuario.Id))
                        TempData["mensagemSucesso"] = "Hardware atualizado com sucesso";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar hardware, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se) { TempData["mensagemErro"] = se.Message; }

            return RedirectToAction(nameof(Index));
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
                if (_hardwareService.Remove(id))
                    TempData["mensagemSucesso"] = "Hardware removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover o hardware, tente novamente em alguns minutos!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<HardwareDeSalaViewModel> GetAllViewModels()
        {
            var hardwares = _hardwareService.GetAllHardwaresSalaByUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id);
            var  hardwaresViewModel = new List<HardwareDeSalaViewModel>();

            hardwares.ForEach(h => hardwaresViewModel.Add(Cast(h)));

            return hardwaresViewModel;
        }

        private HardwareDeSalaViewModel GetByIdViewModel(int id)
        {
            HardwareDeSalaModel h = _hardwareService.GetById(id);

            return Cast(h);
        }

        private HardwareDeSalaViewModel Cast(HardwareDeSalaModel item)
        {
            HardwareDeSalaViewModel h = new HardwareDeSalaViewModel();

            h.Id = item.Id;
            h.MAC = item.MAC;
            h.SalaId = _salaService.GetById(item.SalaId);
            h.TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId);

            return h;
        }
    }
}
