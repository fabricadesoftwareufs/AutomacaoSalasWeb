using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Utils;

namespace SalasUfsWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IPlanejamentoService _planejamentoService;
        private readonly IHorarioSalaService _horarioSalaService;

        public UsuarioController(
                                    IUsuarioService usuarioService,
                                    ITipoUsuarioService tipoUsuarioService,
                                    IOrganizacaoService organizacaoService,
                                    IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                    IPlanejamentoService planejamentoService,
                                    IHorarioSalaService horarioSalaService
                                )
        {
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
            _organizacaoService = organizacaoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _planejamentoService = planejamentoService;
            _horarioSalaService = horarioSalaService;
        }

        // GET: Usuario
        public ActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            
            var usuarioLogado = _usuarioService.GetAuthenticatedUser(claimsIdentity);
            
            var usuarios = _usuarioService.GetAllExceptAuthenticatedUser(usuarioLogado.UsuarioModel.Id);
            
            List<UsuarioAuxModel> lista = new List<UsuarioAuxModel>();

            usuarios.ForEach(s => lista.Add(new UsuarioAuxModel { UsuarioModel = s, TipoUsuarioModel = _tipoUsuarioService.GetById(s.TipoUsuarioId), OrganizacaoModels = _organizacaoService.GetByIdUsuario(s.Id) }));

            return View(lista);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            var usuario = _usuarioService.GetById(id);
            var usuarioView = new UsuarioViewModel { UsuarioModel = usuario, TipoUsuarioModel = _tipoUsuarioService.GetById(usuario.TipoUsuarioId) };
            return View(usuarioView);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UsuarioViewModel usuarioViewModel)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            usuarioViewModel.OrganizacaoModel = _organizacaoService.GetById(usuarioViewModel.OrganizacaoModel.Id);

            if (ModelState.IsValid)
            {
                if (!Methods.ValidarCpf(usuarioViewModel.UsuarioModel.Cpf))
                    return RedirectToAction("Create", "Usuario", new { msg = "invalidCpf" });

                // Criando usuario que será passado para a autenticação.
                var sucesso = new LoginViewModel { Login = usuarioViewModel.UsuarioModel.Cpf, Senha = usuarioViewModel.UsuarioModel.Senha };

                // Informações do objeto
                usuarioViewModel.UsuarioModel.Cpf = Methods.CleanString(usuarioViewModel.UsuarioModel.Cpf);
                usuarioViewModel.UsuarioModel.Senha = Criptography.GeneratePasswordHash(usuarioViewModel.UsuarioModel.Senha);

                try
                {
                    _usuarioService.Insert(usuarioViewModel);
                    TempData["mensagemSucesso"] = "Usuário criado com sucesso!";
                }
                catch (ServiceException se)
                {
                    TempData["mensagemErro"] = se.Message;
                    return View(usuarioViewModel);
                }

                return RedirectToAction("Authenticate", "Login", sucesso);
            }
            // Se nao inserir, vem pra cá e sai.
            /*var erros = ModelState.Values.SelectMany(m => m.Errors)
                                  .Select(e => e.ErrorMessage)
                                  .ToList();
           */
            return View(usuarioViewModel);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");

            var usuario = _usuarioService.GetById(id);
            var usuarioView = new UsuarioViewModel { UsuarioModel = usuario, TipoUsuarioModel = _tipoUsuarioService.GetById(usuario.TipoUsuarioId) };

            return View(usuarioView);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsuarioViewModel usuarioView)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");

            try
            {
                if (ModelState.IsValid)
                {
                    usuarioView.UsuarioModel.TipoUsuarioId = usuarioView.TipoUsuarioModel.Id;
                    if (_usuarioService.Update(usuarioView.UsuarioModel))
                    {
                        TempData["mensagemSucesso"] = "Usuário editado com sucesso!";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao editar o usuário, tente novamente em alguns minutos.";
                        return View(usuarioView);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioView);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_usuarioService.Remove(id))
                    TempData["mensagemSucesso"] = "Usuário removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ou remover usuário, tente novamente em alguns minutos";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public bool HasPlanOrReserv(int idUsuario)
        {
            var plan = _planejamentoService.GetByIdUsuario(idUsuario);
            if (plan != null)
                return true;
            else
            {
                return _horarioSalaService.GetByIdUsuario(idUsuario) != null ? true : false;
            }
        }
    }
}