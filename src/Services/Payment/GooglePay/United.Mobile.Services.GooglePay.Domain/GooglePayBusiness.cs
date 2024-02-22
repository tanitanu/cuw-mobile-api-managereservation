using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Payment;
using United.Mobile.Model.Common;
using United.Mobile.Model.GooglePay;
using United.Utility.Helper;

namespace United.Mobile.Services.GooglePay.Domain
{
    public class GooglePayBusiness : IGooglePayBusiness
    {
        private readonly ICacheLog<GooglePayBusiness> _logger;
        private readonly IConfiguration _configuration;
        private static string REGEX_MP_PREMIER_LVL = @"([a-zA-Z0-9]+)\s([^$]+)";
        private readonly IGooglePayAccessTokenService _googlePayAccessTokenService;
        private readonly IFlightClassService _flightClassService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IHostingEnvironment _currentEnvironment;
        private readonly IHeaders _headers;

        public GooglePayBusiness(ICacheLog<GooglePayBusiness> logger
            , IConfiguration configuration
            , IGooglePayAccessTokenService googlePayAccessTokenService
            , IFlightClassService flightClassService
            , IDynamoDBService dynamoDBService
            , IHostingEnvironment env
            , IHeaders headers)
        {
            _logger = logger;
            _configuration = configuration;
            _googlePayAccessTokenService = googlePayAccessTokenService;
            _flightClassService = flightClassService;
            _dynamoDBService = dynamoDBService;
            _currentEnvironment = env;
            _headers = headers;
        }

        #region InsertFlight
        public async Task<MOBGooglePayFlightResponse> InsertFlight(MOBGooglePayFlightRequest request)
        {
            MOBGooglePayFlightResponse response = new MOBGooglePayFlightResponse();
            string jsonWebToken = string.Empty;
            string jsonWebTokenWithPayload = string.Empty;
            string classResourceId = string.Empty;
            string objectResourceId = string.Empty;
            string flightClassId = string.Empty;
            string flightObjectId = string.Empty;

            _logger.LogInformation("GooglePay_InsertFlight {Request} {transactionId}", request, request.TransactionId);

            flightClassId = request.FlightClassId;
            flightObjectId = request.FlightObjectId;

            //string GooglePayCertPath = string.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("~"), ConfigurationManager.AppSettings["GooglePayCertPath"].ToString());
            string GooglePayCertPath = string.Format("{0}\\{1}", _currentEnvironment.ContentRootPath, _configuration.GetValue<string>("GooglePayCertPath"));
            GooglePayCertPath = GooglePayCertPath.Replace("\\", "/");
            jsonWebToken = GenerateJsonWebToken(GooglePayCertPath);
            string jsonAccessToken = await GetAccessToken(jsonWebToken);
            AccessTokenClass accessTokenObj = JsonConvert.DeserializeObject<AccessTokenClass>(jsonAccessToken);
            string token = accessTokenObj.token_type + " " + accessTokenObj.access_token;

            if (string.IsNullOrEmpty(flightClassId))
                classResourceId = await InsertFlightClass(request, token);
            else
                classResourceId = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), flightClassId);

