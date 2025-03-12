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
using Microsoft.Extensions.Logging;
using Service.Exceptions;
using Utils;
using System;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ConexaoInternetController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IConexaoInternetService _conexaoInternetService;
        private readonly IBlocoService _blocoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<ConexaoInternetController> _logger;

        public ConexaoInternetController(
            IOrganizacaoService organizacaoService,
            IConexaoInternetService conexaoInternetService,
            IBlocoService blocoService,
            IUsuarioOrganizacaoService usuarioOrganizacaoService,
            IUsuarioService usuarioService,
            ILogger<ConexaoInternetController> logger)
        {
            _organizacaoService = organizacaoService;
            _conexaoInternetService = conexaoInternetService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
            _logger = logger;
        }

        // GET: ConexaoInternet
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: ConexaoInternet/Details/5
        public ActionResult Details(uint id)
        {
            try
            {
                ConexaointernetModel conexao = _conexaoInternetService.GetById(id);
                return View(conexao);
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao obter detalhes da conexão de internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ConexaoInternet/Create
        public ActionResult Create()
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(GetBlocos(), "Id", "Titulo");
            return View();
        }

        // POST: ConexaoInternet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConexaointernetModel conexaoModel)
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(GetBlocos(), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    conexaoModel.Senha = Criptography.Encrypt(conexaoModel.Senha);
                    if (_conexaoInternetService.Insert(conexaoModel))
                    {
                        _logger.LogWarning("Conexão de Internet criada com sucesso!");
                        TempData["mensagemSucesso"] = "Ponto de Acesso adicionado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("Houve um problema ao criar a conexão de Internet!");
                        TempData["mensagemErro"] = "Houve um problema ao adicionar um ponto de conexao, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao cadastrar conexão internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }

            return View(conexaoModel);
        }

        // GET: ConexaoInternet/Edit/5
        public ActionResult Edit(uint id)
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(GetBlocos(), "Id", "Titulo");

            try
            {
                ConexaointernetModel conexao = _conexaoInternetService.GetById(id);
                return View(conexao);
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao obter dados para edição da conexão de internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ConexaoInternet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConexaointernetModel conexaoModel)
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(GetBlocos(), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    conexaoModel.Senha = Criptography.Encrypt(conexaoModel.Senha);

                    if (_conexaoInternetService.Update(conexaoModel))
                    {
                        _logger.LogWarning("Conexão de Internet editada com sucesso!");
                        TempData["mensagemSucesso"] = "Ponto de Acesso atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("Houve um problema ao atualizar a conexão de Internet!");
                        TempData["mensagemErro"] = "Houve um problema ao atualizar o Ponto de Acesso, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao editar conexão internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }

            return View(conexaoModel);
        }

        [HttpGet]
        public IActionResult Delete(uint id)
        {
            ViewBag.Blocos = new SelectList(_blocoService.GetAll(), "Id", "Titulo");

            try
            {
                ConexaointernetModel conexao = _conexaoInternetService.GetById(id);
                return View(conexao);
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao obter dados para remoção da conexão de internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ConexaoInternet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            try
            {
                if (_conexaoInternetService.Remove(id))
                {
                    _logger.LogWarning("Conexão de Internet removida com sucesso!");
                    TempData["mensagemSucesso"] = "Ponto de Acesso removido com sucesso!";
                }
                else
                {
                    _logger.LogError("Houve um problema ao tentar remover a conexão!");
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover o Ponto de Acesso!";
                }
            }
            catch (ConexaoInternetException ex)
            {
                _logger.LogError("Erro ao remover conexão internet: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult DecriptarSenha(string senhaCriptografada)
        {
            // Chamar o método de descriptografia
            string senhaDecriptada = Criptography.Decrypt(senhaCriptografada);

            return Json(senhaDecriptada);
        }

        [HttpPost]
        public JsonResult ValidarSenhaAtual(uint id, string senhaAtual)
        {
            try
            {
                var conexao = _conexaoInternetService.GetById(id);
                // Descriptografar a senha armazenada para comparar com a senha atual informada
                string senhaDecriptada = Criptography.Decrypt(conexao.Senha);

                // Compara a senha descriptografada com a senha atual informada
                return Json(senhaDecriptada == senhaAtual);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao validar senha atual: {0}", ex);
                return Json(false);
            }
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

            // Obtém as organizações do usuário uma única vez
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(usuarioId);
            var organizacoesDoUsuario = usuarioOrg.Select(uo => uo.OrganizacaoId).ToList();

            foreach (var conexao in conexoes)
            {
                // Obtém o bloco específico desta conexão
                var bloco = _blocoService.GetById(conexao.IdBloco);

                // Verifica se o bloco pertence a uma das organizações do usuário
                if (bloco != null && organizacoesDoUsuario.Contains(bloco.OrganizacaoId))
                {
                    viewModels.Add(new ConexaoInternetViewModel
                    {
                        Id = conexao.Id,
                        Nome = conexao.Nome,
                        Senha = conexao.Senha,
                        IdBloco = conexao.IdBloco,
                        Blocos = new List<BlocoViewModel>
                        {
                            new BlocoViewModel
                            {
                                Id = bloco.Id,
                                Titulo = bloco.Titulo
                            }
                        }
                    });
                }
            }

            return viewModels;
        }

        private List<OrganizacaoModel> GetOrganizacaos()
        {
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            var organizacoesLotadas = new List<OrganizacaoModel>();
            usuarioOrg.ForEach(uo => organizacoesLotadas.Add(_organizacaoService.GetById(uo.OrganizacaoId)));

            return organizacoesLotadas;
        }
    }
}