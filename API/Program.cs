using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using System;

using System.Threading;

namespace API
{
    public class Program
    {
        private static readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            logger.Info("Iniciando o aplicativo");

            try
            {
                var host = CreateWebHostBuilder(args).Build();
                host.RunAsync(cancelTokenSource.Token).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Aplicação parou de rodar.");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().UseNLog();

        public static void Shutdown()
        {
            cancelTokenSource.Cancel();
        }
    }
}
