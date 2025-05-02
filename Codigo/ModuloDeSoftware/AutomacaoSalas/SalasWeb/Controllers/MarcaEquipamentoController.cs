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
    public class MarcaEquipamentoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMarcaEquipamentoService _marcaEquipamentoService;
        private readonly ILogger<MarcaEquipamentoController> _logger;

        public MarcaEquipamentoController(
            IOrganizacaoService organizacaoService,
            IUsuarioOrganizacaoService usuarioOrganizacaoService,
            IUsuarioService usuarioService,
            IMarcaEquipamentoService marcaEquipamentoService,
            ILogger<MarcaEquipamentoController> logger)
        {
            _organizacaoService = organizacaoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
            _marcaEquipamentoService = marcaEquipamentoService;
            _logger = logger;
        }

        // GET: MarcaEquipamento
        public IActionResult Index()
        {
            return View(ReturnAllViewModels());
        }


        // GET: MarcaEquipamento/Details/5
        public IActionResult Details(uint id)
        {
            try
            {
                MarcaEquipamentoModel marca = _marcaEquipamentoService.GetById(id);
                if (marca == null)
                {
                    return NotFound();
                }
                return View(marca);
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao obter detalhes da marca de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        //GET: MarcaEquipamento/Create
        public ActionResult Create()
        {                    
            return View();
        }

        //POST: MarcaEquipamento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MarcaEquipamentoModel marcaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (_marcaEquipamentoService.Insert(marcaModel))
                    {
                        _logger.LogWarning("Marca de Equipamento adicionado com sucesso!");
                        TempData["mensagemSucesso"] = "Marca de Equipamento adicionado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("Houve um problema ao criar a Marca de Equipamento!");
                        TempData["mensagemErro"] = "Houve um problema ao adicionar uma Marca de Equipamento, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao criar a marca de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }

            return View(marcaModel);

        }

        //GET : MarcaEquipamento/Edit/5
        public ActionResult Edit(uint id)
        {
            try
            {
                MarcaEquipamentoModel marca = _marcaEquipamentoService.GetById(id);
                if (marca == null)
                {
                    return NotFound();
                }
                return View(marca);
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao obter detalhes da marca de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        //POST: MarcaEquipamento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarcaEquipamentoModel marcaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_marcaEquipamentoService.Update(marcaModel))
                    {
                        _logger.LogWarning("Marca de Equipamento editada com sucesso!");
                        TempData["mensagemSucesso"] = "Marca de Equipamento editada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("Houve um problema ao editar a Marca de Equipamento!");
                        TempData["mensagemErro"] = "Houve um problema ao editar a Marca de Equipamento, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao editar a marca de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            return View(marcaModel);
        }

        //GET: MarcaEquipamento/Delete/5
        [HttpGet]
        public IActionResult Delete(uint id)
        {           
            try
            {
                MarcaEquipamentoModel marca = _marcaEquipamentoService.GetById(id);
                return View(marca);
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao obter dados para remoção da Marca do Equipamento: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MarcaEquipamento/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            try
            {
                if (_marcaEquipamentoService.Remove(id))
                {
                    _logger.LogWarning("Marca de equipamento removida com sucesso!");
                    TempData["mensagemSucesso"] = "Marca de Equipamento removida com sucesso!";
                }
                else
                {
                    _logger.LogError("Houve um problema ao tentar remover a Marca de Equipamento!");
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover a Marca de Equipamento!";
                }
            }
            catch (MarcaEquipamentoException ex)
            {
                _logger.LogError("Erro ao remover a Marca de Equipamento: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<MarcaEquipamentoViewModel> ReturnAllViewModels()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var marcas = _marcaEquipamentoService.GetAll();
            var viewModels = new List<MarcaEquipamentoViewModel>();

            // Obtém as organizações do usuário
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(usuarioId);
            var organizacoesDoUsuario = usuarioOrg.Select(uo => uo.OrganizacaoId).ToList();

            // Como MarcaEquipamento não tem relação direta com Organização,
            // podemos incluir todas as marcas para o usuário
            foreach (var marca in marcas)
            {
                viewModels.Add(new MarcaEquipamentoViewModel
                {
                    Id = marca.Id,
                    Nome = marca.Nome,                 
                });
            }

            return viewModels;
        }
    }
}
