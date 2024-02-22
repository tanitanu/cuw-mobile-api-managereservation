using Autofac;
using Autofac.Features.AttributeFilters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Converters;
using Serilog;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.Shopping;
using United.Ebs.Logging.Enrichers;
using United.Mobile.DataAccess.CMSContent;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.SeatPreference;
using United.Mobile.DataAccess.Shopping;
using United.Mobile.Model;
using United.Mobile.SeatPreference.Domain;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Middleware;

namespace United.Mobile.SeatPreference.Api
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
            services.AddControllers();
            services.AddControllers().AddJsonOptions(opts => opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddHttpContextAccessor();
            services.AddScoped<IHeaders, Common.Helper.Headers>();
            services.AddTransient<ISeatPreferenceBusiness, SeatPreferenceBusiness>();
            services.AddScoped<ISessionHelperService, SessionHelperService>();
            services.AddTransient<IShoppingUtility, ShoppingUtility>();
            services.AddTransient<IFFCShoppingcs, FFCShopping>();
            services.AddTransient<IShoppingSessionHelper, ShoppingSessionHelper>();
            services.AddTransient<IDataPowerFactory, DataPowerFactory>();
            services.AddTransient<IShoppingBuyMiles, ShoppingBuyMiles>();
            services.AddScoped<CacheLogWriter>();
            services.AddScoped(typeof(ICacheLog<>), typeof(CacheLog<>));
            services.AddTransient<IAuroraMySqlService, AuroraMySqlService>();
            services.AddSingleton<IFeatureSettings, FeatureSettings>();
            services.AddSingleton<IAWSSecretManager, AWSSecretManager>();
            services.AddSingleton<IDataSecurity, DataSecurity>();

            if (Configuration.GetValue<bool>("IsLegalDocumentFromDynamoDB"))
                services.AddTransient<ILegalDocumentsForTitlesService, LegalDocumentForTitleServiceDynamoDB>();
        }

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.Register(c => new ResilientClient(Configuration.GetSection("dpTokenConfig").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("dpTokenConfigKey");
            container.RegisterType<DPService>().As<IDPService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("cachingConfig").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("cachingConfigKey");
            container.RegisterType<CachingService>().As<ICachingService>().WithAttributeFiltering();
            container.Register(c => new ResilientClient(Configuration.GetSection("DynamoDBClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("DynamoDBClientKey");
            container.RegisterType<DynamoDBService>().As<IDynamoDBService>().WithAttributeFiltering();
            container.Register(c => new ResilientClient(Configuration.GetSection("sessionConfig").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("sessionOnCloudConfigKey");
            container.RegisterType<SessionOnCloudService>().As<ISessionOnCloudService>().WithAttributeFiltering();

            if (!Configuration.GetValue<bool>("IsLegalDocumentFromDynamoDB"))
            {
                container.Register(c => new ResilientClient(Configuration.GetSection("LegalDocumentsOnPremSqlClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("LegalDocumentsOnPremSqlClientKey");
                container.RegisterType<LegalDocumentsForTitlesService>().As<ILegalDocumentsForTitlesService>().WithAttributeFiltering();
            }

            container.Register(c => new ResilientClient(Configuration.GetSection("OptimizelyServiceClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("OptimizelyServiceClientKey");
            container.RegisterType<OptimizelyPersistService>().As<IOptimizelyPersistService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("OnPremSQLServiceClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("ValidateHashPinOnPremSqlClientKey");
            container.RegisterType<ValidateHashPinService>().As<IValidateHashPinService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("CMSContentClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("CMSContentClientKey");
            container.RegisterType<CMSContentService>().As<ICMSContentService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("MPSignInCommonClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("MPSignInCommonClientKey");
            container.RegisterType<MPSignInCommonService>().As<IMPSignInCommonService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("AwsCustomerPreferenceClient").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("AwsCustomerPreferenceClientKey");
            container.RegisterType<CustomerPreferenceService>().As<ICustomerPreferenceService>().WithAttributeFiltering();

            container.Register(c => new ResilientClient(Configuration.GetSection("dpTokenValidateConfig").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("dpTokenValidateKey");
            container.RegisterType<DPTokenValidationService>().As<IDPTokenValidationService>().WithAttributeFiltering();

            if (Configuration.GetValue<bool>("EnableSeatPerfEnhancement"))
            {   //SeatPref call ProfileCS/UpdateTravelersInformation for TCD update
                container.Register(c => new ResilientClient(Configuration.GetSection("UpdateTravelerInfoSeatPref").Get<ResilientClientOpitons>())).Keyed<IResilientClient>("UpdateTravelerInfoSeatPrefKey");
                container.RegisterType<UpdateTravelersInformation>().As<IUpdateTravelersInformation>().WithAttributeFiltering();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApplicationEnricher applicationEnricher, IFeatureSettings featureSettings, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            applicationEnricher.Add(Constants.ServiceNameText, Program.Namespace);
            applicationEnricher.Add(Constants.EnvironmentText, env.EnvironmentName);
            app.MapWhen(context => string.IsNullOrEmpty(context.Request.Path) || string.Equals("/", context.Request.Path), appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    await context.Response.WriteAsync("Welcome from SeatMap Pref Microservice");
                });
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images")),
                RequestPath = "/seatpreferenceservice/images"
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "images")),
                RequestPath = new PathString("/seatpreferenceservice/images")
            });

            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            if (Configuration.GetValue<bool>("EnableFeatureSettingsChanges"))
            {
                applicationLifetime.ApplicationStarted.Register(async () => await OnStart(featureSettings));
                applicationLifetime.ApplicationStopping.Register(async () => await OnShutDown(featureSettings));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task OnStart(IFeatureSettings featureSettings)
        {
            try
            {
                await featureSettings.LoadFeatureSettings(United.Mobile.Model.Common.ServiceNames.SEATPREFERENCE.ToString());
            }
            catch { }
        }

        private async Task OnShutDown(IFeatureSettings featureSettings)
        {
            try
            {
                await featureSettings.DeleteContainerIPAdress(United.Mobile.Model.Common.ServiceNames.SEATPREFERENCE.ToString(), StaticDataLoader._ipAddress);
            }
            catch { }
        }
    }
}