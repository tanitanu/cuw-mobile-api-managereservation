using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using United.Common.Helper;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Common.HelperSeatEngine;
using United.Ebs.Logging.Enrichers;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.ReShop;
using United.Mobile.ManageReservation.Domain;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.Internal;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.ReShop;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.ReservationResponseModel;
using United.Utility.Helper;
using United.Utility.Http;
using Xunit;
using Constants = United.Mobile.Model.Constants;
using MOBPNRByRecordLocatorResponse = United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse;

namespace United.Mobile.Test.ManageReservation.Tests
{
    public class ManageReservationBusinessTest
    {
        private readonly Mock<ICacheLog<ManageReservationBusiness>> _logger;
        private readonly Mock<IHeaders> _headers;
        private readonly IConfiguration _configuration;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<IProductInfoHelper> _productInfoHelper;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<IManageReservation> _manageReservation;
        private readonly Mock<ManageResUtility> _manageResUtility;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<ISeatEngine> _seatEngine;
        private readonly ManageReservationBusiness manageReservationBusiness;
        private readonly Mock<ILegalDocumentsForTitlesService> _legalDocumentsForTitlesService;
        private readonly Mock<IPNRRetrievalService> _PNRRetrievalService;
        private readonly Mock<IRequestReceiptByEmailService> _requestReceiptByEmailService;
        private readonly Mock<ISendReceiptByEmailService> _sendReceiptByEmailService;
        private readonly Mock<IReservationService> _reservationService;
        private readonly Mock<IValidateHashPinService> _validateHashPinService;
        private readonly Mock<ICachingService> _cachingService;
        private readonly ICachingService _cachingService1;
        private readonly IDataPowerFactory _dataPowerFactory;
        private readonly Mock<ICacheLog<DataPowerFactory>> _logger2;
        private readonly Mock<ICacheLog<CachingService>> _logger1;
        private readonly IResilientClient _resilientClient;
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy _policyWrap;
        private readonly string _baseUrl;
        private readonly Mock<ICustomerProfileService> _customerProfileService;
        private readonly Mock<ILoyaltyMemberProfileService> _loyaltyMemberProfileService;
        private readonly Mock<Common.Helper.Merchandize.IMerchandizingServices> _merchandizingServices;
        private readonly Mock<IApplicationEnricher> _applicationEnricher;
        private readonly Mock<IAuroraMySqlService> _auroraMySqlService;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly Mock<ISeatMapEngine> _seatMapEngine;
        private readonly Mock<IFeatureSettings> _featureSettings;

        public ManageReservationBusinessTest()
        {
            _logger = new Mock<ICacheLog<ManageReservationBusiness>>();
            _logger1 = new Mock<ICacheLog<CachingService>>();
            _logger2 = new Mock<ICacheLog<DataPowerFactory>>();
            _dPService = new Mock<IDPService>();
            _productInfoHelper = new Mock<IProductInfoHelper>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _manageReservation = new Mock<IManageReservation>();
            _manageResUtility = new Mock<ManageResUtility>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _seatEngine = new Mock<ISeatEngine>();
            _PNRRetrievalService = new Mock<IPNRRetrievalService>();
            _reservationService = new Mock<IReservationService>();
            _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
              .Build();
            _legalDocumentsForTitlesService = new Mock<ILegalDocumentsForTitlesService>();
            _headers = new Mock<IHeaders>();
            _requestReceiptByEmailService = new Mock<IRequestReceiptByEmailService>();
            _sendReceiptByEmailService = new Mock<ISendReceiptByEmailService>();
            _customerProfileService = new Mock<ICustomerProfileService>();
            _loyaltyMemberProfileService = new Mock<ILoyaltyMemberProfileService>();
            _merchandizingServices = new Mock<IMerchandizingServices>();
            _applicationEnricher = new Mock<IApplicationEnricher>();
            _auroraMySqlService = new Mock<IAuroraMySqlService>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _seatMapEngine = new Mock<ISeatMapEngine>();
            _featureSettings = new Mock<IFeatureSettings>();

        manageReservationBusiness = new ManageReservationBusiness(_logger.Object, _headers.Object, _configuration, _shoppingSessionHelper.Object
                , _sessionHelperService.Object, _manageReservation.Object, _dynamoDBService.Object, _legalDocumentsForTitlesService.Object, _dPService.Object, _PNRRetrievalService.Object, _requestReceiptByEmailService.Object,
                _sendReceiptByEmailService.Object, _reservationService.Object, _productInfoHelper.Object, _customerProfileService.Object, _loyaltyMemberProfileService.Object, _seatEngine.Object, _applicationEnricher.Object, _auroraMySqlService.Object, _shoppingUtility.Object, _seatMapEngine.Object, _featureSettings.Object);

            _validateHashPinService = new Mock<IValidateHashPinService>();
            _cachingService = new Mock<ICachingService>();

            _resilientClient = new ResilientClient(_baseUrl);

            _cachingService1 = new CachingService(_resilientClient, _logger1.Object, _configuration);



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
        [MemberData(nameof(TestDataGenerator.GetPNRByRecordLocator), MemberType = typeof(TestDataGenerator))]
        public void GetPNRByRecordLocator_Tests(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {
            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.GetPNRByRecordLocator(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
            {
                Assert.True(result != null && result.Exception == null);
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
            }

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetPNRByRecordLocator1), MemberType = typeof(TestDataGenerator))]
        public void GetPNRByRecordLocator_Tests1(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));


            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);



