using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.AuxModel;
using Model.ViewModel;
using SalasUfsWeb.Models;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISalaParticularService _salaParticularService;
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IUsuarioService _usuarioService;

        public HomeController(ISalaParticularService salaParticularService,
                              ISalaService salaService,
                              IBlocoService blocoService,
                              IUsuarioService usuarioService)
        {
            _salaService = salaService;
            _salaParticularService = salaParticularService;
            _blocoService = blocoService;
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View(GetSalasUsuario());
        }

        public IActionResult MonitorarSala(MonitorarSalaAuxModel monitorar)
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CancelarReserva(MonitorarSalaAuxModel monitorar) 
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public SalaUsuarioViewModel GetSalasUsuario() 
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);

            var salas = new SalaUsuarioViewModel();
            foreach (var item in _salaParticularService.GetByIdUsuario(usuario.Id))
            {
                var sala = _salaService.GetById(item.SalaId);
                var bloco = _blocoService.GetById(sala.BlocoId);
                salas.SalasExclusivas.Add(new SalaUsuarioAuxModel 
                { 
                    SalaExclusiva = item,
                    Sala = sala,
                    Bloco = bloco
                });
            }

            return salas;
        }
    }
}
