
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalasWeb.Controllers
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
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReservaSalaController(
                                        ISalaService salaService,
                                        IUsuarioService usuarioService,
                                        IBlocoService blocoService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                        IOrganizacaoService organizacaoService,
                                        IHorarioSalaService horarioSalaService,
                                        IHardwareDeSalaService hardwareDeSalaService,
                                        IServiceScopeFactory serviceScopeFactory
                                    )
        {
            _salaService = salaService;
            _usuarioService = usuarioService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _organizacaoService = organizacaoService;
            _horarioSalaService = horarioSalaService;
            _hardwareDeSalaService = hardwareDeSalaService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        // GET: ReservaSalaController
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: ReservaSalaController/Details/5
        [Authorize(Roles = "GESTOR, ADMIN")]
        public ActionResult Details(uint id)
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

            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
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
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
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
                    if (reservaModel.HorarioSalaModel.Data < DateTime.Today)
                    {
                        TempData["mensagemErro"] = "N�o � poss�vel reservar uma sala para uma data que j� passou.";
                        return View(reservaModel);
                    }

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
        public ActionResult Edit(uint id)
        {
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUsuario).Select((o) => o.OrganizacaoId).ToList();
            var organizacoes = _organizacaoService.GetInList(usuarioOrg);

            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUsuario);
            var usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.organizacoes = new SelectList(organizacoes.Select(s => new OrganizacaoModel { Id = s.Id, RazaoSocial = s.RazaoSocial }), "Id", "RazaoSocial");
            ViewBag.usuarios = new SelectList(usuarios.Select(s => new UsuarioModel { Id = s.Id, Nome = s.Nome }), "Id", "Nome");
            ViewBag.salas = new SelectList(salas.Select(s => new SalaModel { Id = s.Id, Titulo = s.Titulo }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos.Select(s => new BlocoModel { Id = s.Id, Titulo = s.Titulo }), "Id", "Titulo");


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
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
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
                    if (reservaModel.HorarioSalaModel.Data < DateTime.Today)
                    {
                        TempData["mensagemErro"] = "N�o � poss�vel editar uma reserva para uma data que j� passou.";
                        return View(reservaModel);
                    }

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

        public List<BlocoModel> GetBlocosByOrg(uint id) => _blocoService.GetByIdOrganizacao(id);
        public List<SalaModel> GetSalasByBloco(uint id) => _salaService.GetByIdBloco(id);

        private List<ReservaAuxModel> ReturnAllViewModels()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var reservas = _horarioSalaService.GetAll();
            List<ReservaAuxModel> reservaSalas = new List<ReservaAuxModel>();

            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(usuarioId);
            var organizacoesDoUsuario = usuarioOrg.Select(uo => uo.OrganizacaoId).ToList();

            foreach (var reserva in reservas)
            {
                var sala = _salaService.GetById(reserva.SalaId);

                var bloco = sala != null ? _blocoService.GetById(sala.BlocoId) : null;

                if (bloco != null && organizacoesDoUsuario.Contains(bloco.OrganizacaoId))
                {
                    reservaSalas.Add(new ReservaAuxModel
                    {
                        HorarioSalaModel = reserva,
                        UsuarioModel = _usuarioService.GetById(reserva.UsuarioId),
                        SalaModel = _salaService.GetById(reserva.SalaId)
                    });
                }

            }
            return reservaSalas;
        }

    }
}
