
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
    public class ReservaSalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IBlocoService _blocoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IHorarioSalaService _horarioSalaService;
        private readonly IHardwareDeSalaService _hardwareDeSalaService;

        public ReservaSalaController(
                                        ISalaService salaService,
                                        IUsuarioService usuarioService,
                                        IBlocoService blocoService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                        IOrganizacaoService organizacaoService,
                                        IHorarioSalaService horarioSalaService,
                                        IHardwareDeSalaService hardwareDeSalaService
                                    )
        {
            _salaService = salaService;
            _usuarioService = usuarioService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _organizacaoService = organizacaoService;
            _horarioSalaService = horarioSalaService;
            _hardwareDeSalaService = hardwareDeSalaService;
        }

        // GET: ReservaSalaController
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Index()
        {
            var reservas = _horarioSalaService.GetAll();
            List<ReservaAuxModel> reservaSalas = new List<ReservaAuxModel>();
            reservas.ForEach(s => reservaSalas.Add(new ReservaAuxModel { HorarioSalaModel = s, UsuarioModel = _usuarioService.GetById(s.UsuarioId), SalaModel = _salaService.GetById(s.SalaId) }));
            return View(reservaSalas);
        }

        // GET: ReservaSalaController/Details/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Details(int id)
        {
            var horarioSala = _horarioSalaService.GetById(id);
            var sala = _salaService.GetById(horarioSala.SalaId);
            var usuario = _usuarioService.GetById(horarioSala.UsuarioId);

            return View(new ReservaAuxModel
            {
                HorarioSalaModel = horarioSala,
                UsuarioModel = usuario,
                SalaModel = sala
            });
        }


        // GET: ReservaSalaController/Create
        public ActionResult Create()
        {

            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id).Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }).ToList();
            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUsuario);
            var usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.organizacoes = new SelectList(organizacoes.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = string.Format("{0} | {1}", s.Cnpj, s.RazaoSocial) }), "Id", "RazaoSocial");
            ViewBag.usuarios = new SelectList(usuarios.Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(salas.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            return View();
        }

        // POST: ReservaSalaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReservaSalaViewModel reservaModel)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUsuario);
            var usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.organizacoes = new SelectList(organizacoes.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = string.Format("{0} | {1}", s.Cnpj, s.RazaoSocial) }), "Id", "RazaoSocial");
            ViewBag.usuarios = new SelectList(usuarios.Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(salas.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_horarioSalaService.Insert(new HorarioSalaModel
                    {
                        HorarioInicio = reservaModel.HorarioSalaModel.HorarioInicio,
                        HorarioFim = reservaModel.HorarioSalaModel.HorarioFim,
                        SalaId = reservaModel.HorarioSalaModel.SalaId,
                        Situacao = HorarioSalaModel.SITUACAO_APROVADA,
                        Data = reservaModel.HorarioSalaModel.Data,
                        Objetivo = reservaModel.HorarioSalaModel.Objetivo,
                        UsuarioId = idUsuario
                    }))
                    {
                        TempData["mensagemSucesso"] = "Reserva feita com sucesso!";
                        var hardwareDeSala = _hardwareDeSalaService.GetByIdSalaAndTipoHardware(reservaModel.HorarioSalaModel.SalaId, TipoHardwareModel.CONTROLADOR_DE_SALA).FirstOrDefault();
                        bool atualizou = _horarioSalaService.SolicitaAtualizacaoHorarioESP(hardwareDeSala.Ip, reservaModel.HorarioSalaModel.Data);
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir nova reserva, tente novamente em alguns minutos.";
                        return View(reservaModel);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(reservaModel);
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: ReservaSalaController/Edit/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Edit(int id)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUsuario);
            var usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.organizacoes = new SelectList(organizacoes.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = string.Format("{0} | {1}", s.Cnpj, s.RazaoSocial) }), "Id", "RazaoSocial");
            ViewBag.usuarios = new SelectList(usuarios.Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(salas.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            var horarioSala = _horarioSalaService.GetById(id);
            var sala = _salaService.GetById(horarioSala.SalaId);
            var bloco = _blocoService.GetById(sala.BlocoId);
            var org = _organizacaoService.GetById(bloco.OrganizacaoId);

            return View(new ReservaSalaViewModel
            {
                HorarioSalaModel = horarioSala,
                BlocoModel = bloco,
                OrganizacaoModel = org
            });
        }

        // POST: ReservaSalaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Edit(ReservaSalaViewModel reservaModel)
        {
            var idUsuario = _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUsuario);
            var usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.organizacoes = new SelectList(organizacoes.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = string.Format("{0} | {1}", s.Cnpj, s.RazaoSocial) }), "Id", "RazaoSocial");
            ViewBag.usuarios = new SelectList(usuarios.Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(salas.Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos.Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_horarioSalaService.Update(new HorarioSalaModel
                    {
                        HorarioInicio = reservaModel.HorarioSalaModel.HorarioInicio,
                        HorarioFim = reservaModel.HorarioSalaModel.HorarioFim,
                        SalaId = reservaModel.HorarioSalaModel.SalaId,
                        Situacao = HorarioSalaModel.SITUACAO_APROVADA,
                        Data = reservaModel.HorarioSalaModel.Data,
                        Objetivo = reservaModel.HorarioSalaModel.Objetivo,
                        UsuarioId = reservaModel.HorarioSalaModel.UsuarioId,
                        Id = reservaModel.HorarioSalaModel.Id
                    }))
                    {
                        TempData["mensagemSucesso"] = "Reserva editada com sucesso!";
                        var hardwareDeSala = _hardwareDeSalaService.GetByIdSalaAndTipoHardware(reservaModel.HorarioSalaModel.SalaId, TipoHardwareModel.CONTROLADOR_DE_SALA).FirstOrDefault();
                        bool atualizou = _horarioSalaService.SolicitaAtualizacaoHorarioESP(hardwareDeSala.Ip, reservaModel.HorarioSalaModel.Data);
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao editar a reserva, tente novamente em alguns minutos.";
                        return View(reservaModel);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(reservaModel);
            }
            return RedirectToAction(nameof(Index));
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
                if (_horarioSalaService.Remove(id))
                    TempData["mensagemSucesso"] = "Reserva de Sala removida com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ao remover a reserva, tente novamente em alguns minutos!";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        public List<BlocoModel> GetBlocosByOrg(int id) => _blocoService.GetByIdOrganizacao(id);
        public List<SalaModel> GetSalasByBloco(int id) => _salaService.GetByIdBloco(id);
    }
}
