using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class SalaController : Controller
    {
        private readonly SalaService _salaService;
        private readonly BlocoService _blocoService;

        public SalaController(SalaService salaService, BlocoService blocoService)
        {
            _salaService = salaService;
            _blocoService = blocoService;
        }
        // GET: Sala
        public ActionResult Index()
        {
            return View(_salaService.GetAll());
        }

        // GET: Sala/Details/5
        public ActionResult Details(int id)
        {
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // GET: Sala/Create
        public ActionResult Create()
        {
            ViewBag.BlocoList = new SelectList(_blocoService.GetAll(), "Id", "Titulo");
            return View();
        }

        // POST: Sala/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaModel salaModel)
        {
            ViewBag.BlocoList = new SelectList(_blocoService.GetAll(), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Insert(salaModel))
                    {
                        TempData["mensagemSucesso"] = "Sala inserida com sucesso!";
                        return View();
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao inserir sala!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return View(salaModel);
        }

        // GET: Sala/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.BlocoList = new SelectList(_blocoService.GetAll(), "Id", "Titulo");
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // POST: Sala/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SalaModel salaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Update(salaModel))
                        TempData["mensagemSucesso"] = "Sala atualizada com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar sala, tente novamente em alguns minutos!";
                }
            }
            catch(ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaModel);
        }

        // GET: Sala/Delete/5
        public ActionResult Delete(int id)
        {
           SalaModel salaModel = _salaService.GetById(id);
           return View(salaModel);
        }

        // POST: Sala/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
            catch(ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}