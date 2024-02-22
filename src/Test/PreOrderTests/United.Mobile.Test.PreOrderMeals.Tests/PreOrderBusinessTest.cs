using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.PreOrderMeals;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Payment;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Services.PreOrderMeals.Domain;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Utility.Helper;
using United.Utility.Http;
using Xunit;
using Constants = United.Mobile.Model.Constants;

namespace United.Mobile.Test.PreOrderMeals.Tests
{
   public class PreOrderBusinessTest
    {
        private readonly Mock<ICacheLog<PreOrderMealsBusiness>> _logger;
        private readonly PreOrderMealsBusiness _preOrderMealsBusiness;
        private readonly Mock<ICacheLog<DataPowerFactory>> _logger9;
        private readonly Mock<IHeaders> _headers;
        private IConfiguration _configuration;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<IFFCShoppingcs> _fFCShoppingcs;
        private readonly Mock<IMerchandizingServices> _merchandizingServices;
        private List<CMSContentMessage> cmsContents;
        private readonly Mock<IRegisterCFOP> _registerCFOP;
        private readonly Mock<ISSOTokenKeyHelper> _sSOTokenKeyHelper;
        private readonly Mock<IFlightReservation> _flightReservation;
        private readonly Mock<IUnfinishedBooking> _unfinishedBooking;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<IPreOrderMealRegisterService> _preOrderMealRegisterService;
        private readonly Mock<IGetMealOfferDetailsFromCslService> _getMealOfferDetailsFromCslService;
        private readonly Mock<IGetPNRByRecordLocatorService> _getPNRByRecordLocatorService;
        private readonly Mock<IRegisterOffersService> _registerOffersService;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy _policyWrap;
        private readonly string _baseUrl;
        private readonly IDataPowerFactory _dataPowerFactory;
        private readonly IResilientClient _resilientClient;
        private readonly Mock<IFeatureSettings> _featureSettings;


        public IConfiguration Configuration
        {
            get
            {
                //if (_configuration == null)
                //{
                _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .Build();
                //}
                return _configuration;
            }
        }

        public PreOrderBusinessTest()
        {
            _logger = new Mock<ICacheLog<PreOrderMealsBusiness>>();
            _logger9 = new Mock<ICacheLog<DataPowerFactory>>();
            _headers = new Mock<IHeaders>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _fFCShoppingcs = new Mock<IFFCShoppingcs>();
            _registerCFOP = new Mock<IRegisterCFOP>();
            _sSOTokenKeyHelper = new Mock<ISSOTokenKeyHelper>();
            _flightReservation = new Mock<IFlightReservation>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _dPService = new Mock<IDPService>();
            _preOrderMealRegisterService = new Mock<IPreOrderMealRegisterService>();
            _getMealOfferDetailsFromCslService = new Mock<IGetMealOfferDetailsFromCslService>();
            _getPNRByRecordLocatorService = new Mock<IGetPNRByRecordLocatorService>();
            _registerOffersService = new Mock<IRegisterOffersService>();
            _unfinishedBooking = new Mock<IUnfinishedBooking>();
            _merchandizingServices = new Mock<IMerchandizingServices>();
            _dataPowerFactory = new DataPowerFactory(Configuration, _sessionHelperService.Object, _logger9.Object);
            _featureSettings = new Mock<IFeatureSettings>();

            _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
              .Build();


            _resilientClient = new ResilientClient(_baseUrl);


            _preOrderMealsBusiness = new PreOrderMealsBusiness(_logger.Object, _configuration, _headers.Object, _shoppingSessionHelper.Object, _sessionHelperService.Object, _fFCShoppingcs.Object, _merchandizingServices.Object, _registerCFOP.Object, _sSOTokenKeyHelper.Object, _flightReservation.Object, _unfinishedBooking.Object, _dPService.Object, _preOrderMealRegisterService.Object, _getMealOfferDetailsFromCslService.Object, _getPNRByRecordLocatorService.Object, _registerOffersService.Object, _shoppingUtility.Object, _featureSettings.Object);
       


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


        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealOffers_Request), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealOffers(MOBInFlightMealsOfferRequest mOBInFlightMealsOfferRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, MOBInFlightMealsOfferResponse mOBInFlightMealsOfferResponse)
        {


            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);

            _merchandizingServices.Setup(p => p.GetMerchOffersDetailsFromCCE(It.IsAny<Session>(), It.IsAny<Service.Presentation.ReservationModel.Reservation>(), It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<MOBPNR>(), It.IsAny<string>(), false)).ReturnsAsync(dynamicOfferDetailResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBInFlightMealsOfferResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsOfferResponse);

         

            //Act
            var result = _preOrderMealsBusiness.GetInflightMealOffers(mOBInFlightMealsOfferRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealOffers_Request1), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealOffers1(MOBInFlightMealsOfferRequest mOBInFlightMealsOfferRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, MOBInFlightMealsOfferResponse mOBInFlightMealsOfferResponse)
        {

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);

