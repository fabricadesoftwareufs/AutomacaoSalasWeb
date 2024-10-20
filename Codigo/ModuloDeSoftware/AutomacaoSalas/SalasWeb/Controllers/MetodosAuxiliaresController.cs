using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;
using System.Collections.Generic;
using System.Security.Claims;

namespace SalasWeb.Controllers
{
    [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
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
            _usuarioOrganizacaoService.GetByIdUsuario(_usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity).UsuarioModel.Id).
                ForEach(ex => organizacoes.Add(_organizacaoService.GetById(ex.OrganizacaoId)));

            return organizacoes;
        }

        public List<UsuarioModel> GetUsuariosByIdOrganizacao(uint idOrganizacao)
        {
            return _usuarioService.GetByIdOrganizacao(idOrganizacao);
        }

        public List<BlocoModel> GetBlocosByIdOrganizacao(uint idOrganizacao)
        {
            return _blocoService.GetByIdOrganizacao(idOrganizacao);
        }

        public List<SalaModel> GetSalasByIdBloco(uint idBloco)
        {
            return _salaService.GetByIdBloco(idBloco);
        }

    }
}
