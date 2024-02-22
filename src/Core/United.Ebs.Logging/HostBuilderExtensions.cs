using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using United.Csl.Ms.Common.Interfaces;
using United.Ebs.Logging.Enrichers;
using United.Ebs.Logging.Providers;

namespace United.Ebs.Logging
{
    public static class HostBuilderExtensions
    {
        public static IServiceCollection ConfigureEbsLogger(this WebHostBuilderContext context, IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext()
                 .Enrich.With(new LogEventEnricher())
                 .CreateLogger();

            services.AddScoped<IApplicationEnricher, ApplicationEnricher>();
            services.AddScoped<IRequestEnricher, RequestEnricher>();
            services.AddScoped<IRequestContext, RequestContext>();
            services.Configure<LoggingConfiguration>(op => context.Configuration.GetSection("Logging:CSLLogging").Bind(op));

            return services;
        }

        // Added by Ashrith to support Worker Service (Background Service)
        public static IServiceCollection ConfigureEbsLogger(this HostBuilderContext context, IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext()
                 .Enrich.With(new LogEventEnricher())
                 .CreateLogger();

            services.AddScoped<IContextEnricher, ContextEnricher>();
            services.Configure<LoggingConfiguration>(op => context.Configuration.GetSection("Logging:CSLLogging").Bind(op));

            return services;
        }

        public static ILoggingBuilder AddEbsLogger(this ILoggingBuilder loggingBuilder)
        {
            if (loggingBuilder == null) throw new ArgumentNullException(nameof(loggingBuilder));

            loggingBuilder.AddSerilog(null, true);

            return loggingBuilder;
        }
    }
}
