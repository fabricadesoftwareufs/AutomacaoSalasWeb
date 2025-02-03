using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ConexaoInternetController : Controller
    {
        private readonly IConexaoInternetService _conexaoInternetService;
        private readonly IBlocoService _blocoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;

        public ConexaoInternetController(
            IConexaoInternetService conexaoInternetService,
            IBlocoService blocoService,
            IUsuarioOrganizacaoService usuarioOrganizacaoService,
            IUsuarioService usuarioService)
        {
            _conexaoInternetService = conexaoInternetService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
        }

        // GET: ConexaoInternet
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: ConexaoInternet/Details/5
        public ActionResult Details(uint id)
        {
            var conexao = _conexaoInternetService.GetById(id);
            return View(conexao);
        }

        // GET: ConexaoInternet/Create
        public ActionResult Create()
        {
            var viewModel = new ConexaoInternetViewModel
            {
                Blocos = GetBlocos()
            };
            return View(viewModel);
        }

        // POST: ConexaoInternet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConexaoInternetViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = new ConexaointernetModel
                    {
                        Nome = viewModel.Nome,
                        Senha = viewModel.Senha,
                        ConfirmarSenha = viewModel.ConfirmarSenha,
                        IdBloco = viewModel.IdBloco
                    };

                    if (_conexaoInternetService.Insert(model))
                    {
                        TempData["mensagemSucesso"] = "Conexão de Internet adicionada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao adicionar a conexão, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            viewModel.Blocos = GetBlocos();
            return View(viewModel);
        }

        // GET: ConexaoInternet/Edit/5
        public ActionResult Edit(uint id)
        {
            var conexao = _conexaoInternetService.GetById(id);
            var viewModel = new ConexaoInternetViewModel
            {
                Id = conexao.Id,
                Nome = conexao.Nome,
                IdBloco = conexao.IdBloco,
                Blocos = GetBlocos()
            };
            return View(viewModel);
        }

        // POST: ConexaoInternet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConexaoInternetViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = new ConexaointernetModel
                    {
                        Id = viewModel.Id,
                        Nome = viewModel.Nome,
                        Senha = viewModel.Senha,
                        ConfirmarSenha = viewModel.ConfirmarSenha,
                        IdBloco = viewModel.IdBloco
                    };

                    if (_conexaoInternetService.Update(model))
                    {
                        TempData["mensagemSucesso"] = "Conexão de Internet atualizada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao atualizar a conexão, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            viewModel.Blocos = GetBlocos();
            return View(viewModel);
        }

        // POST: ConexaoInternet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id)
        {
            try
            {
                if (_conexaoInternetService.Remove(id))
                    TempData["mensagemSucesso"] = "Conexão de Internet removida com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover a conexão!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<BlocoViewModel> GetBlocos()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

            // Obtém os blocos das organizações do usuário
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(usuarioId);
            var blocos = new List<BlocoViewModel>();

            foreach (var org in usuarioOrg)
            {
                var blocosDaOrg = _blocoService.GetByIdOrganizacao(org.OrganizacaoId);
                blocos.AddRange(blocosDaOrg.Select(b => new BlocoViewModel
                {
                    Id = b.Id,
                    Titulo = b.Titulo
                }));
            }

            return blocos;
        }

        private List<ConexaoInternetViewModel> ReturnAllViewModels()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

            var conexoes = _conexaoInternetService.GetAll();
            var viewModels = new List<ConexaoInternetViewModel>();

            foreach (var conexao in conexoes)
            {
                viewModels.Add(new ConexaoInternetViewModel
                {
                    Id = conexao.Id,
                    Nome = conexao.Nome,
                    Senha = conexao.Senha,
                    IdBloco = conexao.IdBloco,
                    Blocos = GetBlocos()
                });
            }

            return viewModels;
        }
    }
}