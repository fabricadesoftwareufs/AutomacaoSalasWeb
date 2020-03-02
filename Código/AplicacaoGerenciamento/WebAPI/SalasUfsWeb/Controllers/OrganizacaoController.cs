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
    public class OrganizacaoController : Controller
    {
        private readonly OrganizacaoService _organizacaoService;

        public OrganizacaoController(OrganizacaoService organizacaoService)
        {
            _organizacaoService = organizacaoService;
        }
        // GET: Organizacao
        public ActionResult Index()
        {
            return View(_organizacaoService.GetAll());
        }

        // GET: Organizacao/Details/5
        public ActionResult Details(int id)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);
            return View(organizacaoModel);
        }

        // GET: Organizacao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organizacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizacaoModel organizacaoModel)
        {
            try
            {
                if(ModelState.IsValid)
                    if(_organizacaoService.Insert(organizacaoModel))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(organizacaoModel);
            }
            return View(organizacaoModel);
        }

        // GET: Organizacao/Edit/5
        public ActionResult Edit(int id)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);
            return View(organizacaoModel);
        }

        // POST: Organizacao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrganizacaoModel organizacaoModel)
        {
            try
            {
                if (ModelState.IsValid)
                    if (_organizacaoService.Update(organizacaoModel))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(organizacaoModel);
            }
            return View(organizacaoModel);
        }

        // GET: Organizacao/Delete/5
        public ActionResult Delete(int id)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);
            return View(organizacaoModel);
        }

        // POST: Organizacao/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);

            try
            {
               if(_organizacaoService.Remove(id))
                    return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(organizacaoModel);
            }
            return View(organizacaoModel);
        }
    }
}