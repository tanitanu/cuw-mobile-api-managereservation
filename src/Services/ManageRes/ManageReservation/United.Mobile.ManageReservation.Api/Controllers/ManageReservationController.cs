using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.ManageRes;
using United.Ebs.Logging.Enrichers;
using United.Mobile.ManageReservation.Domain;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.ManageReservation.Api.Controllers
{
    [Route("managereservationservice/api")]
    [ApiController]
    public class ManageReservationController : ControllerBase
    {
        private readonly ICacheLog<ManageReservationController> _logger;
        private readonly IHeaders _headers;
        private readonly IConfiguration _configuration;
        private readonly IManageReservationBusiness _manageReservationBusiness;
        private readonly IFlightReservation _flightReservation;
        private readonly IRequestEnricher _requestEnricher;
        public readonly string Namespace = typeof(Program).Namespace;
        private readonly IFeatureSettings _featureSettings;
        public ManageReservationController(ICacheLog<ManageReservationController> logger
            , IConfiguration configuration
            , IHeaders headers
            , IManageReservationBusiness manageReservationBusiness
            , IFlightReservation flightReservation
            , IRequestEnricher requestEnricher
            , IFeatureSettings featureSettings
            )
        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _manageReservationBusiness = manageReservationBusiness;
            _flightReservation = flightReservation;
            _featureSettings = featureSettings;
            _requestEnricher = requestEnricher;
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
        [HttpPost]
        [Route("ManageReservation/GetPNRByRecordLocator")]
        public async Task<MOBPNRByRecordLocatorResponse> GetPNRByRecordLocator(MOBPNRByRecordLocatorRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBPNRByRecordLocatorResponse();
            IDisposable timer = null;

            try
            {
                _logger.LogInformation("GetPNRByRecordLocator {@ClientRequest}", JsonConvert.SerializeObject(request));
                using (timer = _logger.BeginTimedOperation("Total time taken for GetPNRByRecordLocator business call", transationId: request.SessionId))
                {
                    response.LanguageCode = request.LanguageCode;
                    response.TransactionId = request.TransactionId;
                    response = await _manageReservationBusiness.GetPNRByRecordLocator(request);
                }
            }

            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetPNRByRecordLocator Warning {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            catch (System.Exception ex)
            {
                _logger.LogError("GetPNRByRecordLocator Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", "We are unable to retrieve the latest information for this itinerary.");
            }

            if (response.Exception != null)
            {
                //ALM# 27193 - Changed the Message as petr the ALM comments by Hueramo, Carla 
                string exMessage = response.Exception.Message;
                //if (exMessage.Trim().IndexOf("The confirmation number entered is invalid.") > -1 || exMessage.Trim().IndexOf("Please enter a valid record locator.") > -1)
                if (exMessage.Trim().IndexOf("The confirmation number entered is invalid.") > -1 || exMessage.Trim().IndexOf("Please enter a valid record locator.") > -1 || exMessage.Trim().IndexOf("The last name you entered, does not match the name we have on file.") > -1 || exMessage.Trim().IndexOf("Please enter a valid last name") > -1)
                {
                    exMessage = _configuration.GetValue<string>("ExceptionMessageForInvalidPNROrInvalidLastName");
                }
                string exCode = response.Exception.Code;
                response = new MOBPNRByRecordLocatorResponse(_configuration);
                response.Exception = new MOBException(exCode, exMessage);
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GetPNRByRecordLocator {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("ManageReservation/PerformInstantUpgrade")]
        public async Task<MOBPNRByRecordLocatorResponse> PerformInstantUpgrade(MOBInstantUpgradeRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBPNRByRecordLocatorResponse();
            IDisposable timer = null;

            try
            {
                _logger.LogInformation("PerformInstantUpgrade {@ClientRequest}", JsonConvert.SerializeObject(request));
                using (timer = _logger.BeginTimedOperation("Total time taken for PerformInstantUpgrade business call", transationId: request.SessionId))
                {
                    response.LanguageCode = request.LanguageCode;
                    response.TransactionId = request.TransactionId;
                    response = await _manageReservationBusiness.PerformInstantUpgrade(request);
                }
            }

            catch (MOBUnitedException uaex)
            {
                var uaexWrapper = new MOBExceptionWrapper(uaex);
                _logger.LogWarning("PerformInstantUpgrade Warning {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaexWrapper));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            catch (System.Exception ex)
            {
                var exceptionWrapper = new MOBExceptionWrapper(ex);
                _logger.LogError("PerformInstantUpgrade Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(exceptionWrapper));
                response.Exception = new MOBException("10000", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("PerformInstantUpgrade {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("ManageReservation/GetOneClickEnrollmentDetailsForPNR")]
        public async Task<MOBOneClickEnrollmentResponse> GetOneClickEnrollmentDetailsForPNR(MOBPNRByRecordLocatorRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBOneClickEnrollmentResponse();
            IDisposable timer = null;

            try
            {
                _logger.LogInformation("GetOneClickEnrollmentDetailsForPNR {@ClientRequest}", JsonConvert.SerializeObject(request));
                using (timer = _logger.BeginTimedOperation("Total time taken for GetOneClickEnrollmentDetailsForPNR business call", transationId: request.SessionId))
                {
                    response.LanguageCode = request.LanguageCode;
                    response.TransactionId = request.TransactionId;
                    response = await _manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(request);
                }
            }

            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetOneClickEnrollmentDetailsForPNR Error {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            catch (System.Exception ex)
            {
                _logger.LogError("GetOneClickEnrollmentDetailsForPNR Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", "We are unable to retrieve the latest information for this itinerary.");
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GetOneClickEnrollmentDetailsForPNR {@ClientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        public async Task<MOBLookUpTravelCreditResponse> GetLookUpTravelCredit(MOBPNRByRecordLocatorRequest request)
        {
            await _headers.SetHttpHeader(string.Empty, request.Application.Id.ToString(), request.Application.Version.ToString(), request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBLookUpTravelCreditResponse();
            IDisposable timer = null;

            try
            {
                using (timer = _logger.BeginTimedOperation("Total time taken for GetLookUpTravelCredit business call", transationId: request.TransactionId))
                {
                    response.FutureFlightCredit = await _flightReservation.GetFutureFlightCreditMessages(request.Application.Id, request.Application.Version.Major);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogWarning("GetLookUpTravelCredit Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), request.TransactionId);
                response.Exception = new MOBException("9999", "We are unable to retrieve the Future Flight Credit Info.");
            }

            if (response.Exception != null)
            {
                string exMessage = response.Exception.Message;
                string exCode = response.Exception.Code;
                response = new MOBLookUpTravelCreditResponse();
                response.Exception = new MOBException(exCode, exMessage);
                _logger.LogInformation("GetLookUpTravelCredit {Exception}", response.Exception);
            }

            _logger.LogInformation("GetFutureFlightCreditMessages {RecordLocator} and {DeviceId} and {TravelCredit} and {response} and {request.Application}", request.RecordLocator, request.DeviceId, "GetLookUpTravelCredit", response, request.Application, "Response");

            return response;
        }

        [HttpPost]
        [Route("FlightReservation/RequestReceiptByEmail")]
        public async Task<MOBReceiptByEmailResponse> RequestReceiptByEmail(MOBReceiptByEmailRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBReceiptByEmailResponse();
            IDisposable timer = null;

            try
            {
                _logger.LogInformation("RequestReceiptByEmail {@ClientRequest}", JsonConvert.SerializeObject(request));
                using (timer = _logger.BeginTimedOperation("Total time taken for RequestReceiptByEmail business call", transationId: request.TransactionId))
                {
                    response = await _manageReservationBusiness.RequestReceiptByEmail(request);
                }
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("RequestReceiptByEmail Warning {exception} and {exceptionstack}", coex.Message, JsonConvert.SerializeObject(coex));
                response.Exception = new MOBException();
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("RequestReceiptByEmail Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("10000", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("RequestReceiptByEmail {@clientResponse}", JsonConvert.SerializeObject(response));
           
            return response;
        }

        [HttpPost("ReShopping/ConfirmScheduleChange")]
        public async Task<United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse> ConfirmScheduleChange(United.Mobile.Model.ReShop.MOBConfirmScheduleChangeRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse();

            IDisposable timer = null;
            try
            {
                using (timer = _logger.BeginTimedOperation("Total time taken for ConfirmScheduleChange business call", transationId: request.TransactionId))
                {
                    _logger.LogInformation("ConfirmScheduleChange {@ClientRequest}", JsonConvert.SerializeObject(request));
                    response = await _manageReservationBusiness.ConfirmScheduleChange(request);
                }

            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("ConfirmScheduleChange Warning {UnitedException} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("PNRConfmScheduleChangeExcMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("ConfirmScheduleChange Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("ConfirmScheduleChange {@clientResponse}", JsonConvert.SerializeObject(response));
            return response;
        }
       
        [HttpPost]
        [Route("MerchandizingServices/GetMileageAndStatusOptions")]
        public async Task<MOBMileageAndStatusOptionsResponse> GetMileageAndStatusOptions(MOBMileageAndStatusOptionsRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBMileageAndStatusOptionsResponse();

            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetMileageAndStatusOptions - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetMileageAndStatusOptions business call", transationId: request.TransactionId))
                {
                    response = await _manageReservationBusiness.GetMileageAndStatusOptions(request);
                }
            }
            catch (MOBUnitedException error)
            {
                _logger.LogWarning("GetMileageAndStatusOptions Warning {exception} and {exceptionstack}", error.Message, JsonConvert.SerializeObject(error));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception error)
            {
                _logger.LogError("GetMileageAndStatusOptions Error {exception} and {exceptionstack}", error.Message, JsonConvert.SerializeObject(error));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GetMileageAndStatusOptions {@clientResponse} {transactionId}", JsonConvert.SerializeObject(response), request.TransactionId);
            return response;
        }

        [HttpPost]
        [Route("HomeScreenOffers/GetActionDetailsForOffers")]
        public async Task<MOBGetActionDetailsForOffersResponse> GetActionDetailsForOffers(MOBGetActionDetailsForOffersRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBGetActionDetailsForOffersResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetActionDetailsForOffers - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetActionDetailsForOffers business call", transationId: request.TransactionId))
                {
                    response = await _manageReservationBusiness.GetActionDetailsForOffers(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetActionDetailsForOffers Error {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetActionDetailsForOffers Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GetActionDetailsForOffers {@clientResponse}", JsonConvert.SerializeObject(response));
            return response;
        }

        [HttpPost]
        [Route("ProductOffers/GetProductOfferAndDetails")]
        public async Task<TravelOptionsResponse> GetProductOfferAndDetails(TravelOptionsRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new TravelOptionsResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetProductOfferAndDetails - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetProductOfferAndDetails business call", transationId: request.TransactionId))
                {
                    response = await _manageReservationBusiness.GetProductOfferAndDetails(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetProductOfferAndDetails Warning {exception} and {exceptionstack}", uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetProductOfferAndDetails Error {exception} and {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GetProductOfferAndDetails {@clientResponse}", JsonConvert.SerializeObject(response));
            return response;
        }


        [HttpPost]
        [Route("ManageReservation/PostBaggageEventMessage")]
        public  void PostBaggageEventMessage(dynamic request)
        {
            try
            {
                    _logger.LogInformation("PostBaggageEventMessage - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));
                    _manageReservationBusiness.PostBaggageEventMessage(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("PostBaggageEventMessage Error {exception}", ex.Message);
               
            }

           
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
                request.ServiceName = ServiceNames.MANAGERESERVATION.ToString();
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