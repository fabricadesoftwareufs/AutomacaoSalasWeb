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
namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class ModeloEquipamentoController : Controller
    {
        private readonly IMarcaEquipamentoService _marcaEquipamentoService;
        private readonly IModeloEquipamentoService _modeloEquipamentoService;
        private readonly ILogger<ModeloEquipamentoController> _logger;
        private readonly IOperacaoCodigoService _operacaoCodigoService;

        public ModeloEquipamentoController(
            IMarcaEquipamentoService marcaEquipamentoService,
            IModeloEquipamentoService modeloEquipamentoService,
            IOperacaoCodigoService operacaoCodigoService,
            ILogger<ModeloEquipamentoController> logger)
        {
            _marcaEquipamentoService = marcaEquipamentoService;
            _modeloEquipamentoService = modeloEquipamentoService;
            _operacaoCodigoService = operacaoCodigoService;
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
                _logger.LogError("Erro ao obter detalhes do modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
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
                // Adicionando diagnóstico para verificar o que está sendo recebido
                _logger.LogInformation($"Modelo recebido - Nome: {modelo.ModeloEquipamento?.Nome}, MarcaID: {modelo.ModeloEquipamento?.MarcaEquipamentoID}");
                _logger.LogInformation($"Codigos recebidos: {modelo.Codigos?.Count ?? 0}");

                if (modelo.Codigos == null || !modelo.Codigos.Any())
                {
                    _logger.LogWarning("Nenhum código foi recebido");
                    ModelState.AddModelError("", "É necessário adicionar pelo menos um código de operação.");
                    return View(modelo);
                }

                // Imprimir cada código recebido para diagnóstico
                if (modelo.Codigos != null)
                {
                    foreach (var codigo in modelo.Codigos)
                    {
                        _logger.LogInformation($"Código: {codigo.Codigo}, OperacaoId: {codigo.IdOperacao}");
                    }
                }

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
                else
                {
                    // Listar erros de ModelState para diagnóstico
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning($"Erro de validação em {entry.Key}: {error.ErrorMessage}");
                        }
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
            ViewBag.MarcaEquipamento = new SelectList(_marcaEquipamentoService.GetAll(), "Id", "Nome");
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
            }
            catch (ModeloEquipamentoException ex)
            {
                _logger.LogError("Erro ao editar modelo de equipamentos: {0}", ex);
                TempData["mensagemErro"] = ex.Message;
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
