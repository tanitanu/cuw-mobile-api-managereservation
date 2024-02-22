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
using United.Common.Helper.Profile;
using United.Common.Helper.Shopping;
using United.Common.HelperSeatEngine;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Profile;
using United.Mobile.DataAccess.SeatEngine;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Pcu;
using United.Mobile.SeatMap.Domain;
using United.Service.Presentation.SecurityResponseModel;
using United.Services.FlightShopping.Common.FlightReservation;
using Xunit;
using Constants = United.Mobile.Model.Constants;
using PKDispenserResponse = United.Service.Presentation.SecurityResponseModel.PKDispenserResponse;
using Reservation = United.Mobile.Model.Shopping.Reservation;

namespace United.Mobile.Test.SeatMap.Tests
{
    public class SeatMapBusinessTest
    {
        private readonly Mock<ILogger<SeatMapBusiness>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<ISeatMapAvailabilityService> _seatMapAvailabilityService;
        private readonly Mock<IProductInfoHelper> _productInfoHelper;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly Mock<ISeatEnginePostService> _seatEnginePostService;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<IShoppingCartService> _shoppingCartService;
        private readonly Mock<IPaymentService> _paymentService;
        private readonly Mock<IManageReservation> _manageReservation;
        private readonly Mock<ISeatEngine> _seatEngine;
        private readonly Mock<IFormsOfPayment> _formsOfPayment;
        private readonly Mock<ISeatMapCSL30> _seatMapCSL30;
        private readonly Mock<IRegisterCFOP> _registerCFOP;
        private readonly Mock<IProductOffers> _shopping;
        private readonly SeatMapBusiness seatMapBusiness;
        private readonly Mock<IPKDispenserService> _pKDispenserService;
        private readonly Mock<ICachingService> _cachingService;
        private readonly Mock<PKDispenserPublicKey> _pKDispenserPublicKey;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<IPaymentUtility> _paymentUtility;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IReferencedataService> _referencedataService;
        private readonly Mock<IDataVaultService> _dataVaultService;
        private readonly Mock<IProfileCreditCard> _profileCreditCard;
        private readonly Mock<ILegalDocumentsForTitlesService> _legalDocumentsForTitlesService;

        public SeatMapBusinessTest()
        {
            _logger = new Mock<ILogger<SeatMapBusiness>>();
            _dPService = new Mock<IDPService>();
            _seatMapAvailabilityService = new Mock<ISeatMapAvailabilityService>();
            _productInfoHelper = new Mock<IProductInfoHelper>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _seatEnginePostService = new Mock<ISeatEnginePostService>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _shoppingCartService = new Mock<IShoppingCartService>();
            _paymentService = new Mock<IPaymentService>();
            _manageReservation = new Mock<IManageReservation>();
            _seatEngine = new Mock<ISeatEngine>();
            _formsOfPayment = new Mock<IFormsOfPayment>();
            _seatMapCSL30 = new Mock<ISeatMapCSL30>();
            _registerCFOP = new Mock<IRegisterCFOP>();
            _shopping = new Mock<IProductOffers>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _paymentUtility = new Mock<IPaymentUtility>();
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
               .Build();
            _pKDispenserService = new Mock<IPKDispenserService>();
            _cachingService = new Mock<ICachingService>();
            _pKDispenserPublicKey = new Mock<PKDispenserPublicKey>(_configuration, _cachingService.Object, _dPService.Object, _pKDispenserService.Object);
            _referencedataService = new Mock<IReferencedataService>();
            _dataVaultService = new Mock<IDataVaultService>();
            _profileCreditCard = new Mock<IProfileCreditCard>();
            _legalDocumentsForTitlesService = new Mock<ILegalDocumentsForTitlesService>();
            _headers = new Mock<IHeaders>();
            seatMapBusiness = new SeatMapBusiness(
                _logger.Object
            ,  _configuration
            ,  _cachingService.Object
            ,  _dPService.Object
            ,  _sessionHelperService.Object
            ,  _seatMapAvailabilityService.Object
            ,  _productInfoHelper.Object
            ,  _seatEnginePostService.Object
            ,  _shoppingCartService.Object
            ,  _manageReservation.Object
            ,  _seatMapCSL30.Object
            ,  _formsOfPayment.Object
            ,  _registerCFOP.Object
            ,  _shopping.Object
            ,  _pKDispenserService.Object
            ,  _shoppingSessionHelper.Object
            ,  _seatEngine.Object
            ,  _paymentUtility.Object
            ,  _dynamoDBService.Object
            ,  _paymentService.Object
            , _headers.Object
                    );
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
        [MemberData(nameof(TestDataGenerator.GetSeatMap), MemberType = typeof(TestDataGenerator))]
        public void GetSeatMap_Tests(string accessCode, int applicationId, Definition.SeatCSL30.SeatMap seatMapResponse)
        {
            string appVersion = "4.1.25";
            string transactionId = "383084f2-67ec-4ecc-8bfb-8fb402c5610b|3edc351a-0d7e-4c85-bf9e-8f260b666c70";
            string languageCode = "en-US";
            string carrierCode = "UA";
            string flightNumber = "802";
            string flightDate = "20211117";
            string departureAirportCode = "DEL";
            string arrivalAirportCode = "EWR";

            if (accessCode == "ACCESSCODE")
            {
                _configuration["EnableCSL30FlightSearchSeatMap"] = "false";
                _configuration["CarrierCodeTOM"] = "UA";
            }

            var flightStatus = GetFileContent("FlightStatusResponse.json");
            var flightStatusRespones = JsonConvert.DeserializeObject<List<ComplimentaryUpgradeCabin>>(flightStatus);
            _dPService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), _configuration)).ReturnsAsync("Bearer Sample Token");

