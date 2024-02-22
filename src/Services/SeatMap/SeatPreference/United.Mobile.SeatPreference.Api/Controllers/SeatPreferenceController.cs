using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.SeatMap;
using United.Mobile.SeatPreference.Domain;
using United.Utility.AppVersion;
using United.Utility.Helper;

namespace United.Mobile.SeatPreference.Api.Controllers
{
    [Route("seatpreferenceservice/api")]
    [ApiController]
    public class SeatPreferenceController : ControllerBase
    {
        private readonly ICacheLog<SeatPreferenceController> _logger;
        private readonly IHeaders _headers;
        private readonly ISeatPreferenceBusiness _seatPrefBusiness;
        private readonly IRequestEnricher _requestEnricher;
        private readonly IConfiguration _configuration;
        private readonly IFeatureSettings _featureSettings;
        public readonly string Namespace = typeof(Program).Namespace;
        public SeatPreferenceController(
            ICacheLog<SeatPreferenceController> logger
            , IHeaders headers
            , ISeatPreferenceBusiness offersBusiness
            , IRequestEnricher requestEnricher
            , IConfiguration configuration
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _headers = headers;
            _configuration = configuration;
            _seatPrefBusiness = offersBusiness;
            _requestEnricher = requestEnricher;
            _featureSettings = featureSettings;
            _requestEnricher.Add("Application", Namespace);
            _requestEnricher.Add("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        /// <summary>
        /// Get Seat Preference
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetSeatPreference")]
        public async Task<Response<PersistSeatPreferenceResponse>> GetSeatPreference([FromBody] Model.Common.OnPremise.Request<PersistSeatPreferenceRequest> request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major.ToString(), request.TransactionId, request.LanguageCode, string.Empty).ConfigureAwait(false);

            var response = new Response<PersistSeatPreferenceResponse>
            {
                TransactionId = request.TransactionId
            };

            _logger.LogInformation("SeatPreference/GetSeatPreference {@ClientRequest}", JsonConvert.SerializeObject(request));

            Stopwatch _stopWatch = new Stopwatch();
            _stopWatch.Start();

            try
            {
                if (ValidateBaseRequest(request))
                {
                    var enableSeatPrefEnhancement = await _featureSettings.GetFeatureSettingValue("EnableSeatPrefEnhancement");
                    if (!enableSeatPrefEnhancement) enableSeatPrefEnhancement = _configuration.GetValue<bool>("EnableSeatPerfEnhancement");
                    if (enableSeatPrefEnhancement && GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.ToString(),
                        "EnableSeatPerfEnhancement_AppVersion_Android", "EnableSeatPerfEnhancement_AppVersion_Iphone", "", "", true, _configuration))
                        response.Data = await _seatPrefBusiness.GetSeatPreferencefromCSLV2(request.Data, request.Application.Id, request.DeviceId, request.TransactionId, request.AccessCode);
                    else
                        response.Data = await _seatPrefBusiness.GetSeatPreferencefromCSL(request.Data, request.Application.Id, request.DeviceId, request.TransactionId, request.AccessCode);
                }
                else
                {
                    response._Errors = new Dictionary<string, string>
                    {

                        {  HttpStatusCode.BadRequest.ToString(), "Bad Request" }
                    };

                    _logger.LogError("SeatPreference/GetSeatPreference Exception - Bad Request");
                }
            }
            catch (Exception ex)
            {
                response._Errors = new Dictionary<string, string>
                {
                    { HttpStatusCode.InternalServerError.ToString(), ex.Message }
                };

                _logger.LogError("SeatPreference/GetSeatPreference Exception {@message} and {@StackTrace}", ex.Message, ex.StackTrace);
            }

            _stopWatch.Stop();
            response.Duration = _stopWatch.ElapsedMilliseconds;

            _logger.LogInformation("SeatPreference/GetSeatPreference {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        /// <summary>
        /// Save Seat Preference
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SaveSeatPreference")]
        public async Task<Response<PostSeatPreferenceResponse>> SaveSeatPreference([FromBody] Model.Common.OnPremise.Request<PostSeatPreferenceRequest> request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major.ToString(), request.TransactionId, request.LanguageCode, string.Empty).ConfigureAwait(false);

            var response = new Response<PostSeatPreferenceResponse>
            {
                TransactionId = request.TransactionId
            };

            _logger.LogInformation("SeatPreference/SaveSeatPreference {@ClientRequest}", JsonConvert.SerializeObject(request));

            Stopwatch _stopWatch = new Stopwatch();
            _stopWatch.Start();

            try
            {
                if (ValidateBaseRequest(request))
                {
                    var enableSeatPrefEnhancement = await _featureSettings.GetFeatureSettingValue("EnableSeatPrefEnhancement");
                    if (!enableSeatPrefEnhancement) enableSeatPrefEnhancement = _configuration.GetValue<bool>("EnableSeatPerfEnhancement");
                    var preferenceBusinessResponse = new PostSeatPreferenceResponse();
                    MOBApplication MobApplication = new MOBApplication()
                    {
                        Id = request.Application.Id,
                        Version = new MOBVersion
                        {
                            Major = request.Application.Version.Major.ToString(),
                            Minor = request.Application.Version.Minor.ToString(),
                        },
                        Name = request.Application.Name,
                    };
                    if (enableSeatPrefEnhancement && GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.ToString(),
                        "EnableSeatPerfEnhancement_AppVersion_Android", "EnableSeatPerfEnhancement_AppVersion_Iphone", "", "", true, _configuration))
                        preferenceBusinessResponse = await _seatPrefBusiness.SaveSeatPreferencetToCSLV2(request.Data, request.Application.Id, request.DeviceId, request.TransactionId, request.AccessCode, MobApplication, request.LanguageCode);
                    else
                        preferenceBusinessResponse = await _seatPrefBusiness.SaveSeatPreferencetToCSL(request.Data, request.Application.Id, request.DeviceId, request.TransactionId, request.AccessCode);

                    response.Data = preferenceBusinessResponse;
                }
                else
                {
                    response._Errors = new Dictionary<string, string>
                    {

                        {  HttpStatusCode.BadRequest.ToString(), "Bad Request" }
                    };

                    _logger.LogError("SeatPreference/SaveSeatPreference Exception - BadRequest");
                }
            }
            catch (Exception ex)
            {
                response._Errors = new Dictionary<string, string>
                {
                    {  HttpStatusCode.InternalServerError.ToString(), ex.Message }
                };

                _logger.LogError("SeatPreference/SaveSeatPreference Exception {@Message} and {@StackTrace}", ex.Message, ex.StackTrace);
            }

            _stopWatch.Stop();
            response.Duration = _stopWatch.ElapsedMilliseconds;

            _logger.LogInformation("Response - SeatPreference/SaveSeatPreference = {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        private bool ValidateBaseRequest<T>(Model.Common.OnPremise.Request<T> request)
        {
            if (request != null
                && request.Application != null
                && request.Application.Version != null
                && !string.IsNullOrEmpty(request.DeviceId)
                && !string.IsNullOrEmpty(request.TransactionId)
                && request.Data != null)
                return true;

            return default;
        }

        [HttpGet]
        [Route("HealthCheck")]
        public string HealthCheck()
        {
            return "Healthy";
        }

        [HttpGet]
        [Route("Version")]
        public string Version()
        {
            string serviceVersionNumber = null;

            try
            {
                serviceVersionNumber = System.Environment.GetEnvironmentVariable("SERVICE_VERSION_NUMBER");
            }
            catch
            {
                // Suppress any exceptions
            }
            finally
            {
                serviceVersionNumber = (null == serviceVersionNumber) ? "Unable to retrieve the version number" : serviceVersionNumber;
            }

            return serviceVersionNumber;
        }

        [HttpGet]
        [Route("environment")]
        public virtual string ApiEnvironment()
        {
            try
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }
            catch
            {
            }
            return "Unknown";
        }

        [HttpGet("GetFeatureSettings")]
        public GetFeatureSettingsResponse GetFeatureSettings()
        {
            GetFeatureSettingsResponse response = new GetFeatureSettingsResponse();

            try
            {
                response = _featureSettings.GetFeatureSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError("GetFeatureSettings Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), "GetFeatureSettings_TransId");
                response.Exception = new MOBException("9999", JsonConvert.SerializeObject(ex));
            }

            return response;
        }

        [HttpPost("RefreshFeatureSettingCache")]
        public async Task<MOBResponse> RefreshFeatureSettingCache(MOBFeatureSettingsCacheRequest request)
        {
            MOBResponse response = new MOBResponse();

            try
            {
                request.ServiceName = ServiceNames.SEATPREFERENCE.ToString();
                await _featureSettings.RefreshFeatureSettingCache(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshFeatureSettingCache Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), "RefreshRetrieveAllFeatureSettings_TransId");
                response.Exception = new MOBException("9999", JsonConvert.SerializeObject(ex));
            }

            return response;
        }
    }
}