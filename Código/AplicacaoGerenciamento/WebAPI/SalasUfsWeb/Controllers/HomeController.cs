using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using SalasUfsWeb.Models;
using Service;
using Service.Interface;
using System.Diagnostics;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
    [Authorize(Roles = "GESTOR, ADMIN, CLIENTE")]
    public class HomeController : Controller
    {
        private readonly ISalaParticularService _salaParticularService;
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IHorarioSalaService _horarioSalaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMonitoramentoService _monitoramentoService;


        public HomeController(ISalaParticularService salaParticularService,
                              ISalaService salaService,
                              IBlocoService blocoService,
                              IUsuarioService usuarioService,
                              IMonitoramentoService monitoramentoService,
                              IHorarioSalaService horarioSalaService)
        {
            _salaService = salaService;
            _salaParticularService = salaParticularService;
            _blocoService = blocoService;
            _monitoramentoService = monitoramentoService;
            _usuarioService = usuarioService;
            _horarioSalaService = horarioSalaService;
        }

        public IActionResult Index()
        {
            return View(GetSalasUsuario());
        }

        public IActionResult MonitorarSala(MonitoramentoModel monitoramento)
        {
            try
            {
                if (!_monitoramentoService.MonitorarSala(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id,monitoramento))
                    TempData["mensagemErro"] = "Não foi possível atender a sua solicitacao, tente novamente!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult CancelarReserva(int idReserva)
        {
            try
            {
                if (_horarioSalaService.ConcelarReserva(idReserva)) TempData["mensagemSucesso"] = "Reserva cancelada com sucesso!";
                else TempData["mensagemErro"] = "Houve um problema ao cancelar sua reserva, tente novamente em alguns minutOs!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

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
            foreach (var item in _salaParticularService.GetByIdUsuario(usuario.UsuarioModel.Id))
            {
                var sala = _salaService.GetById(item.SalaId);
                var bloco = _blocoService.GetById(sala.BlocoId);
                var monitoramento = _monitoramentoService.GetByIdSala(item.SalaId);

                salas.SalasUsuario.Add(new SalaUsuarioAuxModel
                {
                    SalaExclusiva = item,
                    Sala = sala,
                    Bloco = bloco,
                    Monitoramento = monitoramento,
                });
            }

            return salas;
        }

        public SalaUsuarioViewModel GetReservasUsuario(string diaSemana)
        {
            var usuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity);
            var salas = new SalaUsuarioViewModel();

            foreach (var item in _horarioSalaService.GetProximasReservasByIdUsuarioAndDiaSemana(usuario.UsuarioModel.Id, diaSemana))
            {
                var sala = _salaService.GetById(item.SalaId);
                var bloco = _blocoService.GetById(sala.BlocoId);
                var monitoramento = _monitoramentoService.GetByIdSala(item.SalaId);

                salas.SalasUsuario.Add(new SalaUsuarioAuxModel
                {
                    HorarioSala = item,
                    Sala = sala,
                    Bloco = bloco,
                    Monitoramento = monitoramento,
                });
            }

            return salas;
        }

    }
}
