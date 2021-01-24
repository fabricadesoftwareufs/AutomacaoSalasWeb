using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class EquipamentoController : Controller
    {
        private readonly IEquipamentoService _equipamentoService;
        private readonly ICodigoInfravermelhoService _codigoInfravermelhoService;
        private readonly ISalaService _salaService;

        public EquipamentoController(
                                        IEquipamentoService equipamentoService,
                                        ICodigoInfravermelhoService codigoInfravermelhoService,
                                        ISalaService salaService
                                    )
        {
            _equipamentoService = equipamentoService;
            _codigoInfravermelhoService = codigoInfravermelhoService;
            _salaService = salaService;
        }

        // GET: EquipamentoController
        public ActionResult Index()
        {
            var equipamentosModel = _equipamentoService.GetAll();
            List<EquipamentoViewModel> equipamentos = new List<EquipamentoViewModel>();
            equipamentosModel.ForEach(e => equipamentos.Add(new EquipamentoViewModel { EquipamentoModel = e, SalaModel = _salaService.GetById(e.Sala) }));

            return View(equipamentos);
        }

        // GET: EquipamentoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EquipamentoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EquipamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EquipamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EquipamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