            if (string.IsNullOrEmpty(flightObjectId))
                objectResourceId = await InsertFlightObject(request, token, classResourceId);
            else
                objectResourceId = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), flightObjectId);

            PayloadObject payload = new PayloadObject();
            Flightobject flightObject = new Flightobject();
            flightObject.id = objectResourceId;
            flightObject.classId = classResourceId;
            payload.flightObjects = new Flightobject[] { flightObject };

            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            jsonWebTokenWithPayload = GenerateJsonWebTokenWithPayload(jsonPayload, GooglePayCertPath);

            InsertGooglePayPasses(request.ConfirmationCode, classResourceId.Split('.')[1], objectResourceId.Split('.')[1], request.FilterKey, request.DeleteKey, Convert.ToInt32(request.Application.Id), request.Application.Version.Major, request.DeviceId, Newtonsoft.Json.JsonConvert.SerializeObject(request));

            response.Save2GoogleUrl = string.Format(_configuration.GetValue<string>("Save2GoogleUrl"), jsonWebTokenWithPayload);
            response.ClassResourceId = classResourceId.Split('.')[1];
            response.ObjectResourceId = objectResourceId.Split('.')[1];
            response.TransactionId = request.TransactionId;

            _logger.LogInformation("GooglePay_InsertFlight {@response} {transactionId}", response, request.TransactionId);

            return await Task.FromResult(response);
        }

        private async Task<string> GetAccessToken(string jsonWebToken)
        {
            string jsonAccessToken = string.Empty;

            try
            {
                string requestData = string.Format(_configuration.GetValue<string>("GrantType"), jsonWebToken);

                jsonAccessToken =await _googlePayAccessTokenService.GetGooglePayAccessToken(string.Empty, requestData, _headers.ContextValues.SessionId).ConfigureAwait(false);
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    throw new System.Exception(wex.Message);
                }
            }
            return jsonAccessToken;
        }

        private async Task<string> InsertFlightClass(MOBGooglePayFlightRequest request, string token)
        {
            string response = string.Empty;
            MOBGooglePayFlightClassDef classResponse = new MOBGooglePayFlightClassDef();

            try
            {
                MOBGooglePayFlightClassDef flightClassRequest = this.FlightClassRequestFromCreateRequest(request);
                string jsonRequest = JsonConvert.SerializeObject(flightClassRequest);
                //string url = _configuration.GetValue<string>("GooglePayInsertFlightClassUrl");
                string jsonResponse = await _flightClassService.InsertFlightClass(token, jsonRequest, _headers.ContextValues.SessionId, "flightClass").ConfigureAwait(false);

                if (!(string.IsNullOrEmpty(jsonResponse)))
                {
                    classResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBGooglePayFlightClassDef>(jsonResponse);

                    if (classResponse.reviewStatus == "approved")
                    {
                        response = classResponse.id;
                    }
                }
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    _logger.LogError("GooglePay_InsertFlightClass - Exception {ErrorMessageResponse} and {DeviceId}", errorResponse, request.DeviceId);

                    throw new System.Exception(wex.Message);
                }
            }

            return response;
        }

        private MOBGooglePayFlightClassDef FlightClassRequestFromCreateRequest(MOBGooglePayFlightRequest request)
        {
            MOBGooglePayFlightClassDef flightClassDef = new MOBGooglePayFlightClassDef();

            flightClassDef.id = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), Guid.NewGuid().ToString());
            flightClassDef.localScheduledDepartureDateTime = request.LocalScheduledDepartureDateTime;
            flightClassDef.flightHeader.carrier.carrierIataCode = request.CarrierCode;
            flightClassDef.flightHeader.flightNumber = request.FlightNumber;
            flightClassDef.origin.airportIataCode = request.OriginCode;
            flightClassDef.origin.gate = request.OriginGate;
            flightClassDef.destination.airportIataCode = request.DestinationCode;
            flightClassDef.localBoardingDateTime = request.LocalBoardingDateTime;
            //flightClassDef.LocalEstimatedOrActualDepartureDateTime = request.LocalEstimatedOrActualDepartureDateTime; // "2018-08-06T14:11:54"; // request.LocalScheduledDepartureDateTime.ToString("yyyy -MM-ddTHH:mm:ss"); // request.LocalEstimatedOrActualDepartureDateTime;
            flightClassDef.hexBackgroundColor = GetHexBackgroundColor(request.SeatClass.ToUpper(), request.PNRType.ToUpper());
            flightClassDef.flightHeader.carrier.airlineLogo.sourceUri.uri = _configuration.GetValue<string>("GooglePay_UnitedLogo_ImageUrl");
            flightClassDef.flightHeader.carrier.airlineAllianceLogo.sourceUri.uri = (request.PNRType.ToUpper() == "IBE" || request.PNRType.ToUpper() == "IBELITE" || request.PNRType.ToUpper() == "BE" ? _configuration.GetValue<string>("GooglePay_BlackStartAlliance_ImageUrl") : _configuration.GetValue<string>("GooglePay_WhiteStartAlliance_ImageUrl"));

            //if (!(string.IsNullOrEmpty(request.OperatingCarrierCode) && string.IsNullOrEmpty(request.OperatingCarrierName) && string.IsNullOrEmpty(request.OperatingFlightNumber)))
            if (!(string.IsNullOrEmpty(request.OperatingCarrierName)))
            {
                flightClassDef.flightHeader.operatingCarrier.carrierIataCode = string.IsNullOrEmpty(request.OperatingCarrierCode) ? request.CarrierCode : request.OperatingCarrierCode;
                flightClassDef.flightHeader.operatingCarrier.airlineName.defaultValue.value = request.OperatingCarrierName.Replace("OPERATED BY ", "");
                flightClassDef.flightHeader.operatingFlightNumber = string.IsNullOrEmpty(request.OperatingFlightNumber) ? request.FlightNumber : request.OperatingFlightNumber;
            }
            else
                flightClassDef.flightHeader.operatingCarrier = null;

            flightClassDef.localScheduledArrivalDateTime = request.LocalScheduledArrivalDateTime;
            //flightClassDef.LocalEstimatedOrActualArrivalDateTime = request.LocalEstimatedOrActualArrivalDateTime;

            return flightClassDef;
        }

        private string GetHexBackgroundColor(string seatClass, string pnrType)
        {
            if (CheckCabinName(seatClass, "GooglePay_UnitedFirstOrBusinessCabinName"))
                return _configuration.GetValue<string>("GooglePay_UnitedFirstBackgroundColor");

            if (CheckCabinName(seatClass, "GooglePay_UnitedPremiumPlusCabinName"))
                return _configuration.GetValue<string>("GooglePay_UnitedPremiumPlusBackgroundColor");

            switch (pnrType)
            {
                case "IBE":
                case "IBELITE":
                    return _configuration.GetValue<string>("GooglePay_UnitedInternationalBasicEconomyBackgroundColor");
                case "BE":
                    return _configuration.GetValue<string>("GooglePay_UnitedBasicEconomyBackgroundColor");
                default:
                    return _configuration.GetValue<string>("GooglePay_UnitedEconomyBackgroundColor");
            }
        }

        private bool CheckCabinName(string cabinName, string cabinConfigPropName)
        {
            string[] cabinConfigNames = _configuration.GetValue<string>(cabinConfigPropName).Split(',');
            return cabinConfigNames.Any(x => cabinName.Contains(x));
        }

        private async Task<string> InsertFlightObject(MOBGooglePayFlightRequest request, string token, string classResourceId)
        {
            string response = string.Empty;
            MOBGooglePayFlightObjectDef objectResponse = new MOBGooglePayFlightObjectDef();

            try
            {
                MOBGooglePayFlightObjectDef flightObjectRequest = this.FlightObjectRequestFromCreateRequest(request, classResourceId);
                string jsonRequest = JsonConvert.SerializeObject(flightObjectRequest);

                #region

                string jsonResponse = await _flightClassService.InsertFlightClass(token, jsonRequest, _headers.ContextValues.SessionId, "flightObject").ConfigureAwait(false);

                if (!(string.IsNullOrEmpty(jsonResponse)))
                {
                    objectResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBGooglePayFlightObjectDef>(jsonResponse);

                    if (objectResponse.state == "active")
                    {
                        response = objectResponse.id;
                    }
                }
                #endregion
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                    //if (traceSwitch.TraceInfo)
                    //    LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(request.DeleteKey, "GooglePay_InsertFlightObject - Exception", "ErrorMessageResponse", request.Application.Id, request.Application.Version.Major, request.DeviceId, errorResponse, true, true));
                    _logger.LogError("GooglePay_InsertFlightObject - Exception {ErrorMessageResponse} and {DeleteKey}", errorResponse, request.DeleteKey);
                    throw new System.Exception(wex.Message);
                }
            }

            return response;
        }

        private MOBGooglePayFlightObjectDef FlightObjectRequestFromCreateRequest(MOBGooglePayFlightRequest request, string classResourceId)
        {
            MOBGooglePayFlightObjectDef flightObjectDef = new MOBGooglePayFlightObjectDef();

            flightObjectDef.id = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), Guid.NewGuid().ToString());
            flightObjectDef.classId = classResourceId;
            flightObjectDef.passengerName = GetPassengerNameWithPremierStatus(request);
            flightObjectDef.boardingAndSeatingInfo.boardingGroup = string.IsNullOrEmpty(request.BoardingGroup) ? "---" : request.BoardingGroup;
            flightObjectDef.boardingAndSeatingInfo.seatNumber = string.IsNullOrEmpty(request.SeatNumber) ? "---" : request.SeatNumber;
            flightObjectDef.boardingAndSeatingInfo.sequenceNumber = string.IsNullOrEmpty(request.SequenceNumber) ? "---" : request.SequenceNumber;
            flightObjectDef.boardingAndSeatingInfo.seatClass = request.SeatClass + (!string.IsNullOrEmpty(request.FareType) ? "\n(" + request.FareType + ")" : "");
            flightObjectDef.reservationInfo.confirmationCode = request.ConfirmationCode;

            if (!(string.IsNullOrEmpty(request.FrequentFlyerNumber)))
            {
                flightObjectDef.reservationInfo.frequentFlyerInfo.frequentFlyerNumber = request.FrequentFlyerNumber;
                flightObjectDef.reservationInfo.frequentFlyerInfo.frequentFlyerProgramName.defaultValue.value = request.FrequentFlyerProgramName;
            }
            else
                flightObjectDef.reservationInfo.frequentFlyerInfo = null;

            flightObjectDef.barcode.value = request.BarCodeMessage;
            flightObjectDef.barcode.alternateText = GetAlternateText(request);

            if (!string.IsNullOrEmpty(request.EliteAccessType))
            {
                switch (request.EliteAccessType.ToUpper())
                {
                    case "PREMIER ACCESS":
                        flightObjectDef.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.uri = _configuration.GetValue<string>("GooglePay_Premier_Access_ImageUrl");
                        break;

                    case "PRIORITY BOARDING":
                        flightObjectDef.boardingAndSeatingInfo.boardingPrivilegeImage.sourceUri.uri = _configuration.GetValue<string>("GooglePay_Priority_Boarding_ImageUrl");
                        break;
                    default:
                        flightObjectDef.boardingAndSeatingInfo.boardingPrivilegeImage = null;
                        break;
                }
            }
            else
                flightObjectDef.boardingAndSeatingInfo.boardingPrivilegeImage = null;

            if (_configuration.GetValue<bool>("GooglePay_TravelReady_Toggle"))
            {
                string tsaPreCheckAndTrvelReadyImageName = GetTSAPreCheckAndTrvelReadyImageName(request);
                if (!string.IsNullOrEmpty(tsaPreCheckAndTrvelReadyImageName))
                    flightObjectDef.securityProgramLogo.sourceUri.uri = $"{_configuration.GetValue<string>("GooglePay_Base_ImageUrl")}/{tsaPreCheckAndTrvelReadyImageName}.png";
                else
                    flightObjectDef.securityProgramLogo = null;
            }
            else
            {
                if (request.SecurityProgram == true)
                {
                    flightObjectDef.securityProgramLogo.sourceUri.uri = (request.PNRType.ToUpper() == "IBE" || request.PNRType.ToUpper() == "IBELITE" ? _configuration.GetValue<string>("GooglePay_TSAPreCheck_Black_ImageUrl") : _configuration.GetValue<string>("GooglePay_TSAPreCheck_Green_ImageUrl"));
                }
                else
                {
                    flightObjectDef.securityProgramLogo = null;
                }
            }

            flightObjectDef.textModulesData = new List<Textmodulesdata>();
            if (request.Messages != null && request.Messages.Count > 0)
            {
                foreach (MOBKVP message in request.Messages)
                {
                    if (message != null || !(string.IsNullOrEmpty(message.Value)))
                    {
                        Textmodulesdata textmodulesdata3 = new Textmodulesdata();
                        textmodulesdata3.header = string.IsNullOrEmpty(message.Key) ? string.Empty : message.Key;
                        textmodulesdata3.body = message.Value;
                        flightObjectDef.textModulesData.Add(textmodulesdata3);
                    }
                }
            }

            Textmodulesdata textmodulesdata1 = new Textmodulesdata();
            textmodulesdata1.header = "ADDRESS";
            textmodulesdata1.body = "United Airlines, Inc.\nPO Box 66100\nChicago, IL 60666";
            Textmodulesdata textmodulesdata2 = new Textmodulesdata();
            textmodulesdata2.header = "TERMS";
            textmodulesdata2.body = _configuration.GetValue<string>("GooglePay_United_Terms");

            flightObjectDef.textModulesData.Add(textmodulesdata1);
            flightObjectDef.textModulesData.Add(textmodulesdata2);

            return flightObjectDef;
        }

        private string GetTSAPreCheckAndTrvelReadyImageName(MOBGooglePayFlightRequest request)
        {
            string tsaPreCheckImageName = GetTSAPreCheckImageName(request);
            string travelReadyOrVerifiedImageName = GetTravelReadyOrVerifiedImageName(request);

            if (!string.IsNullOrEmpty(tsaPreCheckImageName) && !string.IsNullOrEmpty(travelReadyOrVerifiedImageName))
                return $"{tsaPreCheckImageName}_{travelReadyOrVerifiedImageName}";
            if (!string.IsNullOrEmpty(tsaPreCheckImageName))
                return $"{tsaPreCheckImageName}";
            if (!string.IsNullOrEmpty(travelReadyOrVerifiedImageName))
                return $"{travelReadyOrVerifiedImageName}";
            return string.Empty;
        }

        private string GetTravelReadyOrVerifiedImageName(MOBGooglePayFlightRequest request)
        {
            if (request.IsTravelReady)
                return "travel_ready";
            if (request.IsVerifiedByUnited)
                return "verified";
            return string.Empty;
        }

        private string GetTSAPreCheckImageName(MOBGooglePayFlightRequest request)
        {
            if (request.SecurityProgram)
            {
                if (String.Equals("IBE", request.PNRType, StringComparison.OrdinalIgnoreCase)
                    || String.Equals("PBE", request.PNRType, StringComparison.OrdinalIgnoreCase)
                    || String.Equals("IBELITE", request.PNRType, StringComparison.OrdinalIgnoreCase))
                {
                    return "tsaprecheck_black_1";
                }
                return "tsaprecheck_green_1";
            }
            return string.Empty;
        }

        private string GetPassengerNameWithPremierStatus(MOBGooglePayFlightRequest request)
        {
            //Revenue: GLOBAL SERVICES / UA *G
            //NRPS: GLOBAL SERVICES
            string mpStatusText = GetPremierLevel(request);

            if (string.IsNullOrWhiteSpace(request.CarrierCode) || string.IsNullOrWhiteSpace(mpStatusText))
                return request.PassengerName;

            string[] statusArr = mpStatusText.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.Equals("UA", request.CarrierCode, StringComparison.OrdinalIgnoreCase) && statusArr.Length >= 1)
            {
                return statusArr[0].Trim() + "\n" + request.PassengerName;
            }

            string statusText = statusArr.Length > 1 ? statusArr[1] : statusArr[0];
            if (statusText.Trim().EndsWith("*G"))
                statusText = "*G";
            else if (statusText.Trim().EndsWith("*S"))
                statusText = "*S";

            return statusText + "\n" + request.PassengerName;
        }

        private string GetPremierLevel(MOBGooglePayFlightRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.ZoneBoardingText))
                return request.ZoneBoardingText.Trim();

            if (string.IsNullOrWhiteSpace(request.FrequentFlyerNumber))
                return string.Empty;

            if (Regex.IsMatch(request.FrequentFlyerNumber, REGEX_MP_PREMIER_LVL))
            {
                Match premierLevelMatch = Regex.Match(request.FrequentFlyerNumber, REGEX_MP_PREMIER_LVL);
                string premierLevel = premierLevelMatch.Groups[2].Value;
                if (premierLevel.StartsWith("/ "))
                    premierLevel = premierLevel.Replace("/ ", string.Empty);
                return premierLevel;
            }
            return string.Empty;
        }

        #endregion

        private string GetAlternateText(MOBGooglePayFlightRequest request)
        {
            if (request.IsDigitalIdEligible)
                return GetConfigEntriesWithDefault("GooglePay_Digital_Id_Text", string.Empty);
            if (request.IsBagDropShortCutEligible)
                return GetConfigEntriesWithDefault("GooglePay_BagDrop_Shortcut_Text", string.Empty);
            return string.Empty;
        }

        private string GetConfigEntriesWithDefault(string configKey, string defaultReturnValue)
        {
            var configString = _configuration.GetValue<string>(configKey);
            if (!string.IsNullOrEmpty(configString))
            {
                return configString;
            }

            return defaultReturnValue;
        }

        #region UpdateFlightFromRequest
        public async Task<MOBGooglePayFlightResponse> UpdateFlightFromRequest(MOBGooglePayFlightRequest request)
        {
            MOBGooglePayFlightResponse response = new MOBGooglePayFlightResponse();
            string jsonWebToken = string.Empty;
            string jsonWebTokenWithPayload = string.Empty;
            string classResourceId = string.Empty;
            string objectResourceId = string.Empty;

            string GooglePayCertPath = string.Format("{0}\\{1}", _currentEnvironment.ContentRootPath, _configuration.GetValue<string>("GooglePayCertPath"));
            GooglePayCertPath = GooglePayCertPath.Replace("\\", "/");
            jsonWebToken = GenerateJsonWebToken(GooglePayCertPath);
            string jsonAccessToken =await GetAccessToken(jsonWebToken);
            AccessTokenClass accessTokenObj = Newtonsoft.Json.JsonConvert.DeserializeObject<AccessTokenClass>(jsonAccessToken);
            string token = accessTokenObj.token_type + " " + accessTokenObj.access_token;

            if (!string.IsNullOrEmpty(request.FlightClassId))
                classResourceId = await UpdateFlightClassFromRequest(request, token);
            if (!string.IsNullOrEmpty(request.FlightObjectId))
                objectResourceId = await UpdateFlightObjectFromRequest(request, token);

            response.ClassResourceId = classResourceId.Split('.')[1];
            response.ObjectResourceId = objectResourceId.Split('.')[1];
            response.TransactionId = request.TransactionId;

            return await Task.FromResult(response);
        }

        private async Task<string> UpdateFlightClassFromRequest(MOBGooglePayFlightRequest request, string token)
        {
            string response = string.Empty;
            MOBGooglePayFlightClassDef classResponse = new MOBGooglePayFlightClassDef();
            string flightClassId = string.Empty;

            try
            {
                flightClassId = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), request.FlightClassId);
                MOBGooglePayFlightClassDef flightClassRequest = this.FlightClassRequestFromCreateRequest(request);
                flightClassRequest.id = flightClassId;
                string jsonRequest = JsonConvert.SerializeObject(flightClassRequest);

                string Path = string.Format("flightClass/{0}", flightClassId);

                //TODO
                // HttpWebResponse httpesponse = HttpHelper.Put(url, "application/json; charset=utf-8", token, jsonRequest);

                string jsonResponse =await _flightClassService.UpdateFlightClass(token, jsonRequest, _headers.ContextValues.SessionId, Path).ConfigureAwait(false); ;// HttpHelper.GetResponseBodyAsJsonString(httpesponse);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    classResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBGooglePayFlightClassDef>(jsonResponse);

                    if (classResponse.reviewStatus == "approved")
                    {
                        response = classResponse.id;
                    }
                }
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                    _logger.LogError("GooglePay_UpdateFlightClassFromRequest - Exception {ErrorMessageResponse} and {ConfirmationCode+FlightClassId}", errorResponse, request.ConfirmationCode + "~" + request.FlightClassId);

                    throw new System.Exception(wex.Message);
                }
            }

            return response;
        }

        private async Task<string> UpdateFlightObjectFromRequest(MOBGooglePayFlightRequest request, string token)
        {
            string response = string.Empty;
            MOBGooglePayFlightObjectDef objectResponse = new MOBGooglePayFlightObjectDef();
            string flightClassId = string.Empty;
            string flightObjectId = string.Empty;
            try
            {
                flightClassId = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), request.FlightClassId);
                flightObjectId = string.Format("{0}.{1}", _configuration.GetValue<string>("IssuerIdentity"), request.FlightObjectId);
                MOBGooglePayFlightObjectDef flightObjectRequest = this.FlightObjectRequestFromCreateRequest(request, flightClassId);
                flightObjectRequest.id = flightObjectId;
                string jsonRequest = JsonConvert.SerializeObject(flightObjectRequest);

                string Path = string.Format("flightObject/{0}", flightObjectId);

                string jsonResponse =await  _flightClassService.UpdateFlightClass(token, jsonRequest, _headers.ContextValues.SessionId, Path).ConfigureAwait(false);
                if (!(string.IsNullOrEmpty(jsonResponse)))
                {
                    objectResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBGooglePayFlightObjectDef>(jsonResponse);

                    if (objectResponse.state == "active")
                    {
                        response = objectResponse.id;
                    }
                }
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();
                    _logger.LogError("GooglePay_UpdateFlightClassFromRequest - Exception {ErrorMessageResponse} and {ConfirmationCode+FlightClassId}", errorResponse, request.ConfirmationCode + "~" + request.FlightClassId);

                    throw new System.Exception(wex.Message);
                }
            }
            return response;
        }

        #endregion
        private string GenerateJsonWebToken(string GooglePayCertPath)
        {
            string tokenStr = string.Empty;
            try
            {
                var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var issueTime = DateTime.UtcNow;

                var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
                var exp = (int)issueTime.AddMinutes(55).Subtract(utc0).TotalSeconds;


                System.Security.Claims.Claim claim = new System.Security.Claims.Claim("scope", _configuration.GetValue<string>("WalletObjectIssuerUrl"));
                System.Security.Claims.Claim claim2 = new System.Security.Claims.Claim("iss", _configuration.GetValue<string>("IssuerServiceAccountName"));
                System.Security.Claims.Claim claim3 = new System.Security.Claims.Claim("iat", iat.ToString());
                System.Security.Claims.Claim claim4 = new System.Security.Claims.Claim("exp", exp.ToString());
                System.Security.Claims.Claim claim5 = new System.Security.Claims.Claim("exp", "d94f75acd20c7ff86fb533c6ea281d5df76b1bc7");

                System.Collections.Generic.IEnumerable<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>{
               claim,claim2,claim3,claim4, claim5
           };

                //In Production uses the encrypted Password
                X509Certificate2 certificate;
                string isEncrypted = _configuration.GetValue<string>("IsPCIEncryptionEnabledinProd");
                if (!string.IsNullOrEmpty(isEncrypted) && Convert.ToBoolean(isEncrypted) == true)
                {
                    NameValueCollection section = (NameValueCollection)_configuration.GetSection("SecureAppSettings");
                    string loginPwd = section["GoogleCertificateSecureLogin"];
                    certificate = new X509Certificate2(GooglePayCertPath, loginPwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                }
                else
                {
                    certificate = new X509Certificate2(GooglePayCertPath, "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                }
                var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("IssuerServiceAccountName"),
                    audience: _configuration.GetValue<string>("GooglePayAuthTokenUrl"),
                    claims: claims,
                    //comment the below line to generate a 'none' alg
                    signingCredentials: new X509SigningCredentials(certificate),
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddHours(1)
                    );

                tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tokenStr;
        }

        private string GenerateJsonWebTokenWithPayload(string payload, string GooglePayCertPath)
        {
            var utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var issueTime = DateTime.UtcNow;
            var iat = (int)issueTime.Subtract(utc0).TotalSeconds;
            var exp = (int)issueTime.AddMinutes(55).Subtract(utc0).TotalSeconds;

            System.Security.Claims.Claim claim = new System.Security.Claims.Claim("payload", payload, JsonClaimValueTypes.Json);
            //System.Security.Claims.Claim claim2 = new System.Security.Claims.Claim("origins", "[\"http://localhost:8080\", \"http://www.united.com\"]", JsonClaimValueTypes.JsonArray);
            System.Security.Claims.Claim claim1 = new System.Security.Claims.Claim("typ", "savetoandroidpay");
            System.Collections.Generic.IEnumerable<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>{
                claim,claim1
            };
            X509Certificate2 certificate;
            string isEncrypted = _configuration.GetValue<string>("IsPCIEncryptionEnabledinProd");

            if (!string.IsNullOrEmpty(isEncrypted) && Convert.ToBoolean(isEncrypted) == true)
            {
                NameValueCollection section = (NameValueCollection)_configuration.GetValue<NameValueCollection>("SecureAppSettings");
                string loginPwd = section["GoogleCertificateSecureLogin"];
                certificate = new X509Certificate2(GooglePayCertPath, loginPwd, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            }
            else
            {
                certificate = new X509Certificate2(GooglePayCertPath, "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            }

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("IssuerServiceAccountName"),
                audience: "google",
                claims: claims,
                signingCredentials: new X509SigningCredentials(certificate),
                notBefore: DateTime.UtcNow
                );

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenStr;
        }

        private void InsertGooglePayPasses(string confirmationNumber, string classId, string objectId, string filterKey, string deleteKey, int applicationId, string appVersion, string deviceId, string payload)
        {
            try
            {
                var googlepaypassesData = new GooglePayPassesData()
                {
                    ConfirmationNumber = confirmationNumber,
                    ClassId = classId,
                    ObjectId = objectId,
                    FilterKey = filterKey,
                    DeleteKey = deleteKey,
                    ApplicationId = applicationId,
                    AppVersion = appVersion,
                    DeviceId = deviceId,
                    Payload = payload
                };
                var googlepayDynamoDB = new GooglePayDynamoDB(_configuration, _dynamoDBService);
                googlepayDynamoDB.InsertGooglePayPasses(googlepaypassesData, "Key", _headers.ContextValues.SessionId);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