            _merchandizingServices.Setup(p => p.GetMerchOffersDetailsFromCCE(It.IsAny<Session>(), It.IsAny<Service.Presentation.ReservationModel.Reservation>(), It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<MOBPNR>(), It.IsAny<string>(), false)).ReturnsAsync(dynamicOfferDetailResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBInFlightMealsOfferResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsOfferResponse);



            //Act
            var result = _preOrderMealsBusiness.GetInflightMealOffers(mOBInFlightMealsOfferRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealOffers_flow), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealOffers_flow(MOBInFlightMealsOfferRequest mOBInFlightMealsOfferRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, MOBInFlightMealsOfferResponse mOBInFlightMealsOfferResponse)
        {
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);
            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);
            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);
            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);
            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);
            _merchandizingServices.Setup(p => p.GetMerchOffersDetailsFromCCE(It.IsAny<Session>(), It.IsAny<Service.Presentation.ReservationModel.Reservation>(), It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<MOBPNR>(), It.IsAny<string>(), false)).ReturnsAsync(dynamicOfferDetailResponse);
            _sessionHelperService.Setup(p => p.GetSession<MOBInFlightMealsOfferResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsOfferResponse);
            //Act
            var result = _preOrderMealsBusiness.GetInflightMealOffers(mOBInFlightMealsOfferRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealRefreshments_Request), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealRefreshments(MOBInFlightMealsRefreshmentsRequest mOBInFlightMealsRefreshmentsRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, CheckOutResponse checkOutResponse, GetOffersCce getOffersCce, List<MOBInFlightMealsRefreshmentsResponse> mOBInFlightMealsRefreshmentsResponses )
        {

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);

            _sessionHelperService.Setup(p => p.GetSession<DynamicOfferDetailResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(dynamicOfferDetailResponse);

            DynamicOfferDetailResponse dynamicOfferDetailResponse1 = new DynamicOfferDetailResponse
            {
                Characteristics  = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic>
                {
                  new Service.Presentation.CommonModel.Characteristic()
                  {
                       Code = "24HrFlexibleBookingPolicy",
                       Value = "true",
                       Description = "WEARE"
                  }
                }
            };

            //GetOffersCce getOffersCce = new GetOffersCce
            //{
            //    ObjectName = "United.Persist.Definition.Merchandizing.GetOffersCce",
            //    OfferResponseJson = "QWERTYUIOPLKJGD"
            //};

            //var response2 = JsonConvert.SerializeObject(dynamicOfferDetailResponse1);

             _sessionHelperService.Setup(p => p.GetSession<GetOffersCce>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(getOffersCce);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);


            MOBRegisterOfferResponse mOBRegisterOfferResponse = new MOBRegisterOfferResponse
            {
                LanguageCode = "en-US",
                Flow = "Booking",
                SessionId = "63E14B9EA533467DADFED455A89CE9B9",
                IsDefaultPaymentOption = true

            };
            var response = JsonConvert.SerializeObject(mOBRegisterOfferResponse);

            _registerOffersService.Setup(p => p.RegisterOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            //_registerOffersService.Setup(p => p. RegisterOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"SessionId\":\"63E14B9EA533467DADFED455A89CE9B9\",\"Flow\":\"BOOKING\",\"IsDefaultPaymentOption\":true,\"application\":{\"id\":2,\"isProduction\":false,\"name\":\"Android\",\"version\":{\"major\":\"4.1.62\",\"minor\":\"4.1.62\"}},\"deviceId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2\",\"languageCode\":\"en-US\",\"transactionId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|a13b00e2-19f4-48d9-b614-79e14189f5f6\"}");

            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);

            _shoppingUtility.Setup(p => p.EnableEditForAllCabinPOM(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<MOBItem>>())).Returns(true);

            _sessionHelperService.Setup(p => p.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsRefreshmentsResponses);

            //Act
            var result = _preOrderMealsBusiness.GetInflightMealRefreshments(mOBInFlightMealsRefreshmentsRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }
        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealRefreshments_flow), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealRefreshments_flow(MOBInFlightMealsRefreshmentsRequest mOBInFlightMealsRefreshmentsRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, CheckOutResponse checkOutResponse, GetOffersCce getOffersCce, List<MOBInFlightMealsRefreshmentsResponse> mOBInFlightMealsRefreshmentsResponses)
        {
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);
            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);
            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);
            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);
            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);
            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);
            _sessionHelperService.Setup(p => p.GetSession<DynamicOfferDetailResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(dynamicOfferDetailResponse);
            DynamicOfferDetailResponse dynamicOfferDetailResponse1 = new DynamicOfferDetailResponse
            {
                Characteristics = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic>
                {
                  new Service.Presentation.CommonModel.Characteristic()
                  {
                       Code = "24HrFlexibleBookingPolicy",
                       Value = "true",
                       Description = "WEARE"
                  }
                }
            };
            _sessionHelperService.Setup(p => p.GetSession<GetOffersCce>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(getOffersCce);
            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);
            MOBRegisterOfferResponse mOBRegisterOfferResponse = new MOBRegisterOfferResponse
            {
                LanguageCode = "en-US",
                Flow = "Booking",
                SessionId = "63E14B9EA533467DADFED455A89CE9B9",
                IsDefaultPaymentOption = true
            };
            var response = JsonConvert.SerializeObject(mOBRegisterOfferResponse);
            _registerOffersService.Setup(p => p.RegisterOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);
            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);
            _shoppingUtility.Setup(p => p.EnableEditForAllCabinPOM(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<MOBItem>>())).Returns(true);
            _sessionHelperService.Setup(p => p.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsRefreshmentsResponses);
            //Act
            var result = _preOrderMealsBusiness.GetInflightMealRefreshments(mOBInFlightMealsRefreshmentsRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealOffersForDeeplink_Request), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealOffersForDeeplink(MOBInFlightMealsOfferRequest mOBInFlightMealsOfferRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, CheckOutResponse checkOutResponse, MOBInFlightMealsOfferResponse mOBInFlightMealsOfferResponse)
        {

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);

            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);

            _sessionHelperService.Setup(p => p.GetSession<DynamicOfferDetailResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(dynamicOfferDetailResponse);

            // _sessionHelperService.Setup(p => p.GetSession<GetOffersCce>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(getOffersCce);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);

            _registerOffersService.Setup(p => p.RegisterOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"SessionId\":\"63E14B9EA533467DADFED455A89CE9B9\",\"Flow\":\"BOOKING\",\"IsDefaultPaymentOption\":true,\"application\":{\"id\":2,\"isProduction\":false,\"name\":\"Android\",\"version\":{\"major\":\"4.1.62\",\"minor\":\"4.1.62\"}},\"deviceId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2\",\"languageCode\":\"en-US\",\"transactionId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|a13b00e2-19f4-48d9-b614-79e14189f5f6\"}");

            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);

            MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse = new MOBPNRByRecordLocatorResponse
            {
                ShowAddCalendar = true,
                ShowPremierAccess = true,
                UARecordLocator = "QWERTY12",
                DeviceId = "6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2"
            };
            var jsonResponse = JsonConvert.SerializeObject(mOBPNRByRecordLocatorResponse);


            _getPNRByRecordLocatorService.Setup(p => p.GetPNRByRecordLocator(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(jsonResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBInFlightMealsOfferResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsOfferResponse);

            //Act

            var result = _preOrderMealsBusiness.GetInflightMealOffersForDeeplink(mOBInFlightMealsOfferRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInflightMealOffersForDeeplink_flow), MemberType = typeof(TestDataGenerator))]
        public void GetInflightMealOffersForDeeplink_flow(MOBInFlightMealsOfferRequest mOBInFlightMealsOfferRequest, Session session, List<CMSContentMessage> cMSContentMessages, ReservationDetail reservationDetail, MOBPNR mOBPNR, DynamicOfferDetailResponse dynamicOfferDetailResponse, CheckOutResponse checkOutResponse, MOBInFlightMealsOfferResponse mOBInFlightMealsOfferResponse)
        {
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);
            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);
            _fFCShoppingcs.Setup(p => p.GetSDLContentByGroupName(It.IsAny<MOBRequest>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(cMSContentMessages);
            _shoppingUtility.Setup(p => p.ShopTimeOutCheckforAppVersion(It.IsAny<int>(), It.IsAny<string>())).Returns(true);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);
            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);
            _sessionHelperService.Setup(p => p.GetSession<MOBPNR>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBPNR);
            _sessionHelperService.Setup(p => p.GetSession<DynamicOfferDetailResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(dynamicOfferDetailResponse);
            // _sessionHelperService.Setup(p => p.GetSession<GetOffersCce>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(getOffersCce);
            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);
            _registerOffersService.Setup(p => p.RegisterOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"SessionId\":\"63E14B9EA533467DADFED455A89CE9B9\",\"Flow\":\"BOOKING\",\"IsDefaultPaymentOption\":true,\"application\":{\"id\":2,\"isProduction\":false,\"name\":\"Android\",\"version\":{\"major\":\"4.1.62\",\"minor\":\"4.1.62\"}},\"deviceId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2\",\"languageCode\":\"en-US\",\"transactionId\":\"6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|a13b00e2-19f4-48d9-b614-79e14189f5f6\"}");
            _registerCFOP.Setup(p => p.RegisterFormsOfPayments_CFOP(It.IsAny<CheckOutRequest>())).ReturnsAsync(checkOutResponse);
            MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse = new MOBPNRByRecordLocatorResponse
            {
                ShowAddCalendar = true,
                ShowPremierAccess = true,
                UARecordLocator = "QWERTY12",
                DeviceId = "6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2"
            };
            var jsonResponse = JsonConvert.SerializeObject(mOBPNRByRecordLocatorResponse);
            _getPNRByRecordLocatorService.Setup(p => p.GetPNRByRecordLocator(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(jsonResponse);
            _sessionHelperService.Setup(p => p.GetSession<MOBInFlightMealsOfferResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBInFlightMealsOfferResponse);
            //Act
            var result = _preOrderMealsBusiness.GetInflightMealOffersForDeeplink(mOBInFlightMealsOfferRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

    }
}
