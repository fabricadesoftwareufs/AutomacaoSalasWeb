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
        private readonly ILogger<ConexaoInternetController> logger;

        public ConexaoInternetController(IOrganizacaoService organizacaoService, IConexaoInternetService conexaoInternetService, IBlocoService blocoService, IUsuarioOrganizacaoService usuarioOrganizacaoService, IUsuarioService usuarioService, ILogger<ConexaoInternetController> logger)
        {
            _organizacaoService = organizacaoService;
            _conexaoInternetService = conexaoInternetService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
            this.logger = logger;
        }

        // GET: ConexaoInternet
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: ConexaoInternet/Details/5
        public ActionResult Details(uint id)
        {
            ConexaointernetModel conexao = _conexaoInternetService.GetById(id);
            return View(conexao);
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
                    if (_conexaoInternetService.Insert(conexaoModel))
                    {
                        logger.LogWarning("Conexão de Internet criada com sucesso!");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        logger.LogError("Houve um problema ao criar a conexão de Internet!");
                    }
                }
            }
            catch (ServiceException se)
            {
                logger.LogError("Erro ao cadastrar conexão internet "+ se);
                
            }

            return View(conexaoModel);
        }

        // GET: ConexaoInternet/Edit/5
        public ActionResult Edit(uint id)
        {
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(_blocoService.GetAll(), "Id", "Titulo");
            ConexaointernetModel conexaointernetModel = _conexaoInternetService.GetById(id);        
            return View(conexaointernetModel);
        }

        // POST: ConexaoInternet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConexaointernetModel conexaointernetModel)
        {
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            ViewBag.Blocos = new SelectList(_blocoService.GetAll(), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_conexaoInternetService.Update(conexaointernetModel))
                    {
                        logger.LogWarning("Conexão de Internet editada com sucesso!");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        logger.LogError("Houve um problema ao criar a conexão de Internet!");
                    }
                }
            }
            catch (ServiceException se)
            {
                logger.LogError("Erro ao editar conexão internet " + se);
            }

            
            return View(conexaointernetModel);
        }

        [HttpGet]
        public IActionResult Delete(uint id)
        {
            ViewBag.Blocos = new SelectList(_blocoService.GetAll(), "Id", "Titulo");
            ConexaointernetModel conexaointernetModel = _conexaoInternetService.GetById(id);
            return View(conexaointernetModel);
        }

        // POST: ConexaoInternet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ConexaointernetModel conexaointernetModel)
        {
            try
            {
                h
                if (_conexaoInternetService.Remove(conexaointernetModel.Id))
                {
                    logger.LogWarning("Conexão de Internet removida com sucesso!");
                }                   
                else
                    logger.LogError("Houve um problema ao tentar remover a conexão!");
            }
            catch (ServiceException se)
            {
                logger.LogError("Erro ao remover conexão internet " + se);
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

        private List<OrganizacaoModel> GetOrganizacaos()
        {
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            var organizacoesLotadas = new List<OrganizacaoModel>();
            usuarioOrg.ForEach(uo => organizacoesLotadas.Add(_organizacaoService.GetById(uo.OrganizacaoId)));

            return organizacoesLotadas;
        }
    }
}