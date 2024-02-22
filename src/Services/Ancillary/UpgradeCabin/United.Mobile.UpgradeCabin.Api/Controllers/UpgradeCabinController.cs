using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.Shopping;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.UpgradeCabin;
using United.Mobile.UpgradeCabin.Api;
using United.Mobile.UpgradeCabin.Domain;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.Services.UpgradeCabin.Api.Controllers
{
    [Route("upgradecabinservice/api")]
    [ApiController]
    public class UpgradeCabinController : ControllerBase
    {
        private readonly ICacheLog<UpgradeCabinController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly IRequestEnricher _requestEnricher;
        public readonly string Namespace = typeof(Program).Namespace;
        private static readonly string _UPGRADEMALL = "UPGRADEMALL";
        private readonly IUpgradeCabinBusiness _upgradeCabin;
        private readonly IHeaders _headers;
        private readonly IFeatureSettings _featureSettings;

        public UpgradeCabinController(ICacheLog<UpgradeCabinController> logger, IConfiguration configuration,
            IShoppingSessionHelper shoppingSessionHelper, 
            IUpgradeCabinBusiness upgradeCabin, IHeaders headers, IRequestEnricher requestEnricher
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _shoppingSessionHelper = shoppingSessionHelper;
            _upgradeCabin = upgradeCabin;
            _headers = headers;
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
        [Route("version")]
        public virtual string Version()
        {
            string serviceVersionNumber = null;

            try
            {
                serviceVersionNumber = Environment.GetEnvironmentVariable("SERVICE_VERSION_NUMBER");
            }
            catch
            {
                // Suppress any exceptions
            }
            finally
            {
                serviceVersionNumber = (null == serviceVersionNumber) ? "0.0.0" : serviceVersionNumber;
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
        [HttpPost("UpgradeCabin/UpgradePlusPointWebMyTrip")]
        public async Task<MOBUpgradePlusPointWebMyTripResponse> UpgradePlusPointWebMyTrip(MOBUpgradePlusPointWebMyTripRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBUpgradePlusPointWebMyTripResponse();

            IDisposable timer = null;

            try
            {
                _logger.LogInformation("UpgradePlusPointWebMyTrip - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for UpgradePlusPointWebMyTrip business call", transationId: request.TransactionId))
                {
                    response = await _upgradeCabin.UpgradePlusPointWebMyTrip(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("UpgradePlusPointWebMyTrip Warning {exception} and {exceptionstack}", uaex.Message,JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("UpgradePlusPointWebMyTrip Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException();
                response.Exception.Message = ex.Message;
            }

            response.TransactionId = request.TransactionId;
            response.LanguageCode = request.LanguageCode;
            response.SessionId = request.SessionId;

            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("UpgradePlusPointWebMyTrip {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost("UpgradeCabin/UpgradeCabinEligibleCheck")]
        public async Task<MOBUpgradeCabinEligibilityResponse> UpgradeCabinEligibleCheck(MOBUpgradeCabinEligibilityRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBUpgradeCabinEligibilityResponse();

            IDisposable timer = null;

            try
            {
                _logger.LogInformation("UpgradeCabinEligibleCheck - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for UpgradeCabinEligibleCheck business call", transationId: request.TransactionId))
                {
                    response = await _upgradeCabin.UpgradeCabinEligibleCheck(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("UpgradeCabinEligibleCheck Warning {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("UpgradeCabinEligibleCheck Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("UpgradeCabinEligibleCheck {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }


        [HttpPost("UpgradeCabinRegisterOffer")]
        public async Task<MOBUpgradeCabinRegisterOfferResponse> UpgradeCabinRegisterOffer([FromBody] MOBUpgradeCabinRegisterOfferRequest request)
        {
            var response = new MOBUpgradeCabinRegisterOfferResponse();
            string loggingId = (string.IsNullOrEmpty(request.SessionId)) ? request.TransactionId : request.SessionId;

            if (string.IsNullOrEmpty(request.SessionId) || string.IsNullOrEmpty(request.CartId))
            {
                // response.Exception = MOBUnitedExceptionHandler request, _controllerUtility, request.SessionId, actionname: "UpgradeCabinRegisterOffer");
            }

            Session session = new Session();
            session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
            request.FlowType = _UPGRADEMALL;
            request.Token = session.Token;

            try
            {
                response = await Task.Run(() => _upgradeCabin.UpgradeCabinRegisterOfferAsync(request, session));
            }
            catch (MOBUnitedException uaex)
            {
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

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
                request.ServiceName = ServiceNames.UPGRADECABIN.ToString();
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
