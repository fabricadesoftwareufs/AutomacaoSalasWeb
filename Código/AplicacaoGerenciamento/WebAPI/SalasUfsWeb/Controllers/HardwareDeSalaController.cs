using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class HardwareDeSalaController : Controller
    {
        private readonly HardwareDeSalaService _hardwareService;
        private readonly TipoHardwareService _tipoHardwareService;
        private readonly SalaService _salaService;


        public HardwareDeSalaController(HardwareDeSalaService hardwareService, TipoHardwareService tipoHardwareService, SalaService salaService)
        {
            _hardwareService = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _salaService = salaService;
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
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeSalaModel hardware)
        {

            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
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
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetSelectedList(), "Id", "Descricao");

            HardwareDeSalaModel hardware = _hardwareService.GetById(id);
            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeSalaModel hardware)
        {
            ViewBag.salas = new SelectList(_salaService.GetSelectedList(), "Id", "Titulo");
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

        public bool CompareMAC(HardwareDeSalaModel hardware)
        {
           
            List<HardwareDeSalaModel> itens = _hardwareService.GetAll().Where(s => s.MAC.Equals(hardware.MAC) && s.Id != hardware.Id).ToList();

            return itens.Count() == 0 ? false : true;
        }


        private List<HardwareDeSalaViewModel> ReturnAllViewModels()
        {
            List<HardwareDeSalaModel> hardwares = _hardwareService.GetAll();
            List<HardwareDeSalaViewModel> hardwaresViewModel = new List<HardwareDeSalaViewModel>();
            foreach (var item in hardwares)
            {
                hardwaresViewModel.Add(Cast(item));
            }

            return hardwaresViewModel;
        }

        private HardwareDeSalaViewModel ReturnByIdViewModel(int id)
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
