using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class HardwareDeBlocoController : Controller
    {
        private readonly HardwareDeBlocoService _hardwareService;
        private readonly TipoHardwareService _tipoHardwareService;
        private readonly BlocoService _blocoService;


        public HardwareDeBlocoController(HardwareDeBlocoService hardwareService, TipoHardwareService tipoHardwareService, BlocoService blocoService)
        {
            _hardwareService = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _blocoService = blocoService;
        }


        public IActionResult Index(string pesquisa)
        {
            var hardwares = ReturnAllViewModels();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                hardwares = hardwares.Where(s => s.MAC.Contains(pesquisa)).ToList();
            }

            return View(hardwares);
        }

        public IActionResult Create()
        {
            ViewBag.blocos = new SelectList(_blocoService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeBlocoModel hardware)
        {

            ViewBag.blocos = new SelectList(_blocoService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");

            if (ModelState.IsValid)
            {
                if (!CompareMAC(hardware))
                {
                    if (_hardwareService.Insert(hardware))
                        return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["aviso"] = "Um hardware com o endereço Mac especificado já existe!";
                    return View(hardware);
                }
            }

            return View(hardware);

        }


        public IActionResult Edit(int id)
        {
            ViewBag.blocos = new SelectList(_blocoService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");

            HardwareDeBlocoModel hardware = _hardwareService.GetById(id);
            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeBlocoModel hardware)
        {
            ViewBag.blocos = new SelectList(_blocoService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");
        

            if (ModelState.IsValid)
            {
                if (!CompareMAC(hardware))
                {
                    if (_hardwareService.Update(hardware))
                        return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["aviso"] = "Um hardware com o endereço Mac especificado já existe!";
                    return View(hardware);
                }
            }

            return View(hardware);
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
            if (_hardwareService.Remove(id))
                return RedirectToAction(nameof(Index));

            return View(ReturnByIdViewModel(id));
        }

        public bool CompareMAC(HardwareDeBlocoModel hardware)
        {
           
            List<HardwareDeBlocoModel> itens = _hardwareService.GetAll().Where(s => s.MAC.Equals(hardware.MAC) && s.Id != hardware.Id).ToList();

            return itens.Count() == 0 ? false : true;
        }


        private List<HardwareDeBlocoViewModel> ReturnAllViewModels()
        {
            List<HardwareDeBlocoModel> hardwares = _hardwareService.GetAll();
            List<HardwareDeBlocoViewModel> hardwaresViewModel = new List<HardwareDeBlocoViewModel>();
            foreach (var item in hardwares)
            {
                hardwaresViewModel.Add(Cast(item));
            }

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
