using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.AuxModel;
using Model.ViewModel;
using SalasWeb.Data;
using Service;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utils;

namespace SalasWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IUsuarioOrganizacaoService _usuarioOrganizacaoService;
        private readonly IPlanejamentoService _planejamentoService;
        private readonly IHorarioSalaService _horarioSalaService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public UsuarioController(
            IUsuarioService usuarioService,
            ITipoUsuarioService tipoUsuarioService,
            IOrganizacaoService organizacaoService,
            IUsuarioOrganizacaoService usuarioOrganizacaoService,
            IPlanejamentoService planejamentoService,
            IHorarioSalaService horarioSalaService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender 
        )
        {
            _usuarioService = usuarioService;
            _tipoUsuarioService = tipoUsuarioService;
            _organizacaoService = organizacaoService;
            _usuarioOrganizacaoService = usuarioOrganizacaoService;
            _planejamentoService = planejamentoService;
            _horarioSalaService = horarioSalaService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender; 
        }

        // GET: Usuario/Details/5
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Details(uint id)
        {
            var usuario = _usuarioService.GetById(id);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(id);
            var organizacao = _organizacaoService.GetByIdUsuario(id).FirstOrDefault();

            // Buscar email no Identity
            var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuario.Cpf);

            var usuarioView = new UsuarioViewModel
            {
                UsuarioModel = usuario,
                TipoUsuarioModel = tipoUsuario,
                OrganizacaoModel = organizacao,
                Email = identityUser?.Email ?? "Não informado"
            };

            return View(usuarioView);
        }

        // GET: Usuario
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Index()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var usuarioLogado = _usuarioService.GetAuthenticatedUser(claimsIdentity);
            var usuarios = _usuarioService.GetAllExceptAuthenticatedUser(usuarioLogado.UsuarioModel.Id);

            List<UsuarioAuxModel> lista = new List<UsuarioAuxModel>();

            foreach (var s in usuarios)
            {
                // Buscar email no Identity
                var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == s.Cpf);

                // Verificar se o email foi confirmado
                bool emailConfirmado = false;
                if (identityUser != null)
                {
                    emailConfirmado = await _userManager.IsEmailConfirmedAsync(identityUser);
                }

                lista.Add(new UsuarioAuxModel
                {
                    UsuarioModel = s,
                    TipoUsuarioModel = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(s.Id),
                    OrganizacaoModels = _organizacaoService.GetByIdUsuario(s.Id),
                    Email = identityUser?.Email ?? "Não informado",
                    EmailConfirmado = emailConfirmado 
                });
            }

            return View(lista);
        }

        // GET: Usuario/Create
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public ActionResult Create()
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Create(UsuarioViewModel usuarioViewModel)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        TempData["mensagemErro"] += error.ErrorMessage + " ";
                    }
                }
                return View(usuarioViewModel);
            }

            // Validações
            usuarioViewModel.OrganizacaoModel = _organizacaoService.GetById(usuarioViewModel.OrganizacaoModel.Id);
            if (usuarioViewModel.OrganizacaoModel == null)
            {
                TempData["mensagemErro"] = "Organização não encontrada.";
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarDataNascimento(usuarioViewModel.UsuarioModel.DataNascimento))
            {
                ModelState.AddModelError("UsuarioModel.DataNascimento", "Data de nascimento inválida.");
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarCpf(usuarioViewModel.UsuarioModel.Cpf))
            {
                TempData["mensagemErro"] = "CPF inválido!";
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.Email))
            {
                ModelState.AddModelError("Email", "Email é obrigatório.");
                return View(usuarioViewModel);
            }

            var cpfLimpo = Methods.CleanString(usuarioViewModel.UsuarioModel.Cpf);

            // Verificar se já existe usuário com esse CPF no Identity
            var existingIdentityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == cpfLimpo);
            if (existingIdentityUser != null)
            {
                TempData["mensagemErro"] = "Já existe um usuário cadastrado com este CPF.";
                return View(usuarioViewModel);
            }

            // Verificar se já existe usuário com esse email
            var existingEmailUser = await _userManager.FindByEmailAsync(usuarioViewModel.Email);
            if (existingEmailUser != null)
            {
                TempData["mensagemErro"] = "Já existe um usuário cadastrado com este email.";
                return View(usuarioViewModel);
            }

            usuarioViewModel.UsuarioModel.Cpf = cpfLimpo;

            // Salvar a senha original antes de criptografar para o sistema legado
            var senhaOriginal = usuarioViewModel.UsuarioModel.Senha;

            try
            {
                // 1. Criar no sistema legado
                var usuarioLegado = _usuarioService.Insert(usuarioViewModel);

                // 2. Criar no Identity com EmailConfirmed = false
                var identityUser = new ApplicationUser
                {
                    UserName = usuarioViewModel.Email,
                    Email = usuarioViewModel.Email,
                    EmailConfirmed = false, // Alterado para false - usuário precisa confirmar email
                    Cpf = cpfLimpo,
                    BirthDate = usuarioViewModel.UsuarioModel.DataNascimento
                };

                var result = await _userManager.CreateAsync(identityUser, senhaOriginal);

                if (result.Succeeded)
                {
                    // 3. Adicionar role baseado no tipo de usuário
                    var tipoUsuario = _tipoUsuarioService.GetAll().FirstOrDefault(t => t.Id == usuarioViewModel.TipoUsuarioModel.Id);
                    string role = GetRoleByTipoUsuario(tipoUsuario?.Descricao);
                    await _userManager.AddToRoleAsync(identityUser, role);

                    // 4. Adicionar claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuarioLegado.UsuarioModel.Id.ToString()),
                        new Claim(ClaimTypes.UserData, cpfLimpo),
                        new Claim(ClaimTypes.Name, usuarioViewModel.UsuarioModel.Nome),
                        new Claim(ClaimTypes.Role, role)
                    };
                    await _userManager.AddClaimsAsync(identityUser, claims);

                    // 5. Enviar emails de confirmação e redefinição de senha
                    await SendUserCreationEmailsAsync(identityUser, usuarioViewModel.UsuarioModel.Nome);

                    TempData["mensagemSucesso"] = "Usuário criado com sucesso! Emails de confirmação e redefinição de senha foram enviados.";
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    // Rollback do sistema legado se Identity falhou
                    _usuarioService.Remove((int)usuarioLegado.UsuarioModel.Id);
                    TempData["mensagemErro"] = string.Join(", ", result.Errors.Select(e => e.Description));
                    return View(usuarioViewModel);
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioViewModel);
            }
        }

        /// <summary>
        /// Envia emails de confirmação de conta e redefinição de senha para usuário criado pelo admin
        /// </summary>
        private async Task SendUserCreationEmailsAsync(ApplicationUser user, string userName)
        {
            try
            {
                // 1. Gerar e enviar email de confirmação
                var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var emailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "", userId = user.Id, code = emailConfirmationCode },
                    protocol: Request.Scheme);

                await SendEmailConfirmationEmailAsync(user.Email, userName, emailConfirmationUrl);

                // 2. Gerar e enviar email de redefinição de senha
                var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResetUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "", code = passwordResetCode, email = user.Email },
                    protocol: Request.Scheme);

                await SendPasswordResetEmailAsync(user.Email, userName, passwordResetUrl);
            }
            catch (Exception ex)
            {
                // Log do erro, mas não falhar a criação do usuário
                // Em produção, você deveria usar um logger adequado
                Console.WriteLine($"Erro ao enviar emails: {ex.Message}");
            }
        }

        /// <summary>
        /// Envia email de confirmação de conta
        /// </summary>
        private async Task SendEmailConfirmationEmailAsync(string email, string userName, string confirmationUrl)
        {
            var emailSubject = "Confirme sua conta - Smart Salas";
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <img src=""https://smartsala.itatechjr.com.br/wp-content/uploads/2025/03/SmartSalas-2-WEBP.webp"" 
                             alt=""Smart Salas"" 
                             style=""max-width: 200px; height: auto; display: block; margin: 0 auto;"" />

                        <h3 style='color: #333;'>Bem-vindo ao Smart Salas!</h3>
                        
                        <p>Olá <strong>{userName}</strong>,</p>
                        
                        <p>Uma conta foi criada para você no sistema Smart Salas por um administrador. Para ativar sua conta, você precisa confirmar seu endereço de email.</p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{confirmationUrl}' 
                               style='background-color: #28a745; color: white; padding: 12px 30px; 
                                      text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Confirmar Email
                            </a>
                        </div>
                        
                        <p><strong>Importante:</strong></p>
                        <ul>
                            <li>Este link expira em 24 horas por motivos de segurança</li>
                            <li>Você só poderá fazer login após confirmar seu email</li>
                            <li>Você também receberá um email separado para definir sua própria senha</li>
                        </ul>
                        
                        <p><strong>Suas credenciais de login:</strong></p>
                        <ul>
                            <li><strong>Email:</strong> {email}</li>
                            <li><strong>CPF:</strong> Use o CPF fornecido durante o cadastro</li>
                        </ul>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; text-align: center; 
                                font-size: 12px; color: #666;'>
                        <p>Este é um email automático, não responda.</p>
                        <p>© Smart Salas - Sistema de Gerenciamento de Salas</p>
                    </div>
                </div>";

            await _emailSender.SendEmailAsync(email, emailSubject, emailBody);
        }

        /// <summary>
        /// Envia email de redefinição de senha
        /// </summary>
        private async Task SendPasswordResetEmailAsync(string email, string userName, string resetUrl)
        {
            var emailSubject = "Defina sua senha - Smart Salas";
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='padding: 30px; background-color: #ffffff;'>
                        <img src=""https://smartsala.itatechjr.com.br/wp-content/uploads/2025/03/SmartSalas-2-WEBP.webp"" 
                             alt=""Smart Salas"" 
                             style=""max-width: 200px; height: auto; display: block; margin: 0 auto;"" />

                        <h3 style='color: #333;'>Defina sua senha</h3>
                        
                        <p>Olá <strong>{userName}</strong>,</p>
                        
                        <p>Uma conta foi criada para você no sistema Smart Salas. Para completar o processo de ativação, você precisa definir sua própria senha de acesso.</p>
                        
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{resetUrl}' 
                               style='background-color: #007bff; color: white; padding: 12px 30px; 
                                      text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Definir Minha Senha
                            </a>
                        </div>
                        
                        <p><strong>Instruções:</strong></p>
                        <ol>
                            <li>Primeiro, confirme seu email (você deve ter recebido um email separado para isso)</li>
                            <li>Depois, clique no link acima para definir sua senha</li>
                            <li>Após definir a senha, você poderá fazer login normalmente</li>
                        </ol>
                        
                        <p><strong>Informações importantes:</strong></p>
                        <ul>
                            <li>Este link expira em 24 horas por motivos de segurança</li>
                            <li>Você pode usar tanto seu email quanto seu CPF para fazer login</li>
                            <li>Entre em contato com o administrador se tiver dificuldades</li>
                        </ul>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; text-align: center; 
                                font-size: 12px; color: #666;'>
                        <p>Este é um email automático, não responda.</p>
                        <p>© Smart Salas - Sistema de Gerenciamento de Salas</p>
                    </div>
                </div>";

            await _emailSender.SendEmailAsync(email, emailSubject, emailBody);
        }

        // GET: Usuario/Edit/5
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Edit(uint id)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            var usuario = _usuarioService.GetById(id);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(id);
            var organizacao = _organizacaoService.GetByIdUsuario(id);

            // Buscar email no Identity
            var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuario.Cpf);

            var usuarioView = new UsuarioViewModel
            {
                UsuarioModel = usuario,
                TipoUsuarioModel = tipoUsuario,
                OrganizacaoModel = organizacao.FirstOrDefault(),
                Email = identityUser?.Email ?? ""
            };

            return View(usuarioView);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Edit(int id, UsuarioViewModel usuarioView)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");

            usuarioView.UsuarioModel.IdOrganizacao = usuarioView.OrganizacaoModel.Id;
            usuarioView.UsuarioModel.IdTipoUsuario = usuarioView.TipoUsuarioModel.Id;

            try
            {
                ModelState.Remove("UsuarioModel.Senha");

                if (ModelState.IsValid)
                {
                    // Pegar dados atuais antes da atualização
                    var usuarioAtual = _usuarioService.GetById((uint)id);
                    var tipoUsuarioAtual = _tipoUsuarioService.GetTipoUsuarioByUsuarioId((uint)id);

                    usuarioView.UsuarioModel.Senha = usuarioAtual.Senha;

                    // Atualizar sistema legado
                    if (_usuarioService.Update(usuarioView.UsuarioModel))
                    {
                        // Buscar usuário no Identity
                        var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuarioAtual.Cpf);
                        if (identityUser != null)
                        {
                            // Atualizar email se necessário
                            if (!string.IsNullOrEmpty(usuarioView.Email) && identityUser.Email != usuarioView.Email)
                            {
                                identityUser.Email = usuarioView.Email;
                                identityUser.UserName = usuarioView.Email;
                                await _userManager.UpdateAsync(identityUser);
                            }

                            // Verificar mudanças
                            bool tipoUsuarioMudou = tipoUsuarioAtual.Id != usuarioView.TipoUsuarioModel.Id;
                            bool nomeMudou = usuarioAtual.Nome != usuarioView.UsuarioModel.Nome;

                            // Atualizar roles se tipo de usuário mudou
                            if (tipoUsuarioMudou)
                            {
                                // Remover roles atuais
                                var currentRoles = await _userManager.GetRolesAsync(identityUser);
                                if (currentRoles.Any())
                                {
                                    await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);
                                }

                                // Adicionar nova role
                                var novoTipoUsuario = _tipoUsuarioService.GetAll().FirstOrDefault(t => t.Id == usuarioView.TipoUsuarioModel.Id);
                                string newRole = GetRoleByTipoUsuario(novoTipoUsuario?.Descricao);
                                await _userManager.AddToRoleAsync(identityUser, newRole);
                            }

                            // Atualizar claims se necessário
                            if (tipoUsuarioMudou || nomeMudou)
                            {
                                // Remover claims atuais
                                var currentClaims = await _userManager.GetClaimsAsync(identityUser);
                                if (currentClaims.Any())
                                {
                                    await _userManager.RemoveClaimsAsync(identityUser, currentClaims);
                                }

                                // Preparar novos claims
                                var newClaims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                                    new Claim(ClaimTypes.UserData, usuarioAtual.Cpf),
                                    new Claim(ClaimTypes.Name, usuarioView.UsuarioModel.Nome)
                                };

                                // Adicionar claim de role
                                if (tipoUsuarioMudou)
                                {
                                    var novoTipoUsuario = _tipoUsuarioService.GetAll().FirstOrDefault(t => t.Id == usuarioView.TipoUsuarioModel.Id);
                                    string newRole = GetRoleByTipoUsuario(novoTipoUsuario?.Descricao);
                                    newClaims.Add(new Claim(ClaimTypes.Role, newRole));
                                }

                                await _userManager.AddClaimsAsync(identityUser, newClaims);
                            }

                            // Refresh do sign-in se o usuário atual está sendo editado
                            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            if (currentUserId == id.ToString())
                            {
                                await _signInManager.RefreshSignInAsync(identityUser);
                            }
                        }

                        TempData["mensagemSucesso"] = "Usuário editado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao editar o usuário, tente novamente em alguns minutos.";
                        return View(usuarioView);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioView);
            }
            catch (Exception ex)
            {
                TempData["mensagemErro"] = "Erro inesperado ao editar usuário.";
                return View(usuarioView);
            }

            return View(usuarioView);
        }

        // Método para reenviar emails de ativação
        [HttpPost]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> ResendActivationEmails(uint userId)
        {
            try
            {
                var usuario = _usuarioService.GetById(userId);
                if (usuario == null)
                {
                    TempData["mensagemErro"] = "Usuário não encontrado.";
                    return RedirectToAction("Index");
                }

                var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuario.Cpf);
                if (identityUser == null)
                {
                    TempData["mensagemErro"] = "Usuário não encontrado no sistema de autenticação.";
                    return RedirectToAction("Index");
                }

                // Reenviar emails
                await SendUserCreationEmailsAsync(identityUser, usuario.Nome);

                TempData["mensagemSucesso"] = "Emails de ativação reenviados com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["mensagemErro"] = "Erro ao reenviar emails de ativação.";
            }

            return RedirectToAction("Index");
        }

        // GET: Usuario/EditPassword
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public async Task<ActionResult> EditPassword()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var usuarioLogado = _usuarioService.GetAuthenticatedUser(claimsIdentity);

            // Buscar o usuário no Identity usando o CPF
            var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuarioLogado.UsuarioModel.Cpf);
            if (identityUser == null)
            {
                TempData["mensagemErro"] = "Usuário não encontrado no sistema de autenticação.";
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new AlterarSenhaViewModel { UsuarioId = usuarioLogado.UsuarioModel.Id };
            return View(viewModel);
        }

        // POST: Usuario/EditPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public async Task<ActionResult> EditPassword(AlterarSenhaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;
                    var usuarioLogado = _usuarioService.GetAuthenticatedUser(claimsIdentity);

                    // Buscar o usuário no Identity usando o CPF
                    var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuarioLogado.UsuarioModel.Cpf);
                    if (identityUser == null)
                    {
                        TempData["mensagemErro"] = "Usuário não encontrado no sistema de autenticação.";
                        return View(model);
                    }

                    // Verificar se a senha atual está correta usando o Identity
                    var passwordCheck = await _userManager.CheckPasswordAsync(identityUser, model.SenhaAtual);
                    if (!passwordCheck)
                    {
                        ModelState.AddModelError("SenhaAtual", "Senha atual incorreta.");
                        return View(model);
                    }

                    // Alterar a senha usando o Identity
                    var changePasswordResult = await _userManager.ChangePasswordAsync(identityUser, model.SenhaAtual, model.NovaSenha);

                    if (changePasswordResult.Succeeded)
                    {
                        // Atualizar o signin para manter o usuário logado com a nova senha
                        await _signInManager.RefreshSignInAsync(identityUser);

                        TempData["mensagemSucesso"] = "Senha alterada com sucesso!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Adicionar os erros do Identity ao ModelState
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = "Erro inesperado ao alterar a senha. Tente novamente.";
                return View(model);
            }
            return View(model);
        }

        // GET: Usuario/EditPersonalData
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPersonalData()
        {
            var usuarioId = _usuarioService.GetAuthenticatedUser((ClaimsIdentity)User.Identity)?.UsuarioModel?.Id ?? 0;

            var usuario = _usuarioService.GetById(usuarioId);
            var tipoUsuario = _tipoUsuarioService.GetTipoUsuarioByUsuarioId(usuarioId);
            var organizacao = _organizacaoService.GetByIdUsuario(usuarioId);
            var usuarioView = new UsuarioViewModel { UsuarioModel = usuario, TipoUsuarioModel = tipoUsuario, OrganizacaoModel = organizacao.FirstOrDefault() };

            return View(usuarioView);
        }

        // POST: Usuario/EditPersonalData
        //TODO: Inserir e-mail e possibilitar a edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ALL_ROLES)]
        public ActionResult EditPersonalData(int id, UsuarioViewModel usuarioView)
        {
            try
            {
                // Remove a validação da senha para edição de dados pessoais
                ModelState.Remove("UsuarioModel.Senha");

                if (ModelState.IsValid)
                {
                    usuarioView.UsuarioModel.IdOrganizacao = usuarioView.OrganizacaoModel.Id;
                    usuarioView.UsuarioModel.IdTipoUsuario = usuarioView.TipoUsuarioModel.Id;
                    usuarioView.UsuarioModel.TipoUsuarioId = usuarioView.TipoUsuarioModel.Id;
                    if (_usuarioService.Update(usuarioView.UsuarioModel))
                    {
                        TempData["mensagemSucesso"] = "Dados pessoais salvos com sucesso!";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao editar seus dados, tente novamente em alguns minutos.";
                        return View(usuarioView);
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioView);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: PublicUsuario/Create
        public ActionResult PublicUsuarioCreate()
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            return View();
        }

        // POST: PublicUsuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublicUsuarioCreate(UsuarioViewModel usuarioViewModel)
        {
            ViewBag.TiposUsuario = new SelectList(_tipoUsuarioService.GetAll(), "Id", "Descricao");
            ViewBag.Organizacoes = new SelectList(_organizacaoService.GetAll(), "Id", "RazaoSocial");
            //usuarioViewModel.TipoUsuarioModel = new TipoUsuarioModel { Id = 4 };  // Tipo "pendente"

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        TempData["mensagemErro"] += error.ErrorMessage + " ";
                    }
                }
                return View(usuarioViewModel);
            }

            usuarioViewModel.OrganizacaoModel = _organizacaoService.GetById(usuarioViewModel.OrganizacaoModel.Id);
            if (usuarioViewModel.OrganizacaoModel == null)
            {
                TempData["mensagemErro"] = "Organização não encontrada.";
                return View(usuarioViewModel);
            }


            if (!Methods.ValidarDataNascimento(usuarioViewModel.UsuarioModel.DataNascimento))
            {
                ModelState.AddModelError("UsuarioModel.DataNascimento", "Data de nascimento inválida.");
                return View(usuarioViewModel);
            }

            if (!Methods.ValidarCpf(usuarioViewModel.UsuarioModel.Cpf))
            {
                TempData["mensagemErro"] = "CPF inválido!";
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Nome))
            {
                ModelState.AddModelError("UsuarioModel.Nome", "Nome do usuário não pode ser vazio.");
                return View(usuarioViewModel);
            }

            if (string.IsNullOrEmpty(usuarioViewModel.UsuarioModel.Senha) || usuarioViewModel.UsuarioModel.Senha.Length < 8)
            {
                ModelState.AddModelError("UsuarioModel.Senha", "A senha deve ter pelo menos 8 caracteres.");
                return View(usuarioViewModel);
            }

            if (usuarioViewModel.TipoUsuarioModel == null || usuarioViewModel.TipoUsuarioModel.Id <= 0)
            {
                ModelState.AddModelError("TipoUsuarioModel.Id", "Tipo de usuário inválido.");
                return View(usuarioViewModel);
            }

            usuarioViewModel.UsuarioModel.Cpf = Methods.CleanString(usuarioViewModel.UsuarioModel.Cpf);
            usuarioViewModel.UsuarioModel.Senha = Criptography.GeneratePasswordHash(usuarioViewModel.UsuarioModel.Senha);

            try
            {
                _usuarioService.Insert(usuarioViewModel);
                TempData["mensagemSucesso"] = "Usuário criado com sucesso!";
                return RedirectToAction("Index", "Usuario");
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
                return View(usuarioViewModel);
            }
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = TipoUsuarioModel.ROLE_ADMIN)]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var usuario = _usuarioService.GetById((uint)id);
                if (usuario != null)
                {
                    // 1. Remover do Identity primeiro
                    var identityUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Cpf == usuario.Cpf);
                    if (identityUser != null)
                    {
                        await _userManager.DeleteAsync(identityUser);
                    }

                    // 2. Remover do sistema legado
                    if (_usuarioService.Remove(id))
                    {
                        TempData["mensagemSucesso"] = "Usuário removido com sucesso!";
                    }
                    else
                    {
                        TempData["mensagemErro"] = "Houve um problema ao remover usuário, tente novamente em alguns minutos";
                    }
                }
            }
            catch (ServiceException se)
            {
                TempData["mensagemErro"] = se.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private string GetRoleByTipoUsuario(string descricaoTipo)
        {
            return descricaoTipo?.ToUpper() switch
            {
                "ADMIN" => TipoUsuarioModel.ROLE_ADMIN,
                "COLABORADOR" => TipoUsuarioModel.ROLE_COLABORADOR,
                "GESTOR" => TipoUsuarioModel.ROLE_GESTOR,
                "PENDENTE" => TipoUsuarioModel.ROLE_PENDENTE,
                _ => TipoUsuarioModel.ROLE_PENDENTE
            };
        }
    }
}