using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class ReservaSalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        // GET: ReservaSalaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaSalaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservaSalaController/Create
        public ActionResult Create()
        {
            ViewBag.salas = new SelectList(_salaService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id), "Id", "Titulo");
            
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReservaSalaController/Edit/5
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

        // GET: ReservaSalaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReservaSalaController/Delete/5
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
