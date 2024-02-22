using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using United.Ebs.Logging;


namespace United.Mobile.Services.GooglePay.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static int Main(string[] args)
        {
            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                CreateHostBuilder(args).Build().Run();
                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
          .ConfigureAppConfiguration((hostContext, config) =>
          {
              config.AddJsonFile("appsettings.json", optional: false);
              config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
          })
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.ConfigureServices((context, services) =>
              {
                  services.AddHttpContextAccessor();
                  context.ConfigureEbsLogger(services);
              })
              .ConfigureLogging(x =>
              {
                  x.ClearProviders();
                  x.AddEbsLogger();
              });
              webBuilder.UseStartup<Startup>();
          });
    }
}
