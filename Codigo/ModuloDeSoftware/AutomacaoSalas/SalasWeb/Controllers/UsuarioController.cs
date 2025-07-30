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

namespace SalasWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;//por o user manager
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
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var usuarioLogado = _usuarioService.GetAuthenticatedUser(claimsIdentity);
            var usuarios = _usuarioService.GetAllExceptAuthenticatedUser(usuarioLogado.UsuarioModel.Id);

            List<UsuarioAuxModel> lista = new List<UsuarioAuxModel>();

            usuarios.ForEach(s => lista.Add(new UsuarioAuxModel
            {
                UsuarioModel = s,
                TipoUsuarioModel = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(s.Id),
                OrganizacaoModels = _organizacaoService.GetByIdUsuario(s.Id)
            }));

            return View(lista);
        }

        // GET: Usuario/Details/5
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Details(uint id)
        {
            var usuario = _usuarioService.GetById(id);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(id);
            var organizacao = _organizacaoService.GetByIdUsuario(id).FirstOrDefault();

            var usuarioView = new UsuarioViewModel
            {
                UsuarioModel = usuario,
                TipoUsuarioModel = tipoUsuario,
                OrganizacaoModel = organizacao
            };

            return View(usuarioView);
        }

        // GET: Usuario/Create
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Create()
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Create(UsuarioViewModel usuarioViewModel)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        TempData["mensagemErro"] += error.ErrorMessage + " ";
                    }
                }
                return View(usuarioViewModel);
            }

            usuarioViewModel.OrganizacaoModel = _organizacaoService.GetById(usuarioViewModel.OrganizacaoModel.Id);
            if (usuarioViewModel.OrganizacaoModel == null)
            {
                TempData["mensagemErro"] = "Organização não encontrada.";
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarDataNascimento(usuarioViewModel.UsuarioModel.DataNascimento))
            {
                ModelState.AddModelError("UsuarioModel.DataNascimento", "Data de nascimento inválida.");
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarCpf(usuarioViewModel.UsuarioModel.Cpf))
            {
                TempData["mensagemErro"] = "CPF inválido!";
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Nome))
            {
                ModelState.AddModelError("UsuarioModel.Nome", "Nome do usuário não pode ser vazio.");
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Senha) || usuarioViewModel.UsuarioModel.Senha.Length < 8)
            {
                ModelState.AddModelError("UsuarioModel.Senha", "A senha deve ter pelo menos 8 caracteres.");
                return View(usuarioViewModel);
            }

            if (usuarioViewModel.TipoUsuarioModel == null || usuarioViewModel.TipoUsuarioModel.Id <= 0)
            {
                ModelState.AddModelError("TipoUsuarioModel.Id", "Tipo de usuário inválido.");
                return View(usuarioViewModel);
            }

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
            return RedirectToAction("Index", "Usuario");
        }

        // GET: Usuario/Edit/5
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Edit(uint id)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            var usuario = _usuarioService.GetById(id);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(id);
            var organizacao = _organizacaoService.GetByIdUsuario(id);
            var usuarioView = new UsuarioViewModel { UsuarioModel = usuario, TipoUsuarioModel = tipoUsuario, OrganizacaoModel = organizacao.FirstOrDefault() };

            return View(usuarioView);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Edit(int id, UsuarioViewModel usuarioView)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            usuarioView.UsuarioModel.IdOrganizacao = usuarioView.OrganizacaoModel.Id;
            usuarioView.UsuarioModel.IdTipoUsuario = usuarioView.TipoUsuarioModel.Id;

            try
            {
                ModelState.Remove("UsuarioModel.Senha");

                if (ModelState.IsValid)
                {
                    //Pegar a senha atual do banco antes de atualizar
                    var usuarioAtual = _usuarioService.GetById((uint)id);
                    usuarioView.UsuarioModel.Senha = usuarioAtual.Senha;

                    if (_usuarioService.Update(usuarioView.UsuarioModel))
                    {
                        TempData["mensagemSucesso"] = "Usuário editado com sucesso!";
                        return RedirectToAction(nameof(Index));
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
            return View(usuarioView);
        }

        // GET: Usuario/EditPassword
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPassword()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity)?.UsuarioModel?.Id ?? 0;
            var viewModel = new AlterarSenhaViewModel { UsuarioId = usuarioId };
            return View(viewModel);
        }

        // POST: Usuario/EditPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPassword(AlterarSenhaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Buscar o usuário atual
                    var usuarioAtual = _usuarioService.GetById(model.UsuarioId);
                    if (usuarioAtual == null)
                    {
                        TempData["mensagemErro"] = "Usuário não encontrado.";
                        return View(model);
                    }

                    // Verificar se a senha atual está correta
                    var senhaAtualHash = Criptography.GeneratePasswordHash(model.SenhaAtual);
                    if (usuarioAtual.Senha != senhaAtualHash)
                    {
                        ModelState.AddModelError("SenhaAtual", "Senha atual incorreta.");
                        return View(model);
                    }

                    // Atualizar com a nova senha
                    usuarioAtual.Senha = Criptography.GeneratePasswordHash(model.NovaSenha);
                    usuarioAtual.IdOrganizacao = _organizacaoService.GetByIdUsuario(model.UsuarioId).FirstOrDefault()?.Id ?? 0;
                    usuarioAtual.IdTipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(model.UsuarioId)?.Id ?? 0;

                    if (_usuarioService.Update(usuarioAtual))
                    {
                        TempData["mensagemSucesso"] = "Senha alterada com sucesso!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao alterar a senha, tente novamente em alguns minutos.";
                        return View(model);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(model);
            }
            return View(model);
        }

        // GET: Usuario/EditPersonalData
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPersonalData()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity)?.UsuarioModel?.Id ?? 0;

            var usuario = _usuarioService.GetById(usuarioId);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(usuarioId);
            var organizacao = _organizacaoService.GetByIdUsuario(usuarioId);
            var usuarioView = new UsuarioViewModel { UsuarioModel = usuario, TipoUsuarioModel = tipoUsuario, OrganizacaoModel = organizacao.FirstOrDefault() };

            return View(usuarioView);
        }

        // POST: Usuario/EditPersonalData
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPersonalData(int id, UsuarioViewModel usuarioView)
        {
            try
            {
                // Remove a validação da senha para edição de dados pessoais
                ModelState.Remove("UsuarioModel.Senha");
                
                if (ModelState.IsValid)
                {
                    usuarioView.UsuarioModel.IdOrganizacao = usuarioView.OrganizacaoModel.Id;
                    usuarioView.UsuarioModel.IdTipoUsuario = usuarioView.TipoUsuarioModel.Id;
                    usuarioView.UsuarioModel.TipoUsuarioId = usuarioView.TipoUsuarioModel.Id;
                    if (_usuarioService.Update(usuarioView.UsuarioModel))
                    {
                        TempData["mensagemSucesso"] = "Dados pessoais salvos com sucesso!";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao editar seus dados, tente novamente em alguns minutos.";
                        return View(usuarioView);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioView);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: PublicUsuario/Create
        public ActionResult PublicUsuarioCreate()
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            return View();
        }

        // POST: PublicUsuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublicUsuarioCreate(UsuarioViewModel usuarioViewModel)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            //usuarioViewModel.TipoUsuarioModel = new TipoUsuarioModel { Id = 4 };  // Tipo "pendente"

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        TempData["mensagemErro"] += error.ErrorMessage + " ";
                    }
                }
                return View(usuarioViewModel);
            }

            usuarioViewModel.OrganizacaoModel = _organizacaoService.GetById(usuarioViewModel.OrganizacaoModel.Id);
            if (usuarioViewModel.OrganizacaoModel == null)
            {
                TempData["mensagemErro"] = "Organização não encontrada.";
                return View(usuarioViewModel);
            }


            if (!Methods.ValidarDataNascimento(usuarioViewModel.UsuarioModel.DataNascimento))
            {
                ModelState.AddModelError("UsuarioModel.DataNascimento", "Data de nascimento inválida.");
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarCpf(usuarioViewModel.UsuarioModel.Cpf))
            {
                TempData["mensagemErro"] = "CPF inválido!";
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Nome))
            {
                ModelState.AddModelError("UsuarioModel.Nome", "Nome do usuário não pode ser vazio.");
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Senha) || usuarioViewModel.UsuarioModel.Senha.Length < 8)
            {
                ModelState.AddModelError("UsuarioModel.Senha", "A senha deve ter pelo menos 8 caracteres.");
                return View(usuarioViewModel);
            }

            if (usuarioViewModel.TipoUsuarioModel == null || usuarioViewModel.TipoUsuarioModel.Id <= 0)
            {
                ModelState.AddModelError("TipoUsuarioModel.Id", "Tipo de usuário inválido.");
                return View(usuarioViewModel);
            }

            usuarioViewModel.UsuarioModel.Cpf = Methods.CleanString(usuarioViewModel.UsuarioModel.Cpf);
            usuarioViewModel.UsuarioModel.Senha = Criptography.GeneratePasswordHash(usuarioViewModel.UsuarioModel.Senha);

            try
            {
                _usuarioService.Insert(usuarioViewModel);
                TempData["mensagemSucesso"] = "Usuário criado com sucesso!";
                return RedirectToAction("Index", "Usuario");
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioViewModel);
            }
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
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
    }
}