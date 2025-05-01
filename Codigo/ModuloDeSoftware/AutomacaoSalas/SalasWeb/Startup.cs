using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using SalasWeb.Middlewares;
using Service;
using Service.Interface;

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

            // Configuração de Autenticação.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // User nao logado, é redirecionado a essa pagina.
                    options.LoginPath = "/Login";
                    // Redirecionado pra cá, caso não haja permissão para determinada ação.
                    options.AccessDeniedPath = "/Login/AcessoNegado";
                });

            services.AddDbContext<SalasDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));

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
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();

        }
    }
}
