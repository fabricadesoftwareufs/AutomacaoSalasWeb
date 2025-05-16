using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Model;
using Model.ViewModel;
using Persistence;
using Service;
using Service.Exceptions;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class EquipamentoController : Controller
    {
        private readonly IMarcaEquipamentoService _marcaEquipamentoService;
        private readonly IModeloEquipamentoService _modeloEquipamentoService;
        private readonly IEquipamentoService _equipamentoService;
        private readonly ICodigoInfravermelhoService _codigoInfravermelhoService;
        private readonly ISalaService _salaService;
        private readonly IOperacaoCodigoService _operacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IHardwareDeSalaService _hardwareDeSalaService;
        private readonly ILogger<EquipamentoController> _logger;

        public EquipamentoController(
            IMarcaEquipamentoService marcaEquipamentoService,
            IModeloEquipamentoService modeloEquipamentoService,
            IEquipamentoService equipamentoService,
            ICodigoInfravermelhoService codigoInfravermelhoService,
            ISalaService salaService,
            IOperacaoCodigoService operacaoService,
            IUsuarioService usuarioService,
            IUsuarioOrganizacaoService usuarioOrganizacaoService,
            IBlocoService blocoService,
            IOrganizacaoService organizacaoService,
            IHardwareDeSalaService hardwareDeSalaService,
            ILogger<EquipamentoController> logger)
        {
            _marcaEquipamentoService = marcaEquipamentoService;
            _modeloEquipamentoService = modeloEquipamentoService;
            _equipamentoService = equipamentoService;
            _codigoInfravermelhoService = codigoInfravermelhoService;
            _salaService = salaService;
            _operacaoService = operacaoService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
            _hardwareDeSalaService = hardwareDeSalaService;
            _logger = logger;
        }


        // GET: EquipamentoController
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: EquipamentoController/Details/5
        public ActionResult Details(int id)
        {
            var equipamentoModel = _equipamentoService.GetByIdEquipamento(id);
            var codigos = _codigoInfravermelhoService.GetAllByEquipamento(equipamentoModel.Id);
            var equipamentoViewModel = new EquipamentoViewModel
            {
                EquipamentoModel = equipamentoModel,
                SalaModel = _salaService.GetById(equipamentoModel.Sala),
                HardwareDeSalaModel = equipamentoModel.HardwareDeSala.HasValue
                    ? _hardwareDeSalaService.GetById(equipamentoModel.HardwareDeSala.Value)
                    : null
            };

            equipamentoViewModel.BlocoModel = _blocoService.GetById(equipamentoViewModel.SalaModel.BlocoId);
            List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
            codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel
            {
                Codigo = c.Codigo,
                Id = c.Id,
                IdEquipamento = (int)c.IdModeloEquipamento,
                IdOperacao = c.IdOperacao,
                Operacao = _operacaoService.GetById(c.IdOperacao).Titulo
            }));
            equipamentoViewModel.Codigos = codigosView;

            return View(equipamentoViewModel);
        }

        // Método para obter modelos por marca que deve ser implementado no seu controller
        [HttpGet]
        public JsonResult GetModelosByMarca(uint id)
        {
            try
            {
                var modelos = _modeloEquipamentoService.GetByMarca(id);
                return Json(modelos.Select(m => new { id = m.Id, nome = m.Nome }));
            }
            catch (Exception ex)
            {
                return Json(new List<object>());
            }
        }

        public ActionResult Create()
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            var hardwares = _hardwareDeSalaService.GetBySalaAndTipoEquipamento((int)salas.First().Id, tiposEquipamento.First());

            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Salas = salas;
            ViewBag.Hardwares = hardwares;
            ViewBag.Tipos = tiposEquipamento;
            return View();
        }

        // POST: EquipamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipamentoViewModel equipamentoViewModel)
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            var hardwares = _hardwareDeSalaService.GetBySalaAndTipoEquipamento((int)salas.First().Id, tiposEquipamento.First());

            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome", equipamentoViewModel.ModeloEquipamento?.MarcaEquipamentoID);

            try
            {
                if (equipamentoViewModel.EquipamentoModel.TipoEquipamento == EquipamentoModel.TIPO_LUZES)
                {
                    if (ModelState.ContainsKey("ModeloEquipamento.MarcaEquipamentoID"))
                        ModelState.Remove("ModeloEquipamento.MarcaEquipamentoID");

                    if (ModelState.ContainsKey("ModeloEquipamento.Id"))
                        ModelState.Remove("ModeloEquipamento.Id");

                    if (ModelState.ContainsKey("ModeloEquipamento.Nome"))
                        ModelState.Remove("ModeloEquipamento.Nome");
                    if (ModelState.ContainsKey("ModeloEquipamento"))
                        ModelState.Remove("ModeloEquipamento");
                    equipamentoViewModel.ModeloEquipamento = null;
                    equipamentoViewModel.EquipamentoModel.IdModeloEquipamento = null;  // Garantindo que para luzes o IdModeloEquipamento é null
                }
                else if (equipamentoViewModel.ModeloEquipamento != null)
                {
                    // Para outros tipos de equipamento, copie o ID do modelo para o EquipamentoModel
                    equipamentoViewModel.EquipamentoModel.IdModeloEquipamento = equipamentoViewModel.ModeloEquipamento.Id;
                }

                if (ModelState.IsValid)
                {
                    // Obtém o ID do usuário autenticado
                    var idUsuario = (uint)usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

                    // Passa o ID do usuário para o serviço de equipamento
                    if (_equipamentoService.Insert(equipamentoViewModel, idUsuario))
                    {
                        TempData["mensagemSucesso"] = "Equipamento cadastrado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir novo equipamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = salas;
            ViewBag.Blocos = blocos;
            ViewBag.Hardwares = hardwares;
            ViewBag.Tipos = tiposEquipamento;

            return View(equipamentoViewModel);
        }

        // GET: EquipamentoController/Edit/5
        public ActionResult Edit(int id)
        {

            var equipamento = _equipamentoService.GetByIdEquipamento(id);
            var sala = _salaService.GetById(equipamento.Sala);
            var bloco = _blocoService.GetById(sala.BlocoId);
            var codigos = _codigoInfravermelhoService.GetAllByEquipamento(equipamento.Id);

            List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
            codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel { Codigo = c.Codigo, Id = c.Id, IdEquipamento = (int)c.IdModeloEquipamento, IdOperacao = c.IdOperacao, Operacao = _operacaoService.GetById(c.IdOperacao).Titulo }));

            EquipamentoViewModel equipamentoViewModel = new EquipamentoViewModel
            {
                EquipamentoModel = equipamento,
                SalaModel = _salaService.GetById(equipamento.Sala),
                BlocoModel = bloco,
                OrganizacaoModel = _organizacaoService.GetById(bloco.OrganizacaoId),
                Codigos = codigosView,

                HardwareDeSalaModel = _hardwareDeSalaService.GetById(equipamento.HardwareDeSala.Value)
            };

            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();

            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Tipos = tiposEquipamento;
            ViewBag.Hardwares = hardwares;

            return View(equipamentoViewModel);
        }

        // POST: EquipamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EquipamentoViewModel equipamentoViewModel)
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            var hardwares = _hardwareDeSalaService.GetAtuadorNotUsed();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Tipos = tiposEquipamento;
            ViewBag.Hardwares = hardwares;

            try
            {
                if (ModelState.IsValid)
                {

                    if (_equipamentoService.Update(equipamentoViewModel))
                    {
                        TempData["mensagemSucesso"] = "Equipamento atualizado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao atualizar o equipamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(equipamentoViewModel);
        }

        // GET: EquipamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EquipamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_equipamentoService.Remove(id))
                    TempData["mensagemSucesso"] = "Equipamento removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ou remover o Equipamento, tente novamente em alguns minutos";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("MacSalaEquipamento/{idSala}/{tipoEquipamento}")]
        public IActionResult MacSalaEquipamento(int idSala, string tipoEquipamento)
        {
            var macs = _hardwareDeSalaService.GetBySalaAndTipoEquipamento(idSala, tipoEquipamento);
            return Json(macs);
        }

        private List<EquipamentoViewModel>  ReturnAllViewModels()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;
            var equipamentosModel = _equipamentoService.GetAll();
            var viewModels = new List<EquipamentoViewModel>();

            // Obtém as organizações do usuário uma única vez
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(usuarioId);
            var organizacoesDoUsuario = usuarioOrg.Select(uo => uo.OrganizacaoId).ToList();

            foreach (var equipamento in equipamentosModel)
            {
                // Obtém a sala e o hardware do equipamento
                var sala = _salaService.GetById(equipamento.Sala);
                var hardware = equipamento.HardwareDeSala.HasValue
                    ? _hardwareDeSalaService.GetById(equipamento.HardwareDeSala.Value)
                    : null;

                // Obtém o bloco da sala
                var bloco = sala != null ? _blocoService.GetById(sala.BlocoId) : null;

                // Verifica se o bloco pertence a uma das organizações do usuário
                if (bloco != null && organizacoesDoUsuario.Contains(bloco.OrganizacaoId))
                {
                    viewModels.Add(new EquipamentoViewModel
                    {
                        EquipamentoModel = equipamento,
                        SalaModel = sala,
                        HardwareDeSalaModel = hardware,
                        BlocoModel = bloco 
                    });
                }
            }

            return viewModels;
        }

    }
}
