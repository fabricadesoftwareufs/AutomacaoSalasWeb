using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.AuxModel;
using Service;
using Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
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
            var organizacoes = GetOrganizacoes();
            var blocos = GetBlocosByIdOrganizacao(organizacoes[0].Id);

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Usuarios     = GetUsuariosByIdOrganizacao(organizacoes[0].Id);
            ViewBag.Salas        = GetSalasByIdBloco(blocos[0].Id);
            ViewBag.Blocos       = blocos;

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaParticularAuxModel salaParticularModel)
        {
            ViewBag.Organizacoes = GetOrganizacoes();
            ViewBag.Usuarios     = GetUsuariosByIdOrganizacao(salaParticularModel.Organizacao);
            ViewBag.Salas        = GetSalasByIdBloco(salaParticularModel.BlocoSalas);
            ViewBag.Blocos       = GetBlocosByIdOrganizacao(salaParticularModel.Organizacao);   

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
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir novo registro, tente novamente em alguns minutos.";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            for (var i = 0; i < salaParticularModel.Responsaveis.Count; i++)
                salaParticularModel.Responsaveis[i] = _usuarioService.GetById(salaParticularModel.Responsaveis[i].Id); 

            return View(salaParticularModel);
        }

        public ActionResult Edit(int id)
        {
            var salaExclusivaModel = _salaParticularService.GetById(id);
            var salaModel = _salaService.GetById(salaExclusivaModel.SalaId);
            var idOrg = _blocoService.GetById(salaModel.BlocoId).OrganizacaoId;

            ViewBag.Organizacoes = GetOrganizacoes();
            ViewBag.Usuarios     = GetUsuariosByIdOrganizacao(idOrg);
            ViewBag.Salas        = GetSalasByIdBloco(salaModel.BlocoId);
            ViewBag.Blocos       = GetBlocosByIdOrganizacao(idOrg);
           
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
            ViewBag.Organizacoes = GetOrganizacoes();
            ViewBag.Usuarios     = GetUsuariosByIdOrganizacao(salaParticularModel.Organizacao);
            ViewBag.Salas        = GetSalasByIdBloco(salaParticularModel.BlocoSalas);
            ViewBag.Blocos       = GetBlocosByIdOrganizacao(salaParticularModel.Organizacao);
           
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaParticularService.Update(new SalaParticularModel { Id = salaParticularModel.SalaParticular.Id, SalaId = salaParticularModel.SalaParticular.SalaId, UsuarioId = salaParticularModel.SalaParticular.UsuarioId}))
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
        public ActionResult Delete(int id, IFormCollection collection)
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

        public ActionResult Details(int id)
        {
            SalaParticularModel salaParticular = _salaParticularService.GetById(id);

            return View(new SalaParticularViewModel
            {
                Id = salaParticular.Id,
                SalaId = _salaService.GetById(salaParticular.SalaId),
                Responsavel = _usuarioService.GetById(salaParticular.UsuarioId),
            });
        }

        private List<OrganizacaoModel> GetOrganizacoes() {

            var organizacoes = new List<OrganizacaoModel>();
            _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).
                ForEach(ex => organizacoes.Add(_organizacaoService.GetById(ex.OrganizacaoId)));
            
            return organizacoes;
        }

        private List<SalaParticularViewModel> GetAllSalasParticularesViewModel() 
        {
            var idUser = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
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

        public List<UsuarioModel> GetUsuariosByIdOrganizacao(int idOrganizacao)
        {
            return _usuarioService.GetByIdOrganizacao(idOrganizacao);
        }

        public List<BlocoModel> GetBlocosByIdOrganizacao(int idOrganizacao)
        {
            return _blocoService.GetByIdOrganizacao(idOrganizacao);
        }

        public List<SalaModel> GetSalasByIdBloco(int idBloco)
        {
            return _salaService.GetByIdBloco(idBloco);
        }
    }
}