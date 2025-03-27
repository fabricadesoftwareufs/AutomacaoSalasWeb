using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Service;
using Service.Interface;
using System;
using System.Text;
using System.Threading.Tasks;
using SalasAPI.Middlewares;
using Utils;

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
            services.AddDbContext<SalasDBContext>(options => options.UseMySQL(Configuration.GetConnectionString("MySqlConnection")));

            // Configuração da barreira
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "SalasUfsWebApi.net",
                        ValidAudience = "SalasUfsWebApi.net",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]))
                    };
                    
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
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SalasUFS API",
                    Description = "API pública do sistema SalasUFS",
                    Contact = new OpenApiContact
                    {
                        Name = "Contato",
                        Url = new Uri("http://marcosdosea-002-site1.gtempurl.com/")
                    },
                });
            });

            // Injections
            services.AddAuthorization();
            services.AddScoped<IOrganizacaoService, OrganizacaoService>();
            services.AddScoped<IBlocoService, BlocoService>();
            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<IPlanejamentoService, PlanejamentoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ITipoHardwareService, TipoHardwareService>();
            services.AddScoped<IHardwareDeSalaService, HardwareDeSalaService>();
            services.AddScoped<ISalaParticularService, SalaParticularService>();
            services.AddScoped<ISolicitacaoService, SolicitacacaoService>();
            services.AddScoped<IUsuarioOrganizacaoService, UsuarioOrganizacaoService>();
            services.AddScoped<IHorarioSalaService, HorarioSalaService>();
            services.AddScoped<ITipoUsuarioService, TipoUsuarioService>();
            services.AddScoped<IMonitoramentoService, MonitoramentoService>();
            services.AddScoped<ICodigoInfravermelhoService, CodigoInfravermelhoService>();
            services.AddScoped<IEquipamentoService, EquipamentoService>();
            services.AddScoped<ILogRequestService, LogRequestService>();
            services.AddScoped<IConexaoInternetService, ConexaoInternetService>();
            services.AddScoped<IConexaoInternetSalaService, ConexaoInternetSalaService>();
            services.AddMvcCore(options =>
            {
                options.RequireHttpsPermanent = true; //does not affect API requests
                options.RespectBrowserAcceptHeader = true; //false by default
            })
            .AddApiExplorer()
            .AddNewtonsoftJson()
            .AddDataAnnotations()
            .AddCors();
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

            app.UseSwagger(options =>
            {
                options.SerializeAsV2 = true;
            });
            
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "swagger";
            });

            // Mudar em produção.
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseWhen(context => CheckRoutesHardware(context), appBuilder =>
             {
                 appBuilder.UseLogRequestMiddleware();
             });
            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static bool CheckRoutesHardware(HttpContext context)
        {
            PathString path = context.Request.Path;

            foreach(string route in Constants.ROUTES_HARDWARE)
            {
                if(path.StartsWithSegments(route, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
