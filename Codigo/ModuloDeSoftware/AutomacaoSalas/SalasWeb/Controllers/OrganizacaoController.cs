﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Para utilizar Regex

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class OrganizacaoController : Controller
    {
        private readonly IOrganizacaoService _organizacaoService;

        public OrganizacaoController(IOrganizacaoService organizacaoService)
        {
            _organizacaoService = organizacaoService;
        }

        // GET: Organizacao
        public IActionResult Index()
        {
            return View(ReturnAllViewModels());
        }

        // GET: Organizacao/Details/5
        public ActionResult Details(uint id)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);
            return View(organizacaoModel);
        }

        // GET: Organizacao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organizacao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrganizacaoModel organizacaoModel)
        {
            try
            {
                organizacaoModel.Cnpj = Regex.Replace(organizacaoModel.Cnpj, @"\D", "");

                if (string.IsNullOrEmpty(organizacaoModel.Cnpj) || organizacaoModel.Cnpj.Length != 14)
                {
                    TempData["mensagemErro"] = "CNPJ deve conter exatamente 14 dígitos.";
                    return View(organizacaoModel);
                }

                if (ModelState.IsValid)
                {
                    if (!Utils.Methods.ValidarCnpj(organizacaoModel.Cnpj))
                    {
                        TempData["mensagemErro"] = "CNPJ inválido!";
                        return View(organizacaoModel);
                    }

                    if (_organizacaoService.Insert(organizacaoModel))
                    {
                        TempData["mensagemSucesso"] = "Organização inserida com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir essa organização, tente novamente em alguns minutos!";
                    }
                }
                return View(organizacaoModel);
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction("Index", "Organizacao");
        }

        // GET: Organizacao/Edit/5
        public ActionResult Edit(uint id)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);
            return View(organizacaoModel);
        }

        // POST: Organizacao/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrganizacaoModel organizacaoModel)
        {
            try
            {
                organizacaoModel.Cnpj = Regex.Replace(organizacaoModel.Cnpj, @"\D", "");

                if (string.IsNullOrEmpty(organizacaoModel.Cnpj) || organizacaoModel.Cnpj.Length != 14)
                {
                    ModelState.AddModelError("Cnpj", "CNPJ deve conter exatamente 14 dígitos.");
                    return View(organizacaoModel);
                }

                if (!Utils.Methods.ValidarCnpj(organizacaoModel.Cnpj))
                {
                    ModelState.AddModelError("Cnpj", "CNPJ inválido.");
                    return View(organizacaoModel);
                }

                if (ModelState.IsValid)
                {
                    if (_organizacaoService.Update(organizacaoModel))
                    {
                        TempData["mensagemSucesso"] = "Organização atualizada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao atualizar essa organização, tente novamente em alguns minutos!";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(organizacaoModel);
        }

        // POST: Organizacao/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            OrganizacaoModel organizacaoModel = _organizacaoService.GetById(id);

            try
            {
                if (_organizacaoService.Remove(id))
                {
                    TempData["mensagemSucesso"] = "Organizacao removida com sucesso!";
                }
                else
                {
                    TempData["mensagemErro"] = "Houve um problema ao remover organizacao, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<OrganizacaoModel> ReturnAllViewModels()
        {
            List<OrganizacaoModel> orgAll = _organizacaoService.GetAll();
            List<OrganizacaoModel> orgSel = new List<OrganizacaoModel>();
            foreach (var item in orgAll)
            {
                orgSel.Add(item);
            }

            return orgSel;
        }
    }
}
