using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Common.HelperSeatEngine;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.SeatEngine;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Pcu;
using United.Mobile.ViewResSeatMap.Domain;
using United.Service.Presentation.SecurityResponseModel;
using United.Services.FlightShopping.Common.FlightReservation;
using Xunit;
using MOBBKTrip = United.Mobile.Model.Shopping.Booking.MOBBKTrip;
using PKDispenserResponse = United.Service.Presentation.SecurityResponseModel.PKDispenserResponse;
using Constants = United.Mobile.Model.Constants;
using United.Utility.Helper;
using United.Mobile.Model.Common;

namespace United.Mobile.Test.ViewResSeatMap.Tests
{
    public class ViewResSeatMapBusinessTest
    {
        private readonly Mock<ICacheLog<ViewResSeatMapBusiness>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<ISeatMapAvailabilityService> _seatMapAvailabilityService;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
       // private readonly Mock<ISeatEnginePostService> _seatEnginePostService;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<IShoppingCartService> _shoppingCartService;
        private readonly Mock<IPaymentService> _paymentService;
        private readonly Mock<ISeatEngine> _seatEngine;
        private readonly Mock<ISeatMapCSL30> _seatMapCSL30;
        private readonly ViewResSeatMapBusiness viewResSeatMapBusiness;
        private readonly Mock<IPKDispenserService> _pKDispenserService;
        private readonly Mock<ICachingService> _cachingService;
        private readonly Mock<PKDispenserPublicKey> _pKDispenserPublicKey;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IProductOffers> _productOffer;
        private readonly Mock<IRegisterCFOP> _registerCFOP;
        private readonly Mock<IProductInfoHelper> _productInfoHelper;
        private readonly Mock<IPaymentUtility> _paymentUtility;
        private readonly Mock<IManageReservation> _manageReservation;
        private readonly Mock<IFormsOfPayment> _formsOfPayment;
        private readonly Mock<IFeatureSettings> _featureSettings;

        public ViewResSeatMapBusinessTest()
        {
            _logger = new Mock<ICacheLog<ViewResSeatMapBusiness>>();
            _dPService = new Mock<IDPService>();
            _seatMapAvailabilityService = new Mock<ISeatMapAvailabilityService>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            //_seatEnginePostService = new Mock<ISeatEnginePostService>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _shoppingCartService = new Mock<IShoppingCartService>();
            _paymentService = new Mock<IPaymentService>();
            _seatEngine = new Mock<ISeatEngine>();
            _seatMapCSL30 = new Mock<ISeatMapCSL30>();
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
               .Build();
            _headers = new Mock<IHeaders>();
            _pKDispenserService = new Mock<IPKDispenserService>();
            _cachingService = new Mock<ICachingService>();
            _shoppingCartService = new Mock<IShoppingCartService>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _productOffer = new Mock<IProductOffers>();
            _registerCFOP = new Mock<IRegisterCFOP>();
            _productInfoHelper = new Mock<IProductInfoHelper>();
            _paymentUtility = new Mock<IPaymentUtility>();
            _manageReservation = new Mock<IManageReservation>();
            _formsOfPayment = new Mock<IFormsOfPayment>();
            _featureSettings = new Mock<IFeatureSettings>();
            _pKDispenserPublicKey = new Mock<PKDispenserPublicKey>(_configuration, _cachingService.Object, _dPService.Object, _pKDispenserService.Object);

            viewResSeatMapBusiness = new ViewResSeatMapBusiness(_logger.Object, _configuration,_headers.Object, _sessionHelperService.Object, _shoppingSessionHelper.Object,
                _seatEngine.Object, _seatMapCSL30.Object, _shoppingUtility.Object,_manageReservation.Object, _shoppingCartService.Object, _featureSettings.Object);
            SetupHttpContextAccessor();
            
            SetHeaders();
        }

