using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class BlocoController : Controller
    {
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;
        public BlocoController(IBlocoService blocoService,
                               IOrganizacaoService organizacaoService,
                               IUsuarioOrganizacaoService usuarioOrganizacaoService,
                               IUsuarioService UsuarioService)
        {
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = UsuarioService;
        }

        // GET: Bloco
        public IActionResult Index()
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
                    if (_blocoService.InsertBlocoWithHardware(blocoModel, _usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id))
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
            var usuarioOrg = _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            var organizacoesLotadas = new List<OrganizacaoModel>();
            usuarioOrg.ForEach(uo => organizacoesLotadas.Add(_organizacaoService.GetById(uo.OrganizacaoId)));

            return organizacoesLotadas;
        }

        private List<BlocoViewModel> ReturnAllViewModels()
        {
            var bs = _blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            List<BlocoViewModel> bvm = new List<BlocoViewModel>();
            bs.ForEach(b => bvm.Add(Cast(b)));

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