using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Persistence;
using SalasWeb.Data;
using SalasWeb.Helpers;
using SalasWeb.Middlewares;
using Service;
using Service.Interface;
using System.Threading.Tasks;

namespace SalasUfsWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<SalasDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("UsuariosConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // Configurações para reset de senha
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddScoped<IOrganizacaoService, OrganizacaoService>();
            services.AddScoped<IBlocoService, BlocoService>();
            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<IPlanejamentoService, PlanejamentoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ITipoHardwareService, TipoHardwareService>();
            services.AddScoped<IHardwareDeSalaService, HardwareDeSalaService>();
            services.AddScoped<ICodigoInfravermelhoService, CodigoInfravermelhoService>();
            services.AddScoped<IOperacaoCodigoService, OperacaoCodigoService>();
            services.AddScoped<IEquipamentoService, EquipamentoService>();
            services.AddScoped<ISalaParticularService, SalaParticularService>();
            services.AddScoped<IUsuarioOrganizacaoService, UsuarioOrganizacaoService>();
            services.AddScoped<IHorarioSalaService, HorarioSalaService>();
            services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
            services.AddScoped<IMonitoramentoService, MonitoramentoService>();
            services.AddScoped<ICodigoInfravermelhoService, CodigoInfravermelhoService>();
            services.AddScoped<IEquipamentoService, EquipamentoService>();
            services.AddScoped<ILogRequestService, LogRequestService>();
            services.AddScoped<IConexaoInternetService, ConexaoInternetService>();
            services.AddScoped<IConexaoInternetSalaService, ConexaoInternetSalaService>();
            services.AddScoped<IMarcaEquipamentoService, MarcaEquipamentoService>();
            services.AddScoped<IModeloEquipamentoService, ModeloEquipamentoService>();

            
            services.AddTransient<IEmailSender, EmailSender>();

            // Adicionar Razor Pages
            services.AddRazorPages();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseLogRequestMiddleware();
            app.UseStaticFiles();
            app.UseRouting();

            // Forçando a utilizar autenticação.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); // Adicionar suporte a Razor Pages
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                CriarRoles(roleManager).Wait();
            }
        }

        private static async Task CriarRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = TipoUsuarioModel.ALL_ROLES2.Split(", ");
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}