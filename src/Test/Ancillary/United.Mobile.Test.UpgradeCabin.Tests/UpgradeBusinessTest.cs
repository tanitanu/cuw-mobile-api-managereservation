using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Polly;
using System;
using System.IO;
using System.Net.Http;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Profile;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.UpgradeCabin;
using United.Mobile.UpgradeCabin.Domain;
using United.Service.Presentation.LoyaltyResponseModel;
using United.Utility.Helper;
using United.Utility.Http;
using Xunit;
using Constants = United.Mobile.Model.Constants;

namespace United.Mobile.Test.UpgradeCabin.Tests
{
    public class UpgradeBusinessTest
    {
        private readonly Mock<ICacheLog<UpgradeCabinBusiness>> _logger;
        private readonly Mock<ICacheLog<CachingService>> _logger6;
        private readonly Mock<ICacheLog<DataPowerFactory>> _logger9;
        private readonly UpgradeCabinBusiness _upgradeCabinBusiness;
        private IConfiguration _configuration;
        private readonly Mock<IDPService> _dpService;
        private readonly Mock<IMileagePlus> _mileagePlus;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<ILegalDocumentsForTitlesService> _legalDocumentsForTitlesService;
        private readonly Mock<IProductOffers> _productOffers;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly ManageResUtility _manageResUtility;
        private readonly Mock<IPKDispenserService> _pKDispenserService;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IValidateHashPinService> _validateHashPinService;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<IUpgradeEligibilityService> _upgradeEligibilityService;
        private readonly Mock<ICachingService> _cachingService;
        private readonly ICachingService _cachingService1;
        private readonly IResilientClient _resilientClient;
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy _policyWrap;
        private readonly string _baseUrl;
        private readonly IDataPowerFactory _dataPowerFactory;
        private readonly Mock<IMPSignInCommonService> _mPSignInCommonService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IFeatureSettings> _featureSettings;

        public IConfiguration Configuration
        {
            get
            {
                _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .Build();

                return _configuration;
            }
        }

        public UpgradeBusinessTest()
        {
            _logger = new Mock<ICacheLog<UpgradeCabinBusiness>>();
            _logger6 = new Mock<ICacheLog<CachingService>>();
            _logger9 = new Mock<ICacheLog<DataPowerFactory>>();
            _dpService = new Mock<IDPService>();
            _mileagePlus = new Mock<IMileagePlus>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _legalDocumentsForTitlesService = new Mock<ILegalDocumentsForTitlesService>();
            _productOffers = new Mock<IProductOffers>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _pKDispenserService = new Mock<IPKDispenserService>();
            _headers = new Mock<IHeaders>();
            _validateHashPinService = new Mock<IValidateHashPinService>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _upgradeEligibilityService = new Mock<IUpgradeEligibilityService>();
            _mPSignInCommonService = new Mock<IMPSignInCommonService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _featureSettings = new Mock<IFeatureSettings>();
            _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
              .Build();

            _resilientClient = new ResilientClient(_baseUrl);

            _cachingService1 = new CachingService(_resilientClient, _logger6.Object, _configuration);


            _upgradeCabinBusiness = new UpgradeCabinBusiness(_logger.Object, _configuration, _dpService.Object, _mileagePlus.Object, _shoppingUtility.Object, _dynamoDBService.Object, _legalDocumentsForTitlesService.Object, _productOffers.Object, _sessionHelperService.Object, _pKDispenserService.Object, _headers.Object, _validateHashPinService.Object, _shoppingSessionHelper.Object, _upgradeEligibilityService.Object, _mPSignInCommonService.Object, _httpContextAccessor.Object, _featureSettings.Object);


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
        [MemberData(nameof(TestDataGenerator.UpgradePlusPointWebMyTrip_Request), MemberType = typeof(TestDataGenerator))]
        public void UpgradePlusPointWebMyTrip_Request(MOBUpgradePlusPointWebMyTripRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            //Act
            var result = _upgradeCabinBusiness.UpgradePlusPointWebMyTrip(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.UpgradePlusPointWebMyTrip_Request1), MemberType = typeof(TestDataGenerator))]
        public void UpgradePlusPointWebMyTrip_Request1(MOBUpgradePlusPointWebMyTripRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            //Act
            var result = _upgradeCabinBusiness.UpgradePlusPointWebMyTrip(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.UpgradePlusPointWebMyTrip_Flow), MemberType = typeof(TestDataGenerator))]
        public void UpgradePlusPointWebMyTrip_Flow(MOBUpgradePlusPointWebMyTripRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            //Act
            var result = _upgradeCabinBusiness.UpgradePlusPointWebMyTrip(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.UpgradeCabinEligibleCheck_Request), MemberType = typeof(TestDataGenerator))]
        public void UpgradeCabinEligibleCheck_Request(MOBUpgradeCabinEligibilityRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _dpService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            UpgradeEligibilityResponse upgradeEligibilityResponse = new UpgradeEligibilityResponse
            {
                CartID = "CD74D339-DBEA-4F74-8766-870BAB39DBCF",
                DataCenter = "data",
                TransactionID = "6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|04a5e3c8-bd07-4883-b4cf-64bbd73425f1"


            };
            var response = JsonConvert.SerializeObject(upgradeEligibilityResponse);

            _upgradeEligibilityService.Setup(p => p.GetUpgradeCabinEligibleCheck(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");



            //Act
            var result = _upgradeCabinBusiness.UpgradeCabinEligibleCheck(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.UpgradeCabinEligibleCheck_Request1), MemberType = typeof(TestDataGenerator))]
        public void UpgradeCabinEligibleCheck_Request1(MOBUpgradeCabinEligibilityRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _dpService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            //UpgradeEligibilityResponse upgradeEligibilityResponse = new UpgradeEligibilityResponse
            //{
            //    CartID = "CD74D339-DBEA-4F74-8766-870BAB39DBCF",
            //    DataCenter = "data",
            //    TransactionID = "6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|04a5e3c8-bd07-4883-b4cf-64bbd73425f1",
                
                


            //};
            //var response = JsonConvert.SerializeObject(upgradeEligibilityResponse);


            _upgradeEligibilityService.Setup(p => p.GetUpgradeCabinEligibleCheck(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"Characteristics\":[{\"Code\":\"RTEW567\",\"Value\":\"TREYU\"}],\"Reservation\":{\"FlightSegments\":[{\"HistoryClass\":\"yes\",\"FlightSegment\":{\"FlightSegmentType\":\"cabin\"},\"Seats\":[{\"PriceBeforeCouponApplied\":0,\"Adjustments\":[],\"SegmentIndex\":null,\"PriceAfterCouponApplied\":0,\"IsCouponApplied\":false,\"LimitedRecline\":false,\"SeatAssignment\":\"10E\",\"OldSeatAssignment\":null,\"Origin\":\"IAH\",\"Destination\":\"DEN\",\"FlightNumber\":689,\"DepartureDate\":null,\"TravelerSharesIndex\":\"0\",\"Key\":0,\"UAOperated\":true,\"Price\":0,\"PriceAfterTravelerCompanionRules\":0,\"Currency\":\"USD\",\"ProgramCode\":\"EPU\",\"SeatType\":\"EPLUSPRIMEPLUS\",\"OldSeatPrice\":0,\"IsEPA\":false,\"IsEPAFreeCompanion\":false,\"Miles\":0,\"MilesAfterTravelerCompanionRules\":0,\"OldSeatMiles\":0,\"MilesBeforeCouponApplied\":0,\"MilesAfterCouponApplied\":0}]}]},\"ServiceStatus\":{\"StatusType\":\"Success\",\"ServiceCode\":\"S76\",\"ServiceName\":\"XYZ\",\"ServiceMessages\":[{\"MessageCode\":\"M87543\",\"MessageType\":\"text\",\"MessageText\":\"try again\"}]}}");

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");



            //Act
            var result = _upgradeCabinBusiness.UpgradeCabinEligibleCheck(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }


        [Theory]
        [MemberData(nameof(TestDataGenerator.UpgradeCabinEligibleCheck_Flow), MemberType = typeof(TestDataGenerator))]
        public void UpgradeCabinEligibleCheck_Flow(MOBUpgradeCabinEligibilityRequest request, Session session, HashPinValidate hashPinValidate)
        {

            _dpService.Setup(p => p.GetAnonymousToken(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>())).ReturnsAsync("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");

            UpgradeEligibilityResponse upgradeEligibilityResponse = new UpgradeEligibilityResponse
            {
                CartID = "CD74D339-DBEA-4F74-8766-870BAB39DBCF",
                DataCenter = "data",
                TransactionID = "6f51874a-3ebb-4ed7-8e6b-8aaf96f414d2|04a5e3c8-bd07-4883-b4cf-64bbd73425f1"


            };
            var response = JsonConvert.SerializeObject(upgradeEligibilityResponse);

            _upgradeEligibilityService.Setup(p => p.GetUpgradeCabinEligibleCheck(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(session);

            _shoppingSessionHelper.Setup(p => p.GetValidateSession(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(session);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dpService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns("Bearer AAJITW9iaWxlLUFuZHJvaWRQaG9uZS1jdXN0b21lcnNzb19VQUxfNjQzRTFFNDctMTI0Mi00QjZDLUFCN0UtNjQwMjRFNEJDODRDsWEErZ7_l1V0_BBZ-ThYtUqwjfpWng5K8hGPHCfY_bVihsW5XYtbN5PmIxPSQoAIM5KC2oUGUQGh96Y9TtIKuaw9P4lm0WBuqr63mw6v6YeQ1SYLlIMp0QliuMwWxaqcrdGf33-kLQ02YqCtzjYFFMDDplSrOcvS2CzOPRyayb-GF88s6MvXj3vzYR9QmOlG3v4U7Lop2b1xRF0cUix86A");



            //Act
            var result = _upgradeCabinBusiness.UpgradeCabinEligibleCheck(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }
    }
}
