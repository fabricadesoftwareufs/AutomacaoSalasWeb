using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ADMINISTRATIVE_ROLES)]
    public class SalaParticularController : Controller
    {
        private readonly ISalaParticularService _salaParticularService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IBlocoService _blocoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IOrganizacaoService _organizacaoService;


        public SalaParticularController(ISalaService salaService,
                                        ISalaParticularService salaParticularService,
                                        IUsuarioService usuarioService,
                                        IBlocoService blocoService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                        IOrganizacaoService organizacaoService)
        {
            _salaService = salaService;
            _salaParticularService = salaParticularService;
            _usuarioService = usuarioService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _organizacaoService = organizacaoService;
        }


        public ActionResult Index()
        {
            return View(GetAllSalasParticularesViewModel());
        }

        public ActionResult Create()
        {
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaParticularAuxModel salaParticularModel)
        {
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id); ;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(salaParticularModel.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(salaParticularModel.BlocoSalas);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(salaParticularModel.Organizacao);

            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaParticularService.InsertListSalasParticulares(salaParticularModel))
                    {
                        TempData["mensagemSucesso"] = "Sala Exclusiva associada com sucesso!.";
                        return View();
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao inserir novo registro, tente novamente em alguns minutos.";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaParticularModel);
        }

        public ActionResult Edit(uint id)
        {
            var salaExclusivaModel = _salaParticularService.GetById(id);
            var salaModel = _salaService.GetById(salaExclusivaModel.SalaId);
            var idOrg = _blocoService.GetById(salaModel.BlocoId).OrganizacaoId;

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id); ;
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(idOrg);
            ViewBag.Salas = _salaService.GetByIdBloco(salaModel.BlocoId);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(idOrg);

            return View(new SalaParticularAuxModel
            {
                SalaParticular = new SalaParticularModel { Id = salaExclusivaModel.Id, SalaId = salaExclusivaModel.SalaId, UsuarioId = salaExclusivaModel.UsuarioId },
                BlocoSalas = salaModel.BlocoId,
                Organizacao = idOrg,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalaParticularAuxModel salaParticularModel)
        {
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            ViewBag.Usuarios = _usuarioService.GetByIdOrganizacao(salaParticularModel.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(salaParticularModel.BlocoSalas);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(salaParticularModel.Organizacao);

            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaParticularService.Update(new SalaParticularModel { Id = salaParticularModel.SalaParticular.Id, SalaId = salaParticularModel.SalaParticular.SalaId, UsuarioId = salaParticularModel.SalaParticular.UsuarioId }))
                    {
                        TempData["mensagemSucesso"] = "Registro atualizado com sucesso!.";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um probelma ao atualizar registro, tente novamente em alguns minutos!.";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaParticularModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
        {
            SalaModel salaModel = _salaService.GetById(id);
            try
            {
                if (_salaParticularService.Remove(id))
                    TempData["mensagemSucesso"] = "Responsável desassociado com sucesso!.";
                else
                    TempData["mensagemErro"] = "Houve um problema ao desassociar responsável, tente novamente em alguns minutos!.";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Details(uint id)
        {
            SalaParticularModel salaParticular = _salaParticularService.GetById(id);

            return View(new SalaParticularViewModel
            {
                Id = salaParticular.Id,
                SalaId = _salaService.GetById(salaParticular.SalaId),
                Responsavel = _usuarioService.GetById(salaParticular.UsuarioId),
            });
        }

        private List<SalaParticularViewModel> GetAllSalasParticularesViewModel()
        {
            var idUser = uint.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var organizacoesLotadas = _usuarioOrganizacaoService.GetByIdUsuario(idUser).ToList();

            var salasParticularesViewModel = new List<SalaParticularViewModel>();
            organizacoesLotadas.ForEach(p =>
                _salaParticularService.GetByIdOrganizacao(p.OrganizacaoId).ForEach(s =>
                    salasParticularesViewModel.Add(new SalaParticularViewModel
                    {
                        Id = s.Id,
                        SalaId = _salaService.GetById(s.SalaId),
                        Responsavel = _usuarioService.GetById(s.UsuarioId)
                    })
            ));

            return salasParticularesViewModel;
        }
    }
}