            if (accessCode == "CODE")
            {
                _seatMapAvailabilityService.Setup(p => p.GetCSL30SeatMap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");
            }
            else
            {
                _seatMapAvailabilityService.Setup(p => p.GetCSL30SeatMap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(JsonConvert.SerializeObject(seatMapResponse));
                _configuration["EnableProjectTOM"] = "True";
            }
            _dynamoDBService.Setup(p => p.GetRecords<List<ComplimentaryUpgradeCabin>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(flightStatusRespones);
            var result = seatMapBusiness.GetSeatMap(applicationId, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RegisterSeats), MemberType = typeof(TestDataGenerator))]
        public void RegisterSeats_Tests(MOBRegisterSeatsRequest registerSeatsRequest, SeatChangeState seatChangeState, CheckOutResponse checkOutResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var seatMapCSL30Response = JsonConvert.DeserializeObject<List<Model.ShopSeats.SeatMapCSL30>>(GetFileContent("SeatMapCSL30Response.json"));
            var persistedCSL30SeatMaps = JsonConvert.DeserializeObject<List<Model.ShopSeats.SeatMapCSL30>>(GetFileContent("persistedCSL30SeatMaps.json"));
            var seatResponse = JsonConvert.DeserializeObject<List<Model.Common.MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var reservation = JsonConvert.DeserializeObject<Service.Presentation.ReservationResponseModel.ReservationDetail>(GetFileContent("ReservationDetailResponse.json"));
            var pcu = JsonConvert.DeserializeObject<PcuState>(GetFileContent("PcuStateResponse.json"));
            var offers = JsonConvert.DeserializeObject<GetOffers>(GetFileContent("OffersResponse.json"));
            var flightReservation = JsonConvert.DeserializeObject<FlightReservationResponse>(GetFileContent("FlightReservationResponse.json"));
            var shoppingResponse = JsonConvert.DeserializeObject<MOBShoppingCart>(GetFileContent("ShoppingCartResponse.json"));
            var prodResponse = JsonConvert.DeserializeObject<List<ProdDetail>>(GetFileContent("ProdDetailResponse.json"));
            var reservationResponse = JsonConvert.DeserializeObject<Reservation>(GetFileContent("Reservation.json"));
            var pKDispenser = GetFileContent("PKDispenserKey.json");
            var pKDispenserResponse = JsonConvert.DeserializeObject<PKDispenserResponse>(GetFileContent("PKDispenserResponse.json"));
            var formResponse = JsonConvert.DeserializeObject<List<FormofPaymentOption>>(GetFileContent("EligibleFormOfPayment.json"));
            var FlightSeatMapDetail = GetFileContent("FlightSeatMapDetail.json");

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<PcuState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(pcu);
            _sessionHelperService.Setup(p => p.GetSession<GetOffers>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(offers);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChangeState);

            if (registerSeatsRequest.SessionId != "1")
            {
                _configuration["FixSeatNotFoundManageResObjRefExceptionInRegisterSeatsAction"] = "false";
            }

            _sessionHelperService.Setup(p => p.GetSession<List<Model.Common.MOBSeatMap>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatResponse);
            _sessionHelperService.Setup(p => p.GetSession<Model.Common.Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation);
            _shoppingCartService.Setup(p => p.GetRegisterSeats<FlightReservationResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((flightReservation, 9223372036854775807)));
            _sessionHelperService.Setup(p => p.GetSession<MOBShoppingCart>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(shoppingResponse);
            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);
            if (registerSeatsRequest.Application.Id != 10)
                //_sessionHelperService.Setup(p => p.GetSession<List<Model.ShopSeats.SeatMapCSL30>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatMapCSL30Response);
            _productInfoHelper.Setup(p => p.GetProductBasedTermAndConditions(It.IsAny<string>(), It.IsAny<FlightReservationResponse>(), It.IsAny<bool>())).ReturnsAsync(shoppingResponse.TermsAndConditions);
            _seatEnginePostService.Setup(p => p.SeatEnginePostNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(FlightSeatMapDetail);
            _productInfoHelper.Setup(p => p.ConfirmationPageProductInfo(It.IsAny<FlightReservationResponse>(), It.IsAny<bool>(), It.IsAny<MOBApplication>(), It.IsAny<SeatChangeState>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(shoppingResponse.Products);
            if (registerSeatsRequest.Application.Id == 100)
            {
                _configuration["EnableCSL30ManageResSelectSeatMap"] = "False";
                _configuration["SwithToCSLSeatMapChangeSeats"] = "True";
            }
            if (registerSeatsRequest.Application.Id == 101)
            {
                _configuration["EnableEtcforSeats_PCU_Viewres"] = "False";
            }
            _formsOfPayment.Setup(p => p.EligibleFormOfPayments(It.IsAny<FOPEligibilityRequest>(), It.IsAny<Model.Common.Session>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<List<LogEntry>>())).Returns(Task.FromResult((formResponse, false)));

            _cachingService.Setup(p => p.GetCache<PKDispenserKey>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(pKDispenser);

            _cachingService.Setup(p => p.GetCache<string>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"PublicKey\":\"Public Key\",\"CryptoTypeID\":\"1\",\"Kid\":\"2\",\"Exp\":\"try again\"}");

            _sessionHelperService.Setup(p => p.GetSession<List<Model.ShopSeats.SeatMapCSL30>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(persistedCSL30SeatMaps);


            var result = seatMapBusiness.RegisterSeats(registerSeatsRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RegisterSeats), MemberType = typeof(TestDataGenerator))]
        public void RegisterSeats_Tests_Exception(MOBRegisterSeatsRequest registerSeatsRequest, SeatChangeState seatChangeState, CheckOutResponse checkOutResponse)
        {
            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var seatMapCSL30Response = JsonConvert.DeserializeObject<List<Model.ShopSeats.SeatMapCSL30>>(GetFileContent("SeatMapCSL30Response.json"));
            var seatResponse = JsonConvert.DeserializeObject<List<Model.Common.MOBSeatMap>>(GetFileContent("MOBSeatMapResponse.json"));
            var reservation = JsonConvert.DeserializeObject<Service.Presentation.ReservationResponseModel.ReservationDetail>(GetFileContent("ReservationDetailResponse.json"));
            var pcu = JsonConvert.DeserializeObject<PcuState>(GetFileContent("PcuStateResponse.json"));
            var offers = JsonConvert.DeserializeObject<GetOffers>(GetFileContent("OffersResponse.json"));
            var flightReservation = JsonConvert.DeserializeObject<FlightReservationResponse>(GetFileContent("FlightReservationResponse.json"));
            var shoppingResponse = JsonConvert.DeserializeObject<MOBShoppingCart>(GetFileContent("ShoppingCartResponse.json"));
            var prodResponse = JsonConvert.DeserializeObject<List<ProdDetail>>(GetFileContent("ProdDetailResponse.json"));
            var reservationResponse = JsonConvert.DeserializeObject<Reservation>(GetFileContent("Reservation.json"));
            var pKDispenser = GetFileContent("PKDispenserKey.json");
            var pKDispenserResponse = JsonConvert.DeserializeObject<PKDispenserResponse>(GetFileContent("PKDispenserResponse.json"));
            var formResponse = JsonConvert.DeserializeObject<List<FormofPaymentOption>>(GetFileContent("EligibleFormOfPayment.json"));
            var FlightSeatMapDetail = GetFileContent("FlightSeatMapDetail.json");
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<PcuState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(pcu);
            _sessionHelperService.Setup(p => p.GetSession<GetOffers>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(offers);
            _sessionHelperService.Setup(p => p.GetSession<SeatChangeState>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatChangeState);
            if (registerSeatsRequest.SessionId != "1")
            {
                _sessionHelperService.Setup(p => p.GetSession<List<Model.Common.MOBSeatMap>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ThrowsAsync(new Exception("Error Occurred"));

            }
            _sessionHelperService.Setup(p => p.GetSession<Model.Common.Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation);
            _shoppingCartService.Setup(p => p.GetRegisterSeats<FlightReservationResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult((flightReservation, 9223372036854775807)));
            _sessionHelperService.Setup(p => p.GetSession<MOBShoppingCart>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(shoppingResponse);
            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);

            _sessionHelperService.Setup(p => p.GetSession<List<Model.ShopSeats.SeatMapCSL30>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(seatMapCSL30Response);

            _productInfoHelper.Setup(p => p.GetProductBasedTermAndConditions(It.IsAny<string>(), It.IsAny<FlightReservationResponse>(), It.IsAny<bool>())).ReturnsAsync(shoppingResponse.TermsAndConditions);
            _seatEnginePostService.Setup(p => p.SeatEnginePostNew(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(FlightSeatMapDetail);
            _productInfoHelper.Setup(p => p.ConfirmationPageProductInfo(It.IsAny<FlightReservationResponse>(), It.IsAny<bool>(), It.IsAny<MOBApplication>(), It.IsAny<SeatChangeState>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(shoppingResponse.Products);
            if (registerSeatsRequest.Application.Id == 100)
            {
                _configuration["EnableCSL30ManageResSelectSeatMap"] = "False";
                _configuration["SwithToCSLSeatMapChangeSeats"] = "True";
            }
            if (registerSeatsRequest.Application.Id == 101)
            {
                _configuration["EnableEtcforSeats_PCU_Viewres"] = "False";
            }
            _formsOfPayment.Setup(p => p.EligibleFormOfPayments(It.IsAny<FOPEligibilityRequest>(), It.IsAny<Model.Common.Session>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<List<LogEntry>>())).Returns(Task.FromResult((formResponse, false)));

            _cachingService.Setup(p => p.GetCache<PKDispenserKey>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(pKDispenser);
            var result = seatMapBusiness.RegisterSeats(registerSeatsRequest);

            _cachingService.Setup(p => p.GetCache<string>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"PublicKey\":\"Public Key\",\"CryptoTypeID\":\"1\",\"Kid\":\"2\",\"Exp\":\"try again\"}");



            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

       
    }
}
