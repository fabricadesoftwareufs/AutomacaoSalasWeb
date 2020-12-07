using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Service;
using Service.Interface;
using System.Text;

namespace WebAPI
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
            services.AddDbContext<STR_DBContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));

            // Configuração da barreira
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Gabriel",
                        ValidAudience = "Gabriel",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };

                    // Configurando eventos.
                    /*
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("Token invalido!!");
                            return Task.CompletedTask;
                        },

                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("TOKEN VALIDO");
                            return Task.CompletedTask;
                        }
                    };
                    */
                });

            // Injections
            services.AddScoped<IOrganizacaoService, OrganizacaoService>();
            services.AddScoped<IBlocoService, BlocoService>();
            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<IPlanejamentoService, PlanejamentoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IHardwareDeBlocoService, HardwareDeBlocoService>();
            services.AddScoped<ITipoHardwareService, TipoHardwareService>();
            services.AddScoped<IHardwareDeSalaService, HardwareDeSalaService>();
            services.AddScoped<ISalaParticularService, SalaParticularService>();
            services.AddScoped<IUsuarioOrganizacaoService, UsuarioOrganizacaoService>();
            services.AddScoped<IHorarioSalaService, HorarioSalaService>();
            services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
            services.AddScoped<IMonitoramentoService, MonitoramentoService>();
            services.AddScoped<ICodigoInfravermelhoService, CodigoInfravermelhoService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
                app.UseHsts();
            }

            // Mudar em produção.
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
