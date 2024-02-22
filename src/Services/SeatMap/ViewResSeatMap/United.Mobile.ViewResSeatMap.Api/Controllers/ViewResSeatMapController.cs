using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.ViewResSeatMap.Domain;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.ViewResSeatMap.Api.Controllers
{
    [Route("viewresseatmapservice/api")]
    [ApiController]
    public class ViewResSeatMapController : ControllerBase
    {
        private readonly Stopwatch _stopWatch;
        private readonly ICacheLog<ViewResSeatMapController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly IViewResSeatMapBusiness _viewResSeatMapBusiness;
        private readonly IRequestEnricher _requestEnricher;
        private readonly IFeatureSettings _featureSettings;
        public readonly string Namespace = typeof(Program).Namespace;

        public ViewResSeatMapController(ICacheLog<ViewResSeatMapController> logger
            , IConfiguration configuration
            , IHeaders headers
            , IViewResSeatMapBusiness viewResSeatMapBusiness
            , IRequestEnricher requestEnricher
            , IFeatureSettings featureSettings)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _viewResSeatMapBusiness = viewResSeatMapBusiness;
            _requestEnricher = requestEnricher;
            _featureSettings = featureSettings;
            _requestEnricher.Add("Application", Namespace);
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
        [HttpGet]
        [Route("SeatMap/SelectSeats")]
        public async Task<MOBSeatChangeSelectResponse> SelectSeats(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, string catalogValues = null)
        {
            await _headers.SetHttpHeader(string.Empty, applicationId.ToString(), appVersion, transactionId, languageCode, sessionId);
            var response = new MOBSeatChangeSelectResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("SelectSeats ClientRequest {AccessCode} {LanguageCode} {AppVersion} {ApplicationId} {SessionId} {Origin} {Destination} {FlightNumber} {FlightDate} {PaxIndia} {SeatAssignment} {NextOrigin} {NextDestination}", accessCode, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

                timer = _logger.BeginTimedOperation("Total time taken for SelectSeats business call", transationId: transactionId);
                response = await _viewResSeatMapBusiness.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination, catalogValues);
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("SelectSeats Warning {UnitedException} {exceptionstack}", coex.Message, JsonConvert.SerializeObject(coex));

                response.Exception = new MOBException();
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("SelectSeats Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));

                response.Exception = new MOBException("99999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }

            _logger.LogInformation("SelectSeats {@clientResponse} {transactionId}", JsonConvert.SerializeObject(response), transactionId);

            return response;
        }

        [HttpPost]
        [Route("SeatMap/SeatChangeInitialize")]
        public async Task<MOBSeatChangeInitializeResponse> SeatChangeInitialize(MOBSeatChangeInitializeRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBSeatChangeInitializeResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("SeatChangeInitialize {@clientRequest} {SessionId} and {TransactionId}", JsonConvert.SerializeObject(request), request.SessionId, request.TransactionId);

                timer = _logger.BeginTimedOperation("Total time taken for SeatChangeInitialize business call", transationId: request.TransactionId);
                response = await _viewResSeatMapBusiness.SeatChangeInitialize(request);
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("SeatChangeInitialize Warning {UnitedException} {exceptionstack}", coex.Message, JsonConvert.SerializeObject(coex));
                response.Exception = new MOBException();
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("SeatChangeInitialize Error {exception} {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }

            _logger.LogInformation("SeatChangeInitialize {@clientResponse} {transactionId}", JsonConvert.SerializeObject(response), request.TransactionId);

            return response;
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
                request.ServiceName = ServiceNames.VIEWRESSEATMAP.ToString();
                await _featureSettings.RefreshFeatureSettingCache(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshFeatureSettingCache Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), "RefreshRetrieveAllFeatureSettings_TransId");
                response.Exception = new MOBException("9999", JsonConvert.SerializeObject(ex));
            }
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
    }
}