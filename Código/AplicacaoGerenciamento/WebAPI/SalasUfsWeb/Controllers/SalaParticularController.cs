using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Model;
using Model.ViewModel;
using Persistence;
using Service;
using Service.Interface;
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


        public SalaParticularController(ISalaService salaService, 
                                        ISalaParticularService salaParticularService,
                                        IUsuarioService usuarioService,
                                        IBlocoService blocoService,
                                        IUsuarioOrganizacaoService usuarioOrganizacaoService)
        {
            _salaService = salaService;
            _salaParticularService = salaParticularService;
            _usuarioService = usuarioService;
            _blocoService = blocoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
        }

        
        public ActionResult Index()
        {
            return View(GetAllSalasParticularesViewModel());
        }

        public ActionResult Create()
        {
            var blocos = _blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }).ToList();
            
            ViewBag.usuarios = new SelectList(GetUsuarios().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(GetSalas(blocos[0].Id).Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(blocos, "Id", "Titulo");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaParticularViewModel salaParticularModel)
        {
            ViewBag.usuarios = new SelectList(GetUsuarios().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(GetSalas(salaParticularModel.BlocoId).Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

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

            ViewBag.usuarios = new SelectList(GetUsuarios().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(GetSalas(salaModel.BlocoId).Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo)}), "Id", "Titulo");
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            return View(new SalaParticularViewModel
            {
                Id = salaExclusivaModel.Id,
                Responsavel = new UsuarioModel { Id = salaExclusivaModel.UsuarioId },
                SalaId = salaModel,
                BlocoId = salaModel.BlocoId
                
            });
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalaParticularViewModel salaParticularModel)
        {
            ViewBag.usuarios = new SelectList(GetUsuarios().Select(s => new UsuarioModel { Id = s.Id, Nome = string.Format("{0} | {1}", s.Cpf, s.Nome) }), "Id", "Nome");
            ViewBag.salas = new SelectList(GetSalas(salaParticularModel.BlocoId).Select(s => new SalaModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");
            ViewBag.blocos = new SelectList(_blocoService.GetAllByIdUsuarioOrganizacao(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).Select(s => new BlocoModel { Id = s.Id, Titulo = string.Format("{0} | {1}", s.Id, s.Titulo) }), "Id", "Titulo");

            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaParticularService.Update(new SalaParticularModel { Id = salaParticularModel.Id, SalaId = salaParticularModel.SalaId.Id, UsuarioId = salaParticularModel.Responsavel.Id}))
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


        private List<SalaParticularViewModel> GetAllSalasParticularesViewModel() 
        {
            var salasParticulares = _salaParticularService.GetAll();
            List<SalaParticularViewModel> salasParticularesViewModel = new List<SalaParticularViewModel>();
            
            foreach (var item in salasParticulares) 
            {
                salasParticularesViewModel.Add(new SalaParticularViewModel
                {
                    Id = item.Id,
                    SalaId = _salaService.GetById(item.SalaId),
                    Responsavel = _usuarioService.GetById(item.UsuarioId)
                }); 
            
            }

            return salasParticularesViewModel;
        }


        public List<UsuarioModel> GetUsuarios() 
        {
            var idUser = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var organizacoesLotadas = _usuarioOrganizacaoService.GetByIdUsuario(idUser).ToList();

            var usuarios = new List<UsuarioModel>();
            organizacoesLotadas.ForEach(org => usuarios.AddRange(_usuarioService.GetByIdOrganizacao(org.OrganizacaoId)));

            return usuarios;
        }

        public List<SalaModel> GetSalas(int idBloco)
        {
            return _salaService.GetByIdBloco(idBloco);
        }
    }
}