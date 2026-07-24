using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SalasUfsWeb.Logging;
using System;
using System.IO;

namespace SalasUfsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                WriteStartupFailure(exception);
                throw;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddProvider(new TextFileLoggerProvider());
                    logging.AddFilter<TextFileLoggerProvider>(null, LogLevel.Trace);
                })
                .UseStartup<Startup>();

        private static void WriteStartupFailure(Exception exception)
        {
            Console.Error.WriteLine("Falha critica ao iniciar o SalasWeb.");
            Console.Error.WriteLine(exception);

            try
            {
                var logsDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                Directory.CreateDirectory(logsDirectory);

                var logPath = Path.Combine(logsDirectory, "startup-fatal.log");
                File.AppendAllText(
                    logPath,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Falha critica ao iniciar o SalasWeb.{Environment.NewLine}{exception}{Environment.NewLine}{Environment.NewLine}");
            }
            catch (Exception logException)
            {
                Console.Error.WriteLine("Nao foi possivel gravar startup-fatal.log.");
                Console.Error.WriteLine(logException);
            }
        }
    }
}
