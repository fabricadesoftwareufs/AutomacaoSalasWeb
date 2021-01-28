using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{
    public class EquipamentoController : Controller
    {
        private readonly IEquipamentoService _equipamentoService;
        private readonly ICodigoInfravermelhoService _codigoInfravermelhoService;
        private readonly ISalaService _salaService;
        private readonly IOperacaoCodigoService _operacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;
        public EquipamentoController(
                                        IEquipamentoService equipamentoService,
                                        ICodigoInfravermelhoService codigoInfravermelhoService,
                                        ISalaService salaService,
                                        IOperacaoCodigoService operacaoService,
                                        IUsuarioService usuarioService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                        IBlocoService blocoService,
                                        IOrganizacaoService organizacaoService
                                    )
        {
            _equipamentoService = equipamentoService;
            _codigoInfravermelhoService = codigoInfravermelhoService;
            _salaService = salaService;
            _operacaoService = operacaoService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
        }

        // GET: EquipamentoController
        public ActionResult Index()
        {
            var equipamentosModel = _equipamentoService.GetAll();
            List<EquipamentoViewModel> equipamentos = new List<EquipamentoViewModel>();
            equipamentosModel.ForEach(e => equipamentos.Add(new EquipamentoViewModel { EquipamentoModel = e, SalaModel = _salaService.GetById(e.Sala) }));

            return View(equipamentos);
        }

        // GET: EquipamentoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EquipamentoController/Create
        public ActionResult Create()
        {
            string[] tiposEquipamento = { EquipamentoModel.TIPO_CONDICIONADOR, EquipamentoModel.TIPO_LUZES };

            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.Tipos = tiposEquipamento;
            return View();
        }

        // POST: EquipamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipamentoViewModel equipamentoViewModel)
        {
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            var operacoes = _operacaoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            try
            {
                if (ModelState.IsValid)
                {
                  
                    if (_equipamentoService.Insert(equipamentoViewModel))
                    {
                        TempData["mensagemSucesso"] = "Equipamento cadastrado com sucesso!";
                        return View();
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir novo equipamento, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(equipamentoViewModel);
}

        // GET: EquipamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EquipamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
