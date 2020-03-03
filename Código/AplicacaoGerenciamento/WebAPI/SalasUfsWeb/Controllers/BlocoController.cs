using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class BlocoController : Controller
    {
        private readonly BlocoService _blocoService;
        private readonly OrganizacaoService _organizacaoService;
        public BlocoController(BlocoService blocoService, OrganizacaoService organizacaoService)
        {
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
        }

        // GET: Bloco
        public IActionResult Index(string pesquisa)
        {
            var blocos = ReturnAllViewModels();

            if (!string.IsNullOrEmpty(pesquisa))
            {
                blocos = blocos.Where(o => o.Titulo.Contains(pesquisa)).ToList();
            }

            return View(blocos);
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
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
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
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
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

        private List<BlocoViewModel> ReturnAllViewModels()
        {
            List<BlocoModel> bs = _blocoService.GetAll();
            List<BlocoViewModel> bvm = new List<BlocoViewModel>();
            foreach (var item in bs)
            {
                bvm.Add(Cast(item));
            }

            return bvm;
        }

        private BlocoViewModel Cast(BlocoModel item)
        {
            BlocoViewModel b = new BlocoViewModel();
            b.Id = item.Id;
            b.Titulo = item.Titulo;
            b.OrganizacaoId = _organizacaoService.GetById(item.OrganizacaoId);
            return b;
        }

    }
}