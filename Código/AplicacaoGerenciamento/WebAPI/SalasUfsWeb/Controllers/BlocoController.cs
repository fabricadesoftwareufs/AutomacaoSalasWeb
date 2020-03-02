using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class BlocoController : Controller
    {
        private readonly BlocoService _blocoService;

        public BlocoController(BlocoService blocoService)
        {
            _blocoService = blocoService;
        }

        // GET: Bloco
        public ActionResult Index()
        {
            return View(_blocoService.GetAll());
        }

        // GET: Bloco/Details/5
        public ActionResult Details(int id)
        {
            BlocoModel blocoModel = _blocoService.GetById(id);
            return View(blocoModel);
        }

        // GET: Bloco/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bloco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlocoModel blocoModel)
        {
            try
            {
                if (ModelState.IsValid)
                    if(_blocoService.Insert(blocoModel))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(blocoModel);
            }
            return View(blocoModel);
        }

        // GET: Bloco/Edit/5
        public ActionResult Edit(int id)
        {
            BlocoModel blocoModel = _blocoService.GetById(id);
            return View(blocoModel);
        }

        // POST: Bloco/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BlocoModel blocoModel)
        {
            try
            {
                if (ModelState.IsValid)
                    if (_blocoService.Update(blocoModel))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(blocoModel);
            }
            return View(blocoModel);
        }

        // GET: Bloco/Delete/5
        public ActionResult Delete(int id)
        {
            BlocoModel blocoModel = _blocoService.GetById(id);
            return View(blocoModel);
        }

        // POST: Bloco/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            BlocoModel blocoModel = _blocoService.GetById(id);
            try
            {
                if (_blocoService.Remove(id))
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(blocoModel);
            }
            return View(blocoModel);
        }
    }
}