        private void SetupHttpContextAccessor()
        {
            var guid = Guid.NewGuid().ToString();
            var context = new DefaultHttpContext();
            context.Request.Headers[Constants.HeaderAppIdText] = "1";
            context.Request.Headers[Constants.HeaderAppMajorText] = "1";
            context.Request.Headers[Constants.HeaderAppMinorText] = "0";
            context.Request.Headers[Constants.HeaderDeviceIdText] = guid;
            context.Request.Headers[Constants.HeaderLangCodeText] = "en-us";
            context.Request.Headers[Constants.HeaderRequestTimeUtcText] = DateTime.UtcNow.ToString();
            context.Request.Headers[Constants.HeaderTransactionIdText] = guid;
        }

        private void SetHeaders(string deviceId = "D873298F-F27D-4AEC-BE6C-DE79C4259626"
                , string applicationId = "1"
                , string appVersion = "4.1.26"
                , string transactionId = "3f575588-bb12-41fe-8be7-f57c55fe7762|afc1db10-5c39-4ef4-9d35-df137d56a23e"
                , string languageCode = "en-US"
                , string sessionId = "D58E298C35274F6F873A133386A42916")
        {
            _headers.Setup(_ => _.ContextValues).Returns(
           new HttpContextValues
           {
               Application = new Application()
               {
                   Id = Convert.ToInt32(applicationId),
                   Version = new Mobile.Model.Version
                   {
                       Major = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(0, 1)),
                       Minor = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(2, 1)),
                       Build = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(4, 2))
                   }
               },
               DeviceId = deviceId,
               LangCode = languageCode,
               TransactionId = transactionId,
               SessionId = sessionId
           });
        }

        private string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName
                                                : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSelectSeats), MemberType = typeof(TestDataGenerator))]
        public void SelectSeats_Test(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, SeatChangeState seatChange)
        {

            if (accessCode == "UAWS-MOBILE-ACCESSCODE")
            {
                _configuration["EnableBacklogIssueFixes"] = "false";
            }

            var seatResponse = JsonConvert.DeserializeObject<List<MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap.json"));
            var MOBSeatChangeInitialize = GetFileContent("MOBSeatChangeInitializeResponse.json");

            var MOBSeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(MOBSeatChangeInitialize);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChange);

            _sessionHelperService.Setup(p => p.GetSession<MOBSeatChangeInitializeResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(MOBSeatChangeInitializeResponse);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            if (applicationId != 1 && applicationId != 3)

                _sessionHelperService.Setup(p => p.GetSession<List<MOBSeatMap>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatResponse);

            if (sessionId == "100")
                _configuration["HandleNullExceptionDueToSeatEngineWSError"] = "false";

            _shoppingUtility.Setup(p => p.EnableOAMsgUpdateFixViewRes(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingUtility.Setup(p => p.IsOperatedByOtherAirlines(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = viewResSeatMapBusiness.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            if (result.Exception == null)
                Assert.True(result.Result != null  && result.Result.SessionId != null);
            else
                Assert.True(result.Exception.InnerExceptions != null && result.Exception.Message != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSelectSeats1), MemberType = typeof(TestDataGenerator))]
        public void SelectSeats_Test1(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, SeatChangeState seatChange)
        {

            if (accessCode == "UAWS-MOBILE-ACCESSCODE")
            {
                _configuration["EnableBacklogIssueFixes"] = "false";
            }

            var seatResponse = JsonConvert.DeserializeObject<List<MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap.json"));
            var MOBSeatChangeInitialize = GetFileContent("MOBSeatChangeInitializeResponse.json");

            var MOBSeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(MOBSeatChangeInitialize);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChange);

            _sessionHelperService.Setup(p => p.GetSession<MOBSeatChangeInitializeResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(MOBSeatChangeInitializeResponse);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            _shoppingUtility.Setup(p => p.EnableOAMsgUpdateFixViewRes(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingUtility.Setup(p => p.IsOperatedByOtherAirlines(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            if (applicationId != 1 && applicationId != 3)

                _sessionHelperService.Setup(p => p.GetSession<List<MOBSeatMap>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatResponse);

            if (sessionId == "100")
                _configuration["HandleNullExceptionDueToSeatEngineWSError"] = "false";

            var result = viewResSeatMapBusiness.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            if (result?.Exception == null)
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSelectSeats1), MemberType = typeof(TestDataGenerator))]
        public void SelectSeats_Test2(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, SeatChangeState seatChange)
        {
         
            var seatResponse = JsonConvert.DeserializeObject<List<MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap.json"));
            var MOBSeatChangeInitialize = GetFileContent("MOBSeatChangeInitializeResponse.json");

            var MOBSeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(MOBSeatChangeInitialize);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChange);

            _sessionHelperService.Setup(p => p.GetSession<MOBSeatChangeInitializeResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(MOBSeatChangeInitializeResponse);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            _shoppingUtility.Setup(p => p.EnableOAMsgUpdateFixViewRes(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            _shoppingUtility.Setup(p => p.IsOperatedByOtherAirlines(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _seatMapCSL30.Setup(p => p.GetCSL30SeatMapForRecordLocatorWithLastName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<MOBBKTraveler>>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<MOBTravelerSignInData>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),null, It.IsAny<string>())).ReturnsAsync(seatMap);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);
            

            var result = viewResSeatMapBusiness.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            if (result?.Exception == null)
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSelectSeats1), MemberType = typeof(TestDataGenerator))]
        public void SelectSeats_Test3(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, SeatChangeState seatChange)
        {

            var seatResponse = JsonConvert.DeserializeObject<List<MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap.json"));
            var MOBSeatChangeInitialize = GetFileContent("MOBSeatChangeInitializeResponse.json");

            var MOBSeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(MOBSeatChangeInitialize);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChange);

            _sessionHelperService.Setup(p => p.GetSession<MOBSeatChangeInitializeResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(MOBSeatChangeInitializeResponse);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            _shoppingUtility.Setup(p => p.EnableOAMsgUpdateFixViewRes(It.IsAny<int>(), It.IsAny<string>())).Returns(false);

            _shoppingUtility.Setup(p => p.IsOperatedByOtherAirlines(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _seatMapCSL30.Setup(p => p.GetCSL30SeatMapForRecordLocatorWithLastName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<MOBBKTraveler>>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),  It.IsAny<int>(), It.IsAny<MOBTravelerSignInData>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null, It.IsAny<string>())).ReturnsAsync(seatMap);

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);


            var result = viewResSeatMapBusiness.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            if (result?.Exception == null)
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSeatChangeInitialize), MemberType = typeof(TestDataGenerator))]
        public void SeatChangeInitialize_Test(MOBSeatChangeInitializeRequest request, Model.MPRewards.SeatEngine seatEngine)
        {

            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(GetFileContent("SessionData.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap.json"));
            var EligibilityResponse = JsonConvert.DeserializeObject<EligibilityResponse>(GetFileContent("EligibilityResponse.json"));
            var SeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(GetFileContent("MOBSeatChangeInitializeResponse.json"));
            if (request.LastName == "Error")

                _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(EligibilityResponse);

            _seatEngine.Setup(p => p.GetFlightReservationCSL_CFOP(It.IsAny<MOBSeatChangeInitializeRequest>(), It.IsAny<Model.MPRewards.SeatEngine>(), It.IsAny<bool>())).ReturnsAsync(seatEngine);
            _configuration["EnableSocialDistanceMessagingForSeatMap"] = "True";

            if (request.SessionId == "1")
                _configuration["isEnablePreferredZoneSubscriptionMessagesManageRes"] = "True";

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            if (request.SessionId == "201")

                _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(EligibilityResponse);

            if (request.LastName == "lastName")
            {
                _configuration["IsSeatNumberClickableEnabled"] = "True";
            }

            _seatEngine.Setup(p => p.GetNoFreeSeatCompanionCount(It.IsAny<List<Model.Shopping.Booking.MOBBKTraveler>>(), It.IsAny<List<MOBBKTrip>>())).Returns(10);

            _seatEngine.Setup(p => p.PopulateEPlusSubscriberSeatMessage(It.IsAny<MOBSeatChangeInitializeResponse>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((10, SeatChangeInitializeResponse, 10)));

            _seatEngine.Setup(p => p.PopulateEPlusSubscriberAndMPMemeberSeatMessage(It.IsAny<MOBSeatChangeInitializeResponse>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((10, SeatChangeInitializeResponse, 0, false,false,false)));

            _seatEngine.Setup(p => p.GetOperatedByText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("OperatedByText");

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);



            var result = viewResSeatMapBusiness.SeatChangeInitialize(request);

            if (result.Exception == null)
                Assert.True(result.Result != null && result.Result.SessionId != null);
            else
                Assert.True(result.Exception.InnerExceptions != null && result.Exception.Message != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.InputSeatChangeInitialize), MemberType = typeof(TestDataGenerator))]
        public void SeatChangeInitialize_Test1(MOBSeatChangeInitializeRequest request, Model.MPRewards.SeatEngine seatEngine)
        {

            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(GetFileContent("SessionData.json"));
            var seatMap = JsonConvert.DeserializeObject<List<Model.Shopping.MOBSeatMap>>(GetFileContent("MOBSeatMap1.json"));
            var EligibilityResponse = JsonConvert.DeserializeObject<EligibilityResponse>(GetFileContent("EligibilityResponse.json"));
            var SeatChangeInitializeResponse = JsonConvert.DeserializeObject<MOBSeatChangeInitializeResponse>(GetFileContent("MOBSeatChangeInitializeResponse.json"));
            if (request.LastName == "Error")

                _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(EligibilityResponse);

            _seatEngine.Setup(p => p.GetFlightReservationCSL_CFOP(It.IsAny<MOBSeatChangeInitializeRequest>(), It.IsAny<Model.MPRewards.SeatEngine>(), It.IsAny<bool>())).ReturnsAsync(seatEngine);
            _configuration["EnableSocialDistanceMessagingForSeatMap"] = "True";

            if (request.SessionId == "1")
                _configuration["isEnablePreferredZoneSubscriptionMessagesManageRes"] = "True";

            //_seatEngine.Setup(p => p.GetSeatMapForRecordLocatorWithLastNameCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(seatMap);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            if (request.SessionId == "201")

                _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(EligibilityResponse);

            if (request.LastName == "lastName")
            {
                _configuration["IsSeatNumberClickableEnabled"] = "True";
            }

            _seatEngine.Setup(p => p.GetNoFreeSeatCompanionCount(It.IsAny<List<Model.Shopping.Booking.MOBBKTraveler>>(), It.IsAny<List<MOBBKTrip>>())).Returns(10);

            _seatEngine.Setup(p => p.PopulateEPlusSubscriberSeatMessage(It.IsAny<MOBSeatChangeInitializeResponse>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((10, SeatChangeInitializeResponse, 10)));

            _seatEngine.Setup(p => p.PopulateEPlusSubscriberAndMPMemeberSeatMessage(It.IsAny<MOBSeatChangeInitializeResponse>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult((10, SeatChangeInitializeResponse, 0, false, false, false)));

            _seatEngine.Setup(p => p.GetOperatedByText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns("OperatedByText");

            _shoppingUtility.Setup(p => p.EnableOAMsgUpdateFixViewRes(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingUtility.Setup(p => p.EnableOAMessageUpdate(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _seatMapCSL30.Setup(p => p.GetCSL30SeatMapForRecordLocatorWithLastName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<List<TripSegment>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<MOBBKTraveler>>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<MOBTravelerSignInData>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null, It.IsAny<string>())).ReturnsAsync(seatMap);

            _seatEngine.Setup(p => p.ValidateResponse(It.IsAny<MOBSeatChangeInitializeResponse>())).Returns(false);

            _seatEngine.Setup(p => p.GetSeatMapWithPreAssignedSeats(It.IsAny<List<MOBSeatMap>>(), It.IsAny<List<Model.Shopping.Misc.Seat>>(), It.IsAny<bool>())).Returns(seatMap);

            _seatEngine.Setup(p => p.HasEconomySegment(It.IsAny<List<MOBBKTrip>>())).Returns(true);


            var result = viewResSeatMapBusiness.SeatChangeInitialize(request);

            if (result.Exception == null)
                Assert.True(result.Result != null && result.Result.SessionId != null);
            else
                Assert.True(result.Exception.InnerExceptions != null && result.Exception.Message != null);
        }
    }

}
