using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class SalaController : Controller
    {
        private readonly SalaService _salaService;

        public SalaController(SalaService salaService)
        {
            _salaService = salaService;
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
            return View();
        }

        // POST: Sala/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaModel salaModel)
        {
            try
            {
                if (ModelState.IsValid)
                    if(_salaService.Insert(salaModel))
                        return RedirectToAction(nameof(Index));          
            }
            catch
            {
                return View(salaModel);
            }
            return View(salaModel);
        }

        // GET: Sala/Edit/5
        public ActionResult Edit(int id)
        {
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
                    if (_salaService.Update(salaModel))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(salaModel);
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
                    if (_salaService.Remove(id))
                        return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(salaModel);
            }
            return View(salaModel);
        }
    }
}