            var result = manageReservationBusiness.GetPNRByRecordLocator(pnrByRecordLocatorRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetPNRByRecordLocator1_1), MemberType = typeof(TestDataGenerator))]
        public void GetPNRByRecordLocator_Tests1_1(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));


            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (pnrByRecordLocatorRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                _configuration["EnableEncryptedPNRRequest"] = "True";
            }


            var result = manageReservationBusiness.GetPNRByRecordLocator(pnrByRecordLocatorRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetPNRByRecordLocator_Neg), MemberType = typeof(TestDataGenerator))]
        public void GetPNRByRecordLocator_NegTests(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent(@"NegativeTestCases\SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            // var sessionData = JsonConvert.DeserializeObject<List<Session>>(GetFileContent(@"NegativeTestCases\SessionData.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData[1]);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData[1]);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (pnrByRecordLocatorRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                _configuration["ToggleCovidEmergencytextTPI"] = "False";
            }

            var result = manageReservationBusiness.GetPNRByRecordLocator(pnrByRecordLocatorRequest);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
            {
                Assert.True(result != null && result.Exception == null);
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
            }

        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.GetPNRByRecordLocator_Flow), MemberType = typeof(TestDataGenerator))]
        public void GetPNRByRecordLocator_Flow(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {
            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);
            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.GetPNRByRecordLocator(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
            {
                Assert.True(result != null && result.Exception == null);
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
            }

        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.PerformInstantUpgrade), MemberType = typeof(TestDataGenerator))]
        public void PerformInstantUpgrade_Tests(MOBInstantUpgradeRequest instantUpgradeRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.PerformInstantUpgrade(instantUpgradeRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.PerformInstantUpgrade_exception), MemberType = typeof(TestDataGenerator))]
        public void PerformInstantUpgrade_exception(MOBInstantUpgradeRequest instantUpgradeRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (instantUpgradeRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                _configuration["SwithToCSLPNRService"] = "False";
            }

            var result = manageReservationBusiness.PerformInstantUpgrade(instantUpgradeRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.PerformInstantUpgrade_neg), MemberType = typeof(TestDataGenerator))]
        public void PerformInstantUpgrade_neg(MOBInstantUpgradeRequest instantUpgradeRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {
            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.PerformInstantUpgrade(instantUpgradeRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
            {
                //Assert.True(result != null && result.Exception == null);
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
            }

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.PerformInstantUpgrade_Flow), MemberType = typeof(TestDataGenerator))]
        public void PerformInstantUpgrade_Flow(MOBInstantUpgradeRequest instantUpgradeRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);
            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.PerformInstantUpgrade(instantUpgradeRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);

        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOneClickEnrollmentDetailsForPNR), MemberType = typeof(TestDataGenerator))]
        public void GetOneClickEnrollmentDetailsForPNR_Tests(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOneClickEnrollmentDetailsForPNR_codecov), MemberType = typeof(TestDataGenerator))]
        public void GetOneClickEnrollmentDetailsForPNR_codecov(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (pnrByRecordLocatorRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                pnrByRecordLocatorRequest.IsRefreshedUserData = true;
            }

            var result = manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOneClickEnrollmentDetailsForPNR_codecov1), MemberType = typeof(TestDataGenerator))]
        public void GetOneClickEnrollmentDetailsForPNR_codecove(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[1]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (pnrByRecordLocatorRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                pnrByRecordLocatorRequest.IsRefreshedUserData = true;
            }

            var result = manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOneClickEnrollmentDetailsForPNR_codecov1), MemberType = typeof(TestDataGenerator))]
        public void GetOneClickEnrollmentDetailsForPNR_exception(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[2]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);

            if (pnrByRecordLocatorRequest.DeviceId == "E830EDE6-5FE0-469F-B785-979E55D67C18")
            {
                pnrByRecordLocatorRequest.IsRefreshedUserData = true;
            }

            var result = manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
            {
                // Assert.True(result != null && result.Exception == null);
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
            }

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetOneClickEnrollmentDetailsForPNR_Flow), MemberType = typeof(TestDataGenerator))]
        public void GetOneClickEnrollmentDetailsForPNR_Flow(MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest, MOBPNRByRecordLocatorResponse pnrByRecordLocatorResponse)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<Model.Common.Session>(session);
            var reservation = JsonConvert.DeserializeObject<List<Service.Presentation.ReservationResponseModel.ReservationDetail>>(GetFileContent("ReservationDetailResponse.json"));

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(sessionData);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData);

            _sessionHelperService.Setup(p => p.GetSession<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservation[0]);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(pnrByRecordLocatorResponse);


            var result = manageReservationBusiness.GetOneClickEnrollmentDetailsForPNR(pnrByRecordLocatorRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ConfirmScheduleChange_Test), MemberType = typeof(TestDataGenerator))]
        public void ConfirmScheduleChange_Test(MOBConfirmScheduleChangeRequest mOBConfirmScheduleChangeRequest, Session session, HashPinValidate hashPinValidate, ReservationDetail reservationDetail, MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse)
        {

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _dPService.Setup(p => p.GetAndSaveAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<Session>())).ReturnsAsync("Bearer Token");

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _cachingService.Setup(p => p.GetCache<DPTokenResponse>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("Token");


            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
            {
                new Service.Presentation.CommonModel.Message()
                {
                    Code = "q123",
                    ContentType = "ctext",
                    DisplaySequence = 1,
                    Key = "F941288F5D0E441720E",
                    Number = 2,
                    Status = "success",
                    Text = "message",
                    Type = "text",
                    Value = "FR567"

                }
            };
            var cslResponse = JsonConvert.SerializeObject(messages);

            _reservationService.Setup(p => p.ConfirmScheduleChange<List<United.Service.Presentation.CommonModel.Message>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cslResponse);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(mOBPNRByRecordLocatorResponse);


            //Act
            var result = manageReservationBusiness.ConfirmScheduleChange(mOBConfirmScheduleChangeRequest);
            //Assert
            Assert.True(result.Exception != null || result.Result != null);

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.ConfirmScheduleChange_excep), MemberType = typeof(TestDataGenerator))]
        public void ConfirmScheduleChange_exception(MOBConfirmScheduleChangeRequest mOBConfirmScheduleChangeRequest, Session session, HashPinValidate hashPinValidate, ReservationDetail reservationDetail, MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse)
        {

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(session);
            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _dPService.Setup(p => p.GetAndSaveAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<Session>())).ReturnsAsync("Bearer Token");

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _cachingService.Setup(p => p.GetCache<DPTokenResponse>(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("Token");


            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
            {
                new Service.Presentation.CommonModel.Message()
                {
                    Code = "q123",
                    ContentType = "ctext",
                    DisplaySequence = 1,
                    Key = "F941288F5D0E441720E",
                    Number = 2,
                    Status = "success",
                    Text = "message",
                    Type = "text",
                    Value = "FR567"

                }
            };
            var cslResponse = JsonConvert.SerializeObject(messages);

            _reservationService.Setup(p => p.ConfirmScheduleChange<List<United.Service.Presentation.CommonModel.Message>>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cslResponse);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(mOBPNRByRecordLocatorResponse);


            //Act
            var result = manageReservationBusiness.ConfirmScheduleChange(mOBConfirmScheduleChangeRequest);
            //Assert
            Assert.True(result.Exception != null || result.Result != null);

        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RequestReceiptByEmail), MemberType = typeof(TestDataGenerator))]
        public void RequestReceiptByEmail_Test(MOBReceiptByEmailRequest request, CommonDef commonDef)
        {
            //var CommonDef = GetFileContent("CommonDef.json");
            //var CommonDefData = JsonConvert.DeserializeObject<Session>(CommonDef);

            MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse = new MOBPNRByRecordLocatorResponse
            {
                ShowAddCalendar = true,
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                LanguageCode = "en-US",
                ShowPremierAccess = true

            };
            var response = JsonConvert.SerializeObject(mOBPNRByRecordLocatorResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            //  _shoppingSessionHelper.Setup(p => p.CheckIsCSSTokenValid(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Model.Common.Session>(), It.IsAny<string>())).Returns(Task.FromResult((true, "")));


            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
                {
                    new Service.Presentation.CommonModel.Message()
                    {
                        Code = "C1023",
                        ContentType = "messages",
                        DisplaySequence = 1,
                        Key = "xjZwAuAzMwLwL3MFW9NNV8G",
                        Number = 2,
                        Status = "success",
                        Text = "messages123",
                        Type = "text",
                        Value = "message1234"
                    }
                };
            var response1 = JsonConvert.SerializeObject(messages);

            _requestReceiptByEmailService.Setup(p => p.PostReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1);

            _dPService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Token");



            var result = manageReservationBusiness.RequestReceiptByEmail(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.Exception == null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RequestReceiptByEmail1), MemberType = typeof(TestDataGenerator))]
        public void RequestReceiptByEmail_Test1(MOBReceiptByEmailRequest request, CommonDef commonDef)
        {

            MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse = new MOBPNRByRecordLocatorResponse
            {
                ShowAddCalendar = true,
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                LanguageCode = "en-US",
                ShowPremierAccess = true

            };
            var response = JsonConvert.SerializeObject(mOBPNRByRecordLocatorResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            //  _shoppingSessionHelper.Setup(p => p.CheckIsCSSTokenValid(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Model.Common.Session>(), It.IsAny<string>())).Returns(Task.FromResult((true, "")));

            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
                {
                    new Service.Presentation.CommonModel.Message()
                    {
                        Code = "C1023",
                        ContentType = "messages",
                        DisplaySequence = 1,
                        Key = "xjZwAuAzMwLwL3MFW9NNV8G",
                        Number = 2,
                        Status = "success",
                        Text = "messages123",
                        Type = "text",
                        Value = "message1234"
                    }
                };
            var response1 = JsonConvert.SerializeObject(messages);

            _requestReceiptByEmailService.Setup(p => p.PostReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1);

            _dPService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Token");

            if (request.TransactionId == "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314")
            {
                _configuration["DeviceIDPNRSessionGUIDCaseSensitiveFix"] = "True";
            }

            SendRcptResponse sendRcptResponse = new SendRcptResponse
            {
                ErrMsg = "try again",
                Guid = "ad6526e4-bf8f-4dc1-bcb1-1b0",
                Status = "Success",
                TxnId = "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314"
            };
            var response2 = JsonConvert.SerializeObject(sendRcptResponse);

            _sendReceiptByEmailService.Setup(p => p.SendReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response2);

            var result = manageReservationBusiness.RequestReceiptByEmail(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.Exception == null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RequestReceiptByEmail1_1), MemberType = typeof(TestDataGenerator))]
        public void RequestReceiptByEmail_Test1_1(MOBReceiptByEmailRequest request, CommonDef commonDef)
        {

            MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse = new MOBPNRByRecordLocatorResponse
            {
                ShowAddCalendar = true,
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                LanguageCode = "en-US",
                ShowPremierAccess = true

            };
            var response = JsonConvert.SerializeObject(mOBPNRByRecordLocatorResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
                {
                    new Service.Presentation.CommonModel.Message()
                    {
                        Code = "C1023",
                        ContentType = "messages",
                        DisplaySequence = 1,
                        Key = "xjZwAuAzMwLwL3MFW9NNV8G",
                        Number = 2,
                        Status = "Success",
                        //Text = "messages123",
                        Text = "EMAIL RQSTD",
                        Type = "text",
                        Value = "message1234"
                    }
                };
            var response1 = JsonConvert.SerializeObject(messages);

            _requestReceiptByEmailService.Setup(p => p.PostReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1);

            _dPService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Token");

            if (request.TransactionId == "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314")
            {
                _configuration["DeviceIDPNRSessionGUIDCaseSensitiveFix"] = "True";
            }

            SendRcptResponse sendRcptResponse = new SendRcptResponse
            {
                ErrMsg = "try again",
                Guid = "ad6526e4-bf8f-4dc1-bcb1-1b0",
                Status = "Success",
                TxnId = "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314"
            };
            var response2 = JsonConvert.SerializeObject(sendRcptResponse);

            _sendReceiptByEmailService.Setup(p => p.SendReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response2);

            if (request.TransactionId == "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314")
            {
                _configuration["EnableNewSendReceiptByEmail"] = "False";
            }

            _requestReceiptByEmailService.Setup(p => p.PostReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1);

            var result = manageReservationBusiness.RequestReceiptByEmail(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.Exception == null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.RequestReceiptByEmail_exception), MemberType = typeof(TestDataGenerator))]
        public void RequestReceiptByEmail_exception(MOBReceiptByEmailRequest request, CommonDef commonDef)
        {

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            List<United.Service.Presentation.CommonModel.Message> messages = new List<Service.Presentation.CommonModel.Message>
                {
                    new Service.Presentation.CommonModel.Message()
                    {
                        Code = "C1023",
                        ContentType = "messages",
                        DisplaySequence = 1,
                        Key = "xjZwAuAzMwLwL3MFW9NNV8G",
                        Number = 2,
                        Status = "Success",
                        //Text = "messages123",
                        Text = "EMAIL RQSTD",
                        Type = "text",
                        Value = "message1234"
                    }
                };
            var response1 = JsonConvert.SerializeObject(messages);

            _requestReceiptByEmailService.Setup(p => p.PostReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1);

            _dPService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Token");

            if (request.TransactionId == "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314")
            {
                _configuration["DeviceIDPNRSessionGUIDCaseSensitiveFix"] = "True";
            }

            SendRcptResponse sendRcptResponse = new SendRcptResponse
            {
                ErrMsg = "try again",
                Guid = "ad6526e4-bf8f-4dc1-bcb1-1b0",
                Status = "Success",
                TxnId = "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314"
            };
            var response2 = JsonConvert.SerializeObject(sendRcptResponse);

            _sendReceiptByEmailService.Setup(p => p.SendReceiptByEmailViaCSL(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response2);

            if (request.TransactionId == "ad6526e4-bf8f-4dc1-bcb1-1b0866f4557d|9c120cc9-6dcd-497e-8144-3bd8f379f314")
            {
                _configuration["EnableNewSendReceiptByEmail"] = "False";
            }

            var result = manageReservationBusiness.RequestReceiptByEmail(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.Exception == null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetMileageAndStatusOptions_Request), MemberType = typeof(TestDataGenerator))]
        public void GetMileageAndStatusOptions(MOBMileageAndStatusOptionsRequest mOBMileageAndStatusOptionsRequest, Session session, ReservationDetail reservationDetail, DOTBaggageInfoResponse dOTBaggageInfoResponse, DOTBaggageInfo dOTBaggageInfo, GetOffers getOffers)
        {

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _merchandizingServices.Setup(p => p.GetDOTBaggageInfoWithPNR(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MOBSHOPReservation>(), It.IsAny<Service.Presentation.ReservationModel.Reservation>())).ReturnsAsync(dOTBaggageInfoResponse);


            _sessionHelperService.Setup(p => p.GetSession<GetOffers>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(getOffers);

            var manageReservationBusiness = new ManageReservationBusiness(_logger.Object, _headers.Object, _configuration, _shoppingSessionHelper.Object
                  , _sessionHelperService.Object, _manageReservation.Object, _dynamoDBService.Object, _legalDocumentsForTitlesService.Object, _dPService.Object, _PNRRetrievalService.Object, _requestReceiptByEmailService.Object, _sendReceiptByEmailService.Object, _reservationService.Object, _productInfoHelper.Object, _customerProfileService.Object, _loyaltyMemberProfileService.Object, _seatEngine.Object, _applicationEnricher.Object, _auroraMySqlService.Object, _shoppingUtility.Object, _seatMapEngine.Object, _featureSettings.Object);

            var result = manageReservationBusiness.GetMileageAndStatusOptions(mOBMileageAndStatusOptionsRequest);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetActionDetailsForOffers_Request), MemberType = typeof(TestDataGenerator))]
        public void GetActionDetailsForOffers(MOBGetActionDetailsForOffersRequest mOBGetActionDetailsForOffersRequest, Session session, MOBPNRByRecordLocatorResponse mOBPNRByRecordLocatorResponse, MOBSeatChangeInitializeResponse mOBSeatChangeInitializeResponse)
        {

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _manageReservation.Setup(p => p.GetPNRByRecordLocatorCommonMethod(It.IsAny<MOBPNRByRecordLocatorRequest>())).ReturnsAsync(mOBPNRByRecordLocatorResponse);

            //_manageReservation.Setup(p => p.SeatChangeInitialize(It.IsAny<MOBSeatChangeInitializeRequest>())).ReturnsAsync(mOBSeatChangeInitializeResponse);

            var manageReservationBusiness = new ManageReservationBusiness(_logger.Object, _headers.Object, _configuration, _shoppingSessionHelper.Object
                  , _sessionHelperService.Object, _manageReservation.Object, _dynamoDBService.Object, _legalDocumentsForTitlesService.Object, _dPService.Object, _PNRRetrievalService.Object, _requestReceiptByEmailService.Object, _sendReceiptByEmailService.Object, _reservationService.Object, _productInfoHelper.Object, _customerProfileService.Object, _loyaltyMemberProfileService.Object, _seatEngine.Object, _applicationEnricher.Object, _auroraMySqlService.Object, _shoppingUtility.Object, _seatMapEngine.Object, _featureSettings.Object);

            var result = manageReservationBusiness.GetActionDetailsForOffers(mOBGetActionDetailsForOffersRequest);

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
