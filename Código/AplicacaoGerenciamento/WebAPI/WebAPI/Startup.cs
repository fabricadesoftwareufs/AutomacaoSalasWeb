using Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped<BlocoService>();
            services.AddScoped<HardwareDeBlocoService>();
            services.AddScoped<HardwareDeSalaService>();
            services.AddScoped<HorarioSalaService>();
            services.AddScoped<OrganizacaoService>();
            services.AddScoped<SalaService>();
            services.AddScoped<SalaParticularService>();
            services.AddScoped<PlanejamentoService>();
            services.AddScoped<TipoHardwareService>();
            services.AddScoped<TipoUsuarioService>();
            services.AddScoped<UsuarioOrganizacaoService>();
            services.AddScoped<UsuarioService>();

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
