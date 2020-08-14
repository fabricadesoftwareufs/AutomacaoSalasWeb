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
using Service.Interface;
using Utils;

namespace SalasUfsWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly IOrganizacaoService _organizacaoService;
        public UsuarioController(IUsuarioService usuarioService, ITipoUsuarioService tipoUsuarioService, IOrganizacaoService organizacaoService)
        {
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
            _organizacaoService = organizacaoService;
        }
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            return View();
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}