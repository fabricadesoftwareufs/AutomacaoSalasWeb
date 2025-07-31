using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Model;
using System.Threading.Tasks;

namespace SalasUfsWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = CreateWebHostBuilder(args).Build();
            app.Run();
        }



        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
