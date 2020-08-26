using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SalasUfsWeb.Controllers
{
    [Authorize(Roles = "GESTOR, ADMIN, CLIENTE")]
    public class ReservaSalaController : Controller
    {
        // private readonly ISalaService _salaService;
        // private readonly IUsuarioService _usuarioService;

        // GET: ReservaSalaController
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaSalaController/Details/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: ReservaSalaController/Create
        public ActionResult Create()
        {
            //    ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo");

            return View();
        }

        // POST: ReservaSalaController/Create
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


        // GET: ReservaSalaController/Edit/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservaSalaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GESTOR, ADMIN")]
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

        // GET: ReservaSalaController/Delete/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservaSalaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GESTOR, ADMIN")]
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
