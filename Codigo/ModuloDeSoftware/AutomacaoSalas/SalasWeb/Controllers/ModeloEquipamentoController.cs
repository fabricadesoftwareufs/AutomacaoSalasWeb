using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.ViewModel;
using Service.Interface;
using System.Collections.Generic;
using Service.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;
using Service;
namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ModeloEquipamentoController : Controller
    {
        private readonly IMarcaEquipamentoService _marcaEquipamentoService;
        private readonly IModeloEquipamentoService _modeloEquipamentoService;
        private readonly ILogger<ModeloEquipamentoController> _logger;
        private readonly IOperacaoCodigoService _operacaoCodigoService;
        private readonly ICodigoInfravermelhoService _codigoInfravermelhoService;

        public ModeloEquipamentoController(
            IMarcaEquipamentoService marcaEquipamentoService,
            IModeloEquipamentoService modeloEquipamentoService,
            IOperacaoCodigoService operacaoCodigoService,
            ICodigoInfravermelhoService codigoInfravermelhoService,
            ILogger<ModeloEquipamentoController> logger)
        {
            _marcaEquipamentoService = marcaEquipamentoService;
            _modeloEquipamentoService = modeloEquipamentoService;
            _operacaoCodigoService = operacaoCodigoService;
            _codigoInfravermelhoService = codigoInfravermelhoService;
            _logger = logger;
        }

        // GET: ModeloEquipamentoController
        public ActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: ModeloEquipamentoController/Details/5
        public ActionResult Details(uint id)
        {
            var modeloEquipamentoModel = _modeloEquipamentoService.GetById(id);
            modeloEquipamentoModel.Marca = _marcaEquipamentoService.GetById(modeloEquipamentoModel.MarcaEquipamentoID);
            var marca = _marcaEquipamentoService.GetAll();
            var codigos = _codigoInfravermelhoService.GetAllByEquipamento((int)modeloEquipamentoModel.Id);
            var modeloEquipamentoViewModel = new ModeloEquipamentoViewModel
            {
                ModeloEquipamento = modeloEquipamentoModel,
            };
            List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
            codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel
            {
                Codigo = c.Codigo,
                Id = c.Id,
                IdEquipamento = (int)c.IdModeloEquipamento,
                IdOperacao = c.IdOperacao,
                Operacao = _operacaoCodigoService.GetById(c.IdOperacao).Titulo
            }));
            List<MarcaEquipamentoViewModel> marcasView = new List<MarcaEquipamentoViewModel>();
            marca.ForEach(m => marcasView.Add(new MarcaEquipamentoViewModel
            {
                Id = m.Id,
                Nome = m.Nome
            }));
            modeloEquipamentoViewModel.Marcas = marcasView;
            modeloEquipamentoViewModel.Codigos = codigosView;
            return View(modeloEquipamentoViewModel);
        }


        // GET: ModeloEquipamentoController/Create
        public ActionResult Create()
        {
            var operacoes = _operacaoCodigoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");
            return View();
        }

        // POST: ModeloEquipamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModeloEquipamentoViewModel modelo)
        {
            var operacoes = _operacaoCodigoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");
            try
            {
                if (ModelState.IsValid)
                {
                    if (_modeloEquipamentoService.Insert(modelo))
                    {
                        _logger.LogWarning("Modelo de Equipamento adicionado com sucesso!");
                        TempData["mensagemSucesso"] = "Modelo de Equipamento adicionado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning("Modelo de Equipamento não adicionado!");
                        TempData["mensagemErro"] = "Modelo de Equipamento não adicionado!";
                    }
                }
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao adicionar modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado: {0}", ex);
                TempData["mensagemErro"] = "Ocorreu um erro inesperado ao adicionar o modelo.";
            }
            return View(modelo);
        }

        // GET: ModeloEquipamentoController/Edit/5
        public ActionResult Edit(uint id)
        {
            var operacoes = _operacaoCodigoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");

            try
            {
                ModeloEquipamentoModel modelo = _modeloEquipamentoService.GetById(id);
                if (modelo == null)
                {
                    return NotFound();
                }

                modelo.Marca = _marcaEquipamentoService.GetById(modelo.MarcaEquipamentoID);

                var codigos = _codigoInfravermelhoService.GetAllByEquipamento((int)modelo.Id);

                var modeloEquipamentoViewModel = new ModeloEquipamentoViewModel
                {
                    ModeloEquipamento = modelo
                };

                List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
                codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel
                {
                    Codigo = c.Codigo,
                    Id = c.Id,
                    IdEquipamento = (int)c.IdModeloEquipamento,
                    IdOperacao = c.IdOperacao,
                    Operacao = _operacaoCodigoService.GetById(c.IdOperacao).Titulo
                }));

                modeloEquipamentoViewModel.Codigos = codigosView;

                return View(modeloEquipamentoViewModel);
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao obter dados para edição do modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ModeloEquipamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModeloEquipamentoViewModel modelo)
        {
            var operacoes = _operacaoCodigoService.GetAll().ToList();
            ViewBag.Operacoes = operacoes;
            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_modeloEquipamentoService.Update(modelo))
                    {
                        _logger.LogWarning("Modelo de Equipamento editado com sucesso!");
                        TempData["mensagemSucesso"] = "Modelo de Equipamento editado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning("Modelo de Equipamento não editado!");
                        TempData["mensagemErro"] = "Modelo de Equipamento não editado!";
                    }
                }
                else
                {
                    if (modelo.ModeloEquipamento != null && modelo.ModeloEquipamento.Id > 0)
                    {
                        var codigos = _codigoInfravermelhoService.GetAllByEquipamento((int)modelo.ModeloEquipamento.Id);
                        List<CodigoInfravermelhoViewModel> codigosView = new List<CodigoInfravermelhoViewModel>();
                        codigos.ForEach(c => codigosView.Add(new CodigoInfravermelhoViewModel
                        {
                            Codigo = c.Codigo,
                            Id = c.Id,
                            IdEquipamento = (int)c.IdModeloEquipamento,
                            IdOperacao = c.IdOperacao,
                            Operacao = _operacaoCodigoService.GetById(c.IdOperacao).Titulo
                        }));
                        modelo.Codigos = codigosView;
                    }
                }
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao editar modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro inesperado ao editar modelo de equipamento: {0}", ex);
                TempData["mensagemErro"] = "Ocorreu um erro inesperado ao editar o modelo.";
            }

            return View(modelo);
        }

        // GET: ModeloEquipamentoController/Delete/5
        public ActionResult Delete(uint id)
        {
            try
            {
                ModeloEquipamentoModel modelo = _modeloEquipamentoService.GetById(id);
                if (modelo == null)
                {
                    return NotFound();
                }
                // Carrega a marca associada ao modelo
                modelo.Marca = _marcaEquipamentoService.GetById(modelo.MarcaEquipamentoID);
                return View(modelo);
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao obter dados para exclusão do modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ModeloEquipamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            try
            {
                if (_modeloEquipamentoService.Remove(id))
                {
                    _logger.LogWarning("Modelo de Equipamento removido com sucesso!");
                    TempData["mensagemSucesso"] = "Modelo de Equipamento removido com sucesso!";
                }
                else
                {
                    _logger.LogWarning("Modelo de Equipamento não removido!");
                    TempData["mensagemErro"] = "Modelo de Equipamento não removido!";
                }
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao remover modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<ModeloEquipamentoViewModel> ReturnAllViewModels()
        {
            var modelos = _modeloEquipamentoService.GetAll();
            var modelosViewModel = new List<ModeloEquipamentoViewModel>();
            foreach (var modelo in modelos)
            {
                var marca = _marcaEquipamentoService.GetById(modelo.MarcaEquipamentoID);

                // Atribuir a marca ao modelo
                modelo.Marca = marca; // Adicione esta linha

                modelosViewModel.Add(new ModeloEquipamentoViewModel
                {
                    ModeloEquipamento = modelo,
                    Marcas = new List<MarcaEquipamentoViewModel>
                    {
                        new MarcaEquipamentoViewModel
                        {
                            Id = modelo.MarcaEquipamentoID,
                            Nome = marca.Nome
                        }
                    }
                });
            }
            return modelosViewModel;
        }

    }
}
