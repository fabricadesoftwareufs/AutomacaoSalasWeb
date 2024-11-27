using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
    public class HardwareDeSalaController : Controller
    {
        private readonly IHardwareDeSalaService _hardwareService;
        private readonly ITipoHardwareService _tipoHardwareService;
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IBlocoService _blocoService;

        public HardwareDeSalaController(IHardwareDeSalaService hardwareService,
                                        ITipoHardwareService tipoHardwareService,
                                        ISalaService salaService,
                                        IUsuarioService usuarioService,
                                        IOrganizacaoService organizacaoService,
                                        IBlocoService blocoService)
        {
            _hardwareService = hardwareService;
            _tipoHardwareService = tipoHardwareService;
            _salaService = salaService;
            _usuarioService = usuarioService;
            _organizacaoService = organizacaoService;
            _blocoService = blocoService;
        }


        public IActionResult Index()
        {
            var hardwares = GetAllViewModels();
            return View(hardwares);
        }

        public IActionResult Create()
        {
            var organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var blocos = _blocoService.GetByIdOrganizacao(organizacoes.FirstOrDefault().Id);

            ViewBag.Organizacoes = organizacoes;
            ViewBag.Salas = _salaService.GetByIdBloco(blocos.FirstOrDefault().Id);
            ViewBag.Blocos = blocos;
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HardwareDeSalaModel hardware)
        {
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(idUsuario);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(hardware.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(hardware.Bloco);
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (string.IsNullOrEmpty(hardware.Ip) && hardware.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");

                if (ModelState.IsValid)
                {
                    if (_hardwareService.Insert(hardware, idUsuario))
                    {
                        TempData["mensagemSucesso"] = "Hardware adicionado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao tentar inserir o hardware, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return View(hardware);
        }


        public IActionResult Edit(uint id)
        {
            var hardware = _hardwareService.GetById(id);
            var bloco = _blocoService.GetById(_salaService.GetById(hardware.SalaId).BlocoId);
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(idUsuario);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(bloco.OrganizacaoId);
            ViewBag.Salas = _salaService.GetByIdBloco(bloco.Id);
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            return View(hardware);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, HardwareDeSalaModel hardware)
        {
            var idUsuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id;

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(idUsuario);
            ViewBag.Blocos = _blocoService.GetByIdOrganizacao(hardware.Organizacao);
            ViewBag.Salas = _salaService.GetByIdBloco(hardware.Bloco);
            ViewBag.tipoHardware = new SelectList(_tipoHardwareService.GetAll(), "Id", "Descricao");

            try
            {
                if (string.IsNullOrEmpty(hardware.Ip) && hardware.TipoHardwareId == TipoHardwareModel.CONTROLADOR_DE_SALA)
                {
                    ModelState.AddModelError("Ip", "Adicione um endereço IP");
                    return View(hardware);
                }


                if (ModelState.IsValid)
                {
                    if (_hardwareService.Update(hardware, idUsuario))
                    {
                        TempData["mensagemSucesso"] = "Hardware atualizado com sucesso";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        TempData["mensagemErro"] = "Houve um problema ao atualizar hardware, tente novamente em alguns minutos!";
                }
            }
            catch (ServiceException se) { TempData["mensagemErro"] = se.Message; }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(uint id)
        {
            return View(GetByIdViewModel(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (_hardwareService.Remove(id))
                    TempData["mensagemSucesso"] = "Hardware removido com sucesso!";
                else
                    TempData["mensagemErro"] = "Houve um problema ao tentar remover o hardware, tente novamente em alguns minutos!";
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private List<HardwareDeSalaViewModel> GetAllViewModels()
        {
            var hardwares = _hardwareService.GetAllHardwaresSalaByUsuarioOrganizacao(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);
            var hardwaresViewModel = new List<HardwareDeSalaViewModel>();

            hardwares.ForEach(h => hardwaresViewModel.Add(Cast(h)));

            return hardwaresViewModel;
        }

        private HardwareDeSalaViewModel GetByIdViewModel(uint id)
        {
            HardwareDeSalaModel h = _hardwareService.GetById(id);

            return Cast(h);
        }

        private HardwareDeSalaViewModel Cast(HardwareDeSalaModel item)
        {
            HardwareDeSalaViewModel h = new HardwareDeSalaViewModel();

            h.Id = item.Id;
            h.MAC = item.MAC;
            h.Ip = item.Ip;
            h.SalaId = _salaService.GetById(item.SalaId);
            h.TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId);

            return h;
        }
    }
}
