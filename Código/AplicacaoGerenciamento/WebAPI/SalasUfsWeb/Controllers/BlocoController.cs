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
using Persistence;
using Service;

namespace SalasUfsWeb.Controllers
{
    public class BlocoController : Controller
    {
        private readonly BlocoService _blocoService;
        private readonly OrganizacaoService _organizacaoService;
        private readonly UsuarioOrganizacaoService _usuarioOrganizacaoService;
        public BlocoController(BlocoService blocoService,
                               OrganizacaoService organizacaoService,
                               UsuarioOrganizacaoService usuarioOrganizacaoService)
        {
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
        }

        // GET: Bloco
        public IActionResult Index(string pesquisa)
        {
            return View(ReturnAllViewModels());
        }

        // GET: Bloco/Details/5
        public ActionResult Details(int id)
        {
            BlocoModel blocoModel = _blocoService.GetById(id);
            return View(blocoModel);
        }

        // GET: Bloco/Create
        public ActionResult Create()
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");
            return View();
        }

        // POST: Bloco/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlocoModel blocoModel)
        {
            ViewBag.OrgList = new SelectList(GetOrganizacaos(), "Id", "RazaoSocial");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_blocoService.InsertBlocoWithHardware(blocoModel))
                    {
                        TempData["mensagemSucesso"] = "Bloco adicionado com sucesso!"; return View();
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao adicionar bloco, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(blocoModel);
        }

        // GET: Bloco/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            BlocoModel blocoModel = _blocoService.GetById(id);
            return View(blocoModel);
        }

        // POST: Bloco/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, BlocoModel blocoModel)
        {
            ViewBag.OrgList = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_blocoService.Update(blocoModel))
                        TempData["mensagemSuceso"] = "Bloco atualizado com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar bloco, tente novamente em alguns minutos!";

                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return View(blocoModel);
        }


        // POST: Bloco/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_blocoService.Remove(id))
                    TempData["mensagemSucesso"] = "Bloco Removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover o bloco!";

            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }


        private List<OrganizacaoModel> GetOrganizacaos()
        {
            var idUser = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(idUser);

            var organizacoesLotadas = new List<OrganizacaoModel>();
            usuarioOrg.ForEach(uo => organizacoesLotadas.Add(_organizacaoService.GetById(uo.OrganizacaoId)));

            return organizacoesLotadas;
        }

        private List<BlocoViewModel> ReturnAllViewModels()
        {
            List<BlocoModel> bs = _blocoService.GetAll();
            List<BlocoViewModel> bvm = new List<BlocoViewModel>();
            foreach (var item in bs)
            {
                bvm.Add(Cast(item));
            }

            return bvm;
        }

        private BlocoViewModel Cast(BlocoModel item)
        {
            BlocoViewModel b = new BlocoViewModel();
            b.Id = item.Id;
            b.Titulo = item.Titulo;
            b.OrganizacaoId = _organizacaoService.GetById(item.OrganizacaoId);
            return b;
        }

    }
}