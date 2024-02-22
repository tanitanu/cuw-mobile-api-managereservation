using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OptimizelySDK;
using OptimizelySDK.Logger;
using OptimizelySDK.OptimizelyDecisions;
using System;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Shopping;
using United.Mobile.Model.Catalog;
using United.Utility.Helper;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace United.Common.Helper.Shopping
{
    public sealed class ExperimentProvider
    {
        private readonly ICacheLog _logger;
        private readonly IConfiguration _configuration;
        private readonly ICachingService _cachingService;
        private readonly IOptimizelyPersistService _optimizelyPersistService;
        //private static readonly Lazy<ExperimentProvider> lazy =
        //new Lazy<ExperimentProvider>(() => new ExperimentProvider());
        private static OptimizelySDK.Logger.ILogger logger = new DefaultLogger();
        private static Optimizely optimizely = null;
        //  public static ExperimentProvider Instance { get { return lazy.Value; } }
        public static string projJson = "";
        public static string url = "";
        private readonly IHeaders _headers;
        public ExperimentProvider(ICacheLog logger
            , IConfiguration configuration
            , ICachingService cachingService
            , IOptimizelyPersistService optimizelyPersistService
            , IHeaders headers
            , bool refreshCache = false)
        {
            _logger = logger;
            _configuration = configuration;
            _cachingService = cachingService;
            _optimizelyPersistService = optimizelyPersistService;
            _headers = headers;
            url = string.Format(_configuration.GetValue<string>("OptimizelyURL") + "{0}.json", _configuration.GetValue<string>("ExperimentSdkKey"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _logger.LogInformation("DisablePersistForExperiment = {DisablePersistForExperiment}", _configuration.GetValue<bool>("DisablePersistForExperiment"));
            if (_configuration.GetValue<bool>("DisablePersistForExperiment"))
            {
                //using (var client = new System.Net.WebClient())
                //    projJson = FormatJson(client.DownloadString(url));
                var result = _optimizelyPersistService.GetFormatJson(_headers.ContextValues.SessionId).Result;
                projJson = FormatJson(result);
            }
            else
            {
                _logger.LogInformation("start DisablePersistForExperiment");

                 DownLoadOptimizelyProjJson(refreshCache);
            }
            var decideOptions = new OptimizelyDecideOption[] { OptimizelyDecideOption.IGNORE_USER_PROFILE_SERVICE };
            if (!string.IsNullOrEmpty(projJson))
                optimizely = new Optimizely(projJson, defaultDecideOptions: decideOptions);

        }

        private async Task DownLoadOptimizelyProjJson(bool refreshCache = false)
        {
            var projJsonPersist =await _cachingService.GetCache<string>(_configuration.GetValue<string>("OptimizelyProjectJSONPersistKey") + "OptimizelyProjectJSON", _headers.ContextValues.TransactionId).ConfigureAwait(false);
            if (string.IsNullOrEmpty(projJsonPersist) == false && refreshCache == false)
            {
                projJson = projJsonPersist;
                return;
            }
            //using (var client = new System.Net.WebClient())
            //    projJson = FormatJson(client.DownloadString(url));
            var result =await _optimizelyPersistService.GetFormatJson(_headers.ContextValues.SessionId).ConfigureAwait(false);
            projJson = FormatJson(result);
            await _cachingService.SaveCache<string>(_configuration.GetValue<string>("OptimizelyProjectJSONPersistKey") + "OptimizelyProjectJSON", projJson, _headers.ContextValues.TransactionId, new TimeSpan(1, 30, 0));
        }

        private static string FormatJson(string json)
        {
            // de-serialize, then re-serialize to format
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(json), Formatting.Indented);
        }
    }
}

