using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Persistence;
using Service;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasUfsWeb.Controllers
{
    public class SalaController : Controller
    {
        private readonly SalaService _salaService;
        private readonly BlocoService _blocoService;
        private readonly HardwareDeSalaService _hardwareDeSalaService;
        private readonly TipoHardwareService _tipoHardwareService;
        private readonly UsuarioOrganizacaoService _usuarioOrganizacaoService;


        public SalaController(SalaService salaService,
                              BlocoService blocoService,
                              HardwareDeSalaService hardwareDeSalaService,
                              TipoHardwareService tipoHardwareService,
                              UsuarioOrganizacaoService usuarioOrganizacaoService)
        {
            _salaService = salaService;
            _blocoService = blocoService;
            _hardwareDeSalaService = hardwareDeSalaService;
            _tipoHardwareService = tipoHardwareService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
        }
        // GET: Sala
        public ActionResult Index()
        {
            return View(GetAllSalasViewModel());
        }

        // GET: Sala/Details/5
        public ActionResult Details(int id)
        {
            return View(GetSalaViewModel(id));
        }

        // GET: Sala/Create
        public ActionResult Create()
        {
            ViewBag.BlocoList = new SelectList(GetBlocos(), "Id", "Titulo");
            ViewBag.TipoHardware = _tipoHardwareService.GetAll();

            return View();
        }

        // POST: Sala/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaModel salaModel)
        {
            ViewBag.BlocoList = new SelectList(GetBlocos(), "Id", "Titulo");
            ViewBag.TipoHardware = _tipoHardwareService.GetAll();

            try
            {
                if (ModelState.IsValid)
                {

                    if (_salaService.InsertSalaWithHardwares(salaModel))
                    {
                        TempData["mensagemSucesso"] = "Sala inserida com sucesso!"; return View();
                    }
                    else TempData["mensagemErro"] = "Houve um problema ao inserir sala!";

                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return View(salaModel);
        }

        // GET: Sala/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.BlocoList = new SelectList(GetBlocos(), "Id", "Titulo");
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // POST: Sala/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SalaModel salaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Update(salaModel))
                        TempData["mensagemSucesso"] = "Sala atualizada com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar sala, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(salaModel);
        }

        // GET: Sala/Delete/5
        public ActionResult Delete(int id)
        {
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // POST: Sala/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            SalaModel salaModel = _salaService.GetById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    if (_salaService.Remove(id))
                        TempData["mensagemSucesso"] = "Sala removida com sucesso!";
                    else
                        TempData["mensagemErro"] = "Houve um problema ao remover a sala, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<SalaViewModel> GetAllSalasViewModel()
        {
            var idUser = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var todasSalas = _salaService.GetAll();
            var blocos = GetBlocos();

            var query = (from ol in todasSalas
                         join bl in blocos on ol.BlocoId equals bl.Id
                         select new SalaViewModel
                         {
                             Sala = ol,
                             BlocoSala = bl,
                         }).ToList();

            return query;
        }

        private SalaViewModel GetSalaViewModel(int id)
        {
            var sala = _salaService.GetById(id);
            var hardwaresViewModel = new List<HardwareDeSalaViewModel>();

            foreach (var item in _hardwareDeSalaService.GetByIdSala(id))
                hardwaresViewModel.Add(new HardwareDeSalaViewModel { Id = item.Id, MAC = item.MAC, TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId) });

            return new SalaViewModel
            {
                Sala = sala,
                HardwaresSala = hardwaresViewModel,
                BlocoSala = _blocoService.GetById(sala.BlocoId)
            };
        }

        public List<BlocoModel> GetBlocos()
        {
            var idUser = int.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var organizacoesLotadas = _usuarioOrganizacaoService.GetByIdUsuario(idUser);

            List<BlocoModel> blocos = new List<BlocoModel>();
            organizacoesLotadas.ForEach(org => blocos.AddRange(_blocoService.GetByIdOrganizacao(org.OrganizacaoId)));

            return blocos;
        }
    }
}