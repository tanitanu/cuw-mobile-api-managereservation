using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using United.Ebs.Logging;

namespace United.Mobile.UpgradeCabin.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            string runType = "1";

            try
            {
                switch (runType)
                {
                    case "1":
                        CreateHostBuilder(args).Build().Run();
                        break;
                    default:
                        var configuration = GetConfiguration();
                        Log.Logger = CreateSerilogLogger(configuration);

                        // CreateWebHostBuilder(args).Build().Run();
                        Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                        var host = BuildWebHost(configuration, args);

                        Log.Information("Starting web host ({ApplicationContext})...", AppName);
                        host.Run();
                        break;
                }

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
         .UseSerilog()
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


        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .CaptureStartupErrors(false)
               .UseStartup<Startup>()
               //.UseApplicationInsights()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .UseConfiguration(configuration)
               //  .UseSerilog()
               .Build();

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", "")
                .Enrich.FromLogContext()
                 .WriteTo.Console()
                //.ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
