using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace SalasUfsWeb.Controllers
{

    public class MetodosAuxiliaresController : ControllerBase
    {
        private readonly ISalaService _salaService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IBlocoService _blocoService;
        private readonly IOrganizacaoService _organizacaoService;


        public MetodosAuxiliaresController(ISalaService salaService,
                                           IUsuarioService usuarioService,
                                           IUsuarioOrganizacaoService usuarioOrganizacaoService,
                                           IBlocoService blocoService,
                                           IOrganizacaoService organizacaoService)
        {
            _salaService = salaService;
            _usuarioService = usuarioService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _blocoService = blocoService;
            _organizacaoService = organizacaoService;
        }

        public List<OrganizacaoModel> GetOrganizacoes()
        {

            var organizacoes = new List<OrganizacaoModel>();
            _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.RetornLoggedUser((ClaimsIdentity)User.Identity).Id).
                ForEach(ex => organizacoes.Add(_organizacaoService.GetById(ex.OrganizacaoId)));

            return organizacoes;
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
