using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using Service;
using Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ADMINISTRATIVE_ROLES)]
    public class SalaController : Controller
    {
        private readonly ISalaService _salaService;
        private readonly IBlocoService _blocoService;
        private readonly IHardwareDeSalaService _hardwareDeSalaService;
        private readonly ITipoHardwareService _tipoHardwareService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IConexaoInternetService _conexaoInternetService;
        private readonly IConexaoInternetSalaService _conexaoInternetSalaService;

        public SalaController(ISalaService salaService, IBlocoService blocoService, IHardwareDeSalaService hardwareDeSalaService, ITipoHardwareService tipoHardwareService, IUsuarioOrganizacaoService usuarioOrganizacaoService, IUsuarioService usuarioService, IOrganizacaoService organizacaoService, IConexaoInternetService conexaoInternetService, IConexaoInternetSalaService conexaoInternetSalaService)
        {
            _salaService = salaService;
            _blocoService = blocoService;
            _hardwareDeSalaService = hardwareDeSalaService;
            _tipoHardwareService = tipoHardwareService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _usuarioService = usuarioService;
            _organizacaoService = organizacaoService;
            _conexaoInternetService = conexaoInternetService;
            _conexaoInternetSalaService = conexaoInternetSalaService;
        }

        // GET: Sala
        public ActionResult Index()
        {
            return View(GetAllSalasViewModel());
        }

        // GET: Sala/Details/5
        public ActionResult Details(uint id)
        {
            return View(GetSalaViewModel(id));
        }

        // GET: Sala/Create
        public ActionResult Create()
        {
            var usuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel;
            var orgs = _organizacaoService.GetByIdUsuario(usuario.Id);
            var organizacaoId = orgs.FirstOrDefault().Id;

            // Obtém os blocos
            var blocos = _blocoService.GetByIdOrganizacao(organizacaoId);

            // Obter conexões apenas do primeiro bloco (inicialmente selecionado)
            var conexoesDisponiveis = new List<ConexaointernetModel>();
            if (blocos.Any())
            {
                conexoesDisponiveis = _conexaoInternetService.GetByIdBloco(blocos.First().Id);
            }

            ViewBag.Organizacoes = orgs;
            ViewBag.BlocoList = blocos;
            ViewBag.TipoHardware = _tipoHardwareService.GetByIdOrganizacao(organizacaoId);
            ViewBag.ConexoesInternet = conexoesDisponiveis;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalaAuxModel salaModel)
        {
            var usuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity);
            var blocos = _blocoService.GetByIdOrganizacao(salaModel.OrganizacaoId);

            // Obtém as conexões de internet disponíveis
            var conexoesDisponiveis = new List<ConexaointernetModel>();
            foreach (var bloco in blocos)
            {
                var conexoesDoBloco = _conexaoInternetService.GetByIdBloco(bloco.Id);
                conexoesDisponiveis.AddRange(conexoesDoBloco);
            }

            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(usuario.UsuarioModel.Id);
            ViewBag.BlocoList = blocos;
            ViewBag.TipoHardware = _tipoHardwareService.GetAll();
            ViewBag.ConexoesInternet = conexoesDisponiveis;

            try
            {
                if (ModelState.IsValid)
                {
                    bool salaInserida = _salaService.InsertSalaWithHardwaresOrSalasWithPontosdeAcesso(salaModel, usuario.UsuarioModel.Id);
                    if (salaInserida)
                    {
                        TempData["mensagemSucesso"] = "Sala inserida com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao inserir a sala!";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }
            return View(salaModel);
        }


        // GET: Sala/Edit/5
        [ResponseCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(uint id)
        {
            var salaModel = _salaService.GetById(id);
            var idOrganizacao = _blocoService.GetById(salaModel.BlocoId).OrganizacaoId;
            
            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(idOrganizacao);
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id);

            var conexoesSala = _conexaoInternetSalaService.GetByIdSala(id).OrderBy(c => c.Prioridade).ToList();

            var blocos = _blocoService.GetByIdOrganizacao(idOrganizacao);
            var conexoesDisponiveis = new List<ConexaointernetModel>();
            foreach (var bloco in blocos)
            {
                var conexoesDoBloco = _conexaoInternetService.GetByIdBloco(bloco.Id);
                conexoesDisponiveis.AddRange(conexoesDoBloco);
            }
            ViewBag.ConexoesInternet = conexoesDisponiveis;

            var model = new SalaAuxModel
            {
                Sala = new SalaModel
                {
                    Id = salaModel.Id,
                    Titulo = salaModel.Titulo,
                    BlocoId = salaModel.BlocoId
                },
                OrganizacaoId = idOrganizacao,
                ConexaoInternetSala = conexoesSala
            };

            return View(model);
        }

        // POST: Sala/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SalaAuxModel salaModel)
        {
            var usuario = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity);
            ViewBag.BlocoList = _blocoService.GetByIdOrganizacao(salaModel.OrganizacaoId);
            ViewBag.Organizacoes = _organizacaoService.GetByIdUsuario(usuario.UsuarioModel.Id);

            var blocos = _blocoService.GetByIdOrganizacao(salaModel.OrganizacaoId);
            var conexoesDisponiveis = new List<ConexaointernetModel>();
            foreach (var bloco in blocos)
            {
                var conexoesDoBloco = _conexaoInternetService.GetByIdBloco(bloco.Id);
                conexoesDisponiveis.AddRange(conexoesDoBloco);
            }
            ViewBag.ConexoesInternet = conexoesDisponiveis;

            try
            {
                if (ModelState.IsValid)
                {
                    bool salaAtualizada = _salaService.Update(new SalaModel
                    {
                        Id = salaModel.Sala.Id,
                        BlocoId = salaModel.Sala.BlocoId,
                        Titulo = salaModel.Sala.Titulo
                    });

                    if (salaAtualizada)
                    {           
                        var conexoesExistentes = _conexaoInternetSalaService.GetByIdSala(salaModel.Sala.Id);
                        foreach (var conexao in conexoesExistentes)
                        {
                            _conexaoInternetSalaService.Remove(conexao.ConexaoInternetId, salaModel.Sala.Id);
                        }

                        if (salaModel.ConexaoInternetSala?.Count > 0)
                        {
                            foreach (var item in salaModel.ConexaoInternetSala)
                            {
                                _conexaoInternetSalaService.Insert(new ConexaoInternetSalaModel
                                {
                                    ConexaoInternetId = item.ConexaoInternetId,
                                    SalaId = salaModel.Sala.Id,
                                    Prioridade = item.Prioridade
                                });
                            }
                        }

                        TempData["mensagemSucesso"] = "Sala atualizada com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
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
        public ActionResult Delete(uint id)
        {
            SalaModel salaModel = _salaService.GetById(id);
            return View(salaModel);
        }

        // POST: Sala/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(uint id, IFormCollection collection)
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

        // GET: Sala/GetConexoesByBloco
        public JsonResult GetConexoesByBloco(uint idBloco)
        {
            var conexoes = _conexaoInternetService.GetByIdBloco(idBloco);
            return Json(conexoes);
        }

        // GET: Sala/GetBlocosByOrganizacao
        public JsonResult GetBlocosByOrganizacao(uint idOrganizacao)
        {
            var blocos = _blocoService.GetByIdOrganizacao(idOrganizacao);
            return Json(blocos);
        }

        private List<SalaViewModel> GetAllSalasViewModel()
        {
            var idUser = uint.Parse(((ClaimsIdentity)User.Identity).Claims.Where(s => s.Type == ClaimTypes.SerialNumber).Select(s => s.Value).FirstOrDefault());
            var salasViewModel = new List<SalaViewModel>();

            var salas = _salaService.GetAllByIdUsuarioOrganizacao(idUser);
            salas.ForEach(s => salasViewModel.Add(new SalaViewModel { BlocoSala = _blocoService.GetById(s.BlocoId), Sala = _salaService.GetById(s.Id) }));


            return salasViewModel;
        }

        private SalaViewModel GetSalaViewModel(uint id)
        {
            var sala = _salaService.GetById(id);
            var hardwaresViewModel = new List<HardwareDeSalaViewModel>();
            var conexaoInternetSalaModel = new List<ConexaoInternetSalaModel>();

            foreach (var item in _hardwareDeSalaService.GetByIdSala(id))
                hardwaresViewModel.Add(new HardwareDeSalaViewModel { Id = item.Id, MAC = item.MAC, TipoHardwareId = _tipoHardwareService.GetById(item.TipoHardwareId) });

            foreach (var item in _conexaoInternetSalaService.GetByIdSala(id))
            {
                var pontoAcesso = _conexaoInternetService.GetById(item.ConexaoInternetId); 
                conexaoInternetSalaModel.Add(new ConexaoInternetSalaModel
                {
                    ConexaoInternetId = item.ConexaoInternetId,
                    Prioridade = item.Prioridade,
                    SalaId = item.SalaId,
                    Ssid = pontoAcesso?.Nome
                });
            }

            return new SalaViewModel
            {
                Sala = sala,
                HardwaresSala = hardwaresViewModel,
                ConexaoSala = conexaoInternetSalaModel,
                BlocoSala = _blocoService.GetById(sala.BlocoId),
                
            };
        }
    }
}