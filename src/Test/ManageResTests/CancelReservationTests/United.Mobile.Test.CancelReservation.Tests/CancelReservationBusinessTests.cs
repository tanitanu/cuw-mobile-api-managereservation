using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Shopping;
using United.Mobile.CancelReservation.Domain;
using United.Mobile.DataAccess.CancelReservation;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.AccountManagement;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Service.Presentation.ReservationResponseModel;
using United.Utility.Helper;
using United.Utility.Http;
using Xunit;
using Constants = United.Mobile.Model.Constants;


namespace United.Mobile.Test.CancelReservation.Tests
{
    public class CancelReservationBusinessTests
    {
        private readonly Mock<ICacheLog<CancelReservationBusiness>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<ManageResUtility> _manageResUtility;
        private readonly Mock<IRegisterCFOP> _registerCFOP;
        private readonly Mock<IPaymentUtility> _paymentUtility;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<ICancelRefundService> _cancelRefundService;
        private readonly Mock<ICancelAndRefundService> _cancelAndRefundService;
        private readonly CancelReservationBusiness _cancelReservationBusiness;
        private readonly Mock<IFlightReservation> _flightReservation;
        private readonly Mock<IShoppingUtility> _shoppingUtility;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<IValidateHashPinService> _validateHashPinService;
        private readonly Mock<ILegalDocumentsForTitlesService> _legalDocumentsForTitlesService;
        private readonly Mock<IMPSignInCommonService> _mPSignInCommonService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IFeatureSettings> _featureSettings;

        public CancelReservationBusinessTests()
        {
            _logger = new Mock<ICacheLog<CancelReservationBusiness>>();
            _configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
              .Build();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _manageResUtility = new Mock<ManageResUtility>();
            _registerCFOP = new Mock<IRegisterCFOP>();
            _paymentUtility = new Mock<IPaymentUtility>();
            _dPService = new Mock<IDPService>();
            _cancelRefundService = new Mock<ICancelRefundService>();
            _cancelAndRefundService = new Mock<ICancelAndRefundService>();
            _flightReservation = new Mock<IFlightReservation>();
            _shoppingUtility = new Mock<IShoppingUtility>();
            _headers = new Mock<IHeaders>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _validateHashPinService = new Mock<IValidateHashPinService>();
            _legalDocumentsForTitlesService = new Mock<ILegalDocumentsForTitlesService>();
            _mPSignInCommonService = new Mock<IMPSignInCommonService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _featureSettings = new Mock<IFeatureSettings>();

            _cancelReservationBusiness = new CancelReservationBusiness(_logger.Object, _configuration, _shoppingSessionHelper.Object, _sessionHelperService.Object, _paymentUtility.Object, _registerCFOP.Object, _dPService.Object, _cancelRefundService.Object, _cancelAndRefundService.Object, _headers.Object, _dynamoDBService.Object, _validateHashPinService.Object, _legalDocumentsForTitlesService.Object, _mPSignInCommonService.Object, _httpContextAccessor.Object, _featureSettings.Object);
            
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
        [MemberData(nameof(TestDataGenerator.CancelRefundInfo), MemberType = typeof(TestDataGenerator))]
        public void CancelRefundInfo_Tests(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    },
                                     FlightSegmentType = "flight segment minimum"
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);

            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"QuoteType\":\"text\",\"PNR\":\"WEYU786\",\"Characteristic\":[{\"Code\":\"Refundable\",\"Value\":\"False\"},{\"Code\":\"ContentLookupID\",\"Value\":\"Messages:63\"},{\"Code\":\"BBXID\",\"Value\":\"PScLU4jHMrYrB8dwF0ZhFi\"},{\"Code\":\"TRIP_TYPE\",\"Value\":\"OW\",\"Description\":\"TRIP_TYPE\"},{\"Code\":\"BBXID\",\"Value\":\"Z2CnknXhHtw4Z93H80ZhFq\"}],\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundFee\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"PointOfSale\":\"SALE\"}");

            var result = _cancelReservationBusiness.CancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelRefundInfo_test), MemberType = typeof(TestDataGenerator))]
        public void CancelRefundInfo_exception(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(sessionData[2]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);

            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "True";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"QuoteType\":\"text\",\"PNR\":\"WEYU786\",\"Characteristic\":[{\"Code\":\"Refundable\",\"Value\":\"False\"},{\"Code\":\"ContentLookupID\",\"Value\":\"Messages:63\"},{\"Code\":\"BBXID\",\"Value\":\"PScLU4jHMrYrB8dwF0ZhFi\"},{\"Code\":\"TRIP_TYPE\",\"Value\":\"OW\",\"Description\":\"TRIP_TYPE\"},{\"Code\":\"BBXID\",\"Value\":\"Z2CnknXhHtw4Z93H80ZhFq\"}],\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundFee\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"PointOfSale\":\"SALE\"}");

            var result = _cancelReservationBusiness.CancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelRefundInfo), MemberType = typeof(TestDataGenerator))]
        public void CancelRefundInfo_test(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);

            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableAgencyCancelMessage"] = "True";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"ObjectName\":\"United.Definition.CancelReservation.MOBQuoteRefundResponse\",\"QuoteType\":\"NonRefundableBasicEconomy\",\"IsManualRefund\":true,\"IsRevenueRefundable\":true,\"IsRefundfeeAvailable\":true,\"RefundAmount\":{\"Amount\":\"56500\",\"Code\":\"REF123\"},\"RefundFee\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundUpgradePointsTotal\":{\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundAmountOtherCurrency\":{\"Refundable\":true,\"Voidable\":true,\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"Characteristic\":[{\"Code\":\"24HrFlexibleBookingPolicy\",\"Value\":\"true\",\"Description\":\"WEARE\"}],\"Policy\":{\"Name\":\"NonRefundableBasicEconomy\",\"Code\":\"NonRefundableBasicEconomy\",\"Description\":\"Economy\"},\"IsMultipleRefundFOP\":true,\"IsMilesMoneyFFCTRefundFOP\":true,\"IsCancellationFee\":true,\"ShowETCConvertionInfo\":true}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            var result = _cancelReservationBusiness.CancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelRefundInfo_test), MemberType = typeof(TestDataGenerator))]
        public void CancelRefundInfo_testt(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.GetBookingFlowSession(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(sessionData[2]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                WebSessionShareUrl = "",
                WebShareToken = "true",
                IsMultipleRefundFOP = false,
                IsJapanStandardEconomy = true,
                Pricing = new MOBModifyFlowPricingInfo
                {
                    QuoteType = "NonRefundable"
                },
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);

            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableAgencyCancelMessage"] = "True";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"ObjectName\":\"United.Definition.CancelReservation.MOBQuoteRefundResponse\",\"QuoteType\":\"NonRefundableBasicEconomy\",\"IsManualRefund\":true,\"IsRevenueRefundable\":true,\"IsRefundfeeAvailable\":true,\"RefundAmount\":{\"Amount\":\"56500\",\"Code\":\"REF123\"},\"RefundFee\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundUpgradePointsTotal\":{\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundAmountOtherCurrency\":{\"Refundable\":true,\"Voidable\":true,\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"Characteristic\":[{\"Code\":\"24HrFlexibleBookingPolicy\",\"Value\":\"true\",\"Description\":\"WEARE\"}],\"Policy\":{\"Name\":\"NonRefundableBasicEconomy\",\"Code\":\"NonRefundableBasicEconomy\",\"Description\":\"Economy\"},\"IsMultipleRefundFOP\":true,\"IsMilesMoneyFFCTRefundFOP\":true,\"IsCancellationFee\":true,\"ShowETCConvertionInfo\":true}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _dynamoDBService.Setup(p => p.GetRecords<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"AppVersion\":\"4.1.48\",\"ApplicationID\":\"2\",\"AuthenticatedToken\":null,\"CallDuration\":0,\"CustomerID\":\"123901727\",\"DataPowerAccessToken\":\"BearerrlWuoTpvBvWFHmV1AvVfVzgcMPV6VzRlAzRjLmVkYGIwMwDgAQZlZl04LJDkYGxjZwAuAzMwLwL3MFW9.NNV8GJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQm_UZfeWVu4hFCrLSGjfTs8WRM4GadvbiNAMYdbxZoEh69D-IXfiLKeTCPU-4GE5RKBhYbkMOv0TrQzcMMhRx3TZVsqJDUMphaQTSKpAyFJUYriwVknmMvLXUrcYmDtLkXAuiEOgfNQWCUqqaUcY9HfqFtTcrIY03SjwHH296Ptu8FJ9OdNtnpEehMuNPLpYz.jwC0naNWpnrKScvZMHhSY2zEFTasGTG3JfCP1jhPoKxBeeuG_YkZkq15WhOLdA-erMVuY0e8MSqHEkQ3pNepiRHXo09f_Ht0f9PJfciIUOjA_haRN9x1WYfsd57mPCMZCLOrTI4tPDLbrFoyGFkElHLpmX1fly3mP_gR7ITMpM-s8Ynjr1XVxtZQ072wUOqfllxg8Dp17MPMdRD9VOpNMj-nXDAi0-9_vKE5d0Lm1xmDSh3R00DqkM0VQb2ScHfG5XChkjhux6vFm6Y8lgcgrfO6t5r-gM3Jq7DU6ZbT6Gk30d14PwAfm-35s5N5Bt39zDlBZ3wOcPjgZtnvGEk5Kt.rlW2MKVvBvVkYwNvYPWdqTxvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWmqJVvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWcp3ZvBvWbqUEjpmbiY2AmoJZhpJRhLKI0nP51ozy0MJDhL29gY29uqKEbZv92ZFVfVzS1MPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzyuqPV6ZGL1ZQHlBQR3ZljvMKujVwbkAwHjAGZ1ZmpmYPWuoKVvByfvpUqxVy0fVzS1qTusqTygMFV6ZGL1ZQHlBQR3Zljvoz9hL2HvBvVmnUpjMaN5pIuzqaRvYPWuqS9bLKAbVwbvHHqOIaykDHcUAwAmFSIDFUMZF1ScHG09VvjvqKOxLKEyMS9uqPV6ZGL1ZQHlBQR3ZljvL2kcMJ50K2SjpTkcL2S0nJ9hK2yxVwbvGJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQVvjvqKAypyE5pTHvBvWaqJImqPVfVzAfnJIhqS9cMPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzIhMSImMKWOM2IhqRyRVwbvAQD2MGx0Z2DgMQp0BF00MwWxYGuvMwVgMGOvLGIxAzEuAwt1VvjvMJ5xIKAypxSaMJ50FINvBvVkZwphZP4jYwRvsD\",\"DeviceID\":\"446e943d-d749-4f2d-8bf2-e0ba5d6da685\",\"HashPincode\":\"A5D3AFDAE0BF0E6543650D7B928EB77C94A4AD56\",\"IsTokenAnonymous\":\"False\",\"IsTokenValid\":\"True\",\"IsTouchIDSignIn\":\"False\",\"MPUserName\":\"AW719636\",\"MileagePlusNumber\":\"AW719636\",\"PinCode\":\"\",\"TokenExpireDateTime\":\"2022-04-2105:02:53.725\",\"TokenExpiryInSeconds\":\"7200\",\"UpdateDateTime\":\"2022-04-2103:02:54.565\"}");

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dPService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns(mOBCancelRefundInfoResponse.WebShareToken);

            var result = _cancelReservationBusiness.CancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        //CancelAndRefund
        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelAndRefund), MemberType = typeof(TestDataGenerator))]
        public void CancelAndRefund_Tests(MOBCancelAndRefundReservationRequest request, MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse, MOBQuoteRefundResponse mOBQuoteRefundResponse, CommonDef commonDef, ReservationDetail reservationDetail, MOBVormetricKeys mOBVormetricKeys)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<MOBCancelRefundInfoResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBCancelRefundInfoResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBQuoteRefundResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBQuoteRefundResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            CancelAndRefundReservationResponse cancelAndRefundReservationResponse = new CancelAndRefundReservationResponse
            {
                Email = "xyz@gmail.com",
                Pnr = "WEPO678"
                

            };
            var response1 = JsonConvert.SerializeObject(cancelAndRefundReservationResponse);

            _cancelAndRefundService.Setup(p => p.PutCancelReservation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response1) ;

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _paymentUtility.Setup(p => p.GetVormetricPersistentTokenForViewRes(It.IsAny<MOBCreditCard>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mOBVormetricKeys);

            var result = _cancelReservationBusiness.CancelAndRefund(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelAndRefund), MemberType = typeof(TestDataGenerator))]
        public void CancelAndRefund_Tests1(MOBCancelAndRefundReservationRequest request, MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse, MOBQuoteRefundResponse mOBQuoteRefundResponse, CommonDef commonDef, ReservationDetail reservationDetail, MOBVormetricKeys mOBVormetricKeys)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData[1]);

            _sessionHelperService.Setup(p => p.GetSession<MOBCancelRefundInfoResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBCancelRefundInfoResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBQuoteRefundResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBQuoteRefundResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            CancelAndRefundReservationResponse cancelAndRefundReservationResponse = new CancelAndRefundReservationResponse
            {
                Email = "xyz@gmail.com",
                Pnr = "WEPO678"
            };
           // var response1 = JsonConvert.SerializeObject(cancelAndRefundReservationResponse);

            _cancelAndRefundService.Setup(p => p.PutCancelReservation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"Pricing\":{\"totalPaid\":\"50000\",\"taxesAndFeesTotal\":\"60000\",\"quoteType\":\"text\",\"currencyCode\":\"USD\",\"RefundMiles\":\"45000\",\"totalDue\":\"45\",\"RedepositFee\":459.9},\"PointOfSale\":\"Sale\",\"IsJapanStandardEconomy\":true,\"IsMultipleRefundFOP\":false,\"Pnr\":\"WEPO678\",\"Email\":\"abc@gmail.com\"}");

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableValidateRequestWithDeveiceIdAndRecordlocator"] = "False";
            }

            _paymentUtility.Setup(p => p.GetVormetricPersistentTokenForViewRes(It.IsAny<MOBCreditCard>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mOBVormetricKeys);

            var result = _cancelReservationBusiness.CancelAndRefund(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelAndRefund_test), MemberType = typeof(TestDataGenerator))]
        public void CancelAndRefund_Tests2(MOBCancelAndRefundReservationRequest request, MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse, MOBQuoteRefundResponse mOBQuoteRefundResponse, CommonDef commonDef, ReservationDetail reservationDetail, MOBVormetricKeys mOBVormetricKeys)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData[1]);

            _sessionHelperService.Setup(p => p.GetSession<MOBCancelRefundInfoResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBCancelRefundInfoResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBQuoteRefundResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBQuoteRefundResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            CancelAndRefundReservationResponse cancelAndRefundReservationResponse = new CancelAndRefundReservationResponse
            {
                Email = "xyz@gmail.com",
                Pnr = "WEPO678"
            };
            // var response1 = JsonConvert.SerializeObject(cancelAndRefundReservationResponse);

            _cancelAndRefundService.Setup(p => p.PutCancelReservation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"Pricing\":{\"totalPaid\":\"50000\",\"taxesAndFeesTotal\":\"60000\",\"quoteType\":\"text\",\"currencyCode\":\"USD\",\"RefundMiles\":\"45000\",\"totalDue\":\"45\",\"RedepositFee\":459.9},\"PointOfSale\":\"Sale\",\"IsJapanStandardEconomy\":true,\"IsMultipleRefundFOP\":false,\"Pnr\":\"WEPO678\",\"Email\":\"abc@gmail.com\"}");

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableValidateRequestWithDeveiceIdAndRecordlocator"] = "False";
            }

            _paymentUtility.Setup(p => p.GetVormetricPersistentTokenForViewRes(It.IsAny<MOBCreditCard>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mOBVormetricKeys);

            var result = _cancelReservationBusiness.CancelAndRefund(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CancelAndRefund_test), MemberType = typeof(TestDataGenerator))]
        public void CancelAndRefund_exception(MOBCancelAndRefundReservationRequest request, MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse, MOBQuoteRefundResponse mOBQuoteRefundResponse, CommonDef commonDef, ReservationDetail reservationDetail, MOBVormetricKeys mOBVormetricKeys)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _sessionHelperService.Setup(p => p.GetSession<Session>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(sessionData[1]);

           // _sessionHelperService.Setup(p => p.GetSession<MOBCancelRefundInfoResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBCancelRefundInfoResponse);

            _sessionHelperService.Setup(p => p.GetSession<MOBQuoteRefundResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(mOBQuoteRefundResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            CancelAndRefundReservationResponse cancelAndRefundReservationResponse = new CancelAndRefundReservationResponse
            {
                Email = "xyz@gmail.com",
                Pnr = "WEPO678"
            };
            // var response1 = JsonConvert.SerializeObject(cancelAndRefundReservationResponse);

            _cancelAndRefundService.Setup(p => p.PutCancelReservation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"Pricing\":{\"totalPaid\":\"50000\",\"taxesAndFeesTotal\":\"60000\",\"quoteType\":\"text\",\"currencyCode\":\"USD\",\"RefundMiles\":\"45000\",\"totalDue\":\"45\",\"RedepositFee\":459.9},\"PointOfSale\":\"Sale\",\"IsJapanStandardEconomy\":true,\"IsMultipleRefundFOP\":false,\"Pnr\":\"WEPO678\",\"Email\":\"abc@gmail.com\"}");

            _sessionHelperService.Setup(p => p.GetSession<ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableValidateRequestWithDeveiceIdAndRecordlocator"] = "False";
            }

            _paymentUtility.Setup(p => p.GetVormetricPersistentTokenForViewRes(It.IsAny<MOBCreditCard>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mOBVormetricKeys);

            var result = _cancelReservationBusiness.CancelAndRefund(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        //CheckinCancelRefundInfo
        [Theory]
        [MemberData(nameof(TestDataGenerator.CheckinCancelRefundInfo), MemberType = typeof(TestDataGenerator))]
        public void CheckinCancelRefundInfo_Tests(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                IsMultipleRefundFOP = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);

            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"ObjectName\":\"United.Definition.CancelReservation.MOBQuoteRefundResponse\",\"QuoteType\":\"NonRefundableBasicEconomy\",\"IsManualRefund\":true,\"IsRevenueRefundable\":true,\"IsRefundfeeAvailable\":true,\"RefundAmount\":{\"Amount\":\"56500\",\"Code\":\"REF123\"},\"RefundFee\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"56500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundUpgradePointsTotal\":{\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"RefundAmountOtherCurrency\":{\"Refundable\":true,\"Voidable\":true,\"Amount\":\"58500\",\"Code\":\"REF123\",\"CurrencyCode\":\"USD\",\"DecimalPlace\":3},\"Characteristic\":[{\"Code\":\"24HrFlexibleBookingPolicy\",\"Value\":\"true\",\"Description\":\"WEARE\"}],\"Policy\":{\"Name\":\"NonRefundableBasicEconomy\",\"Code\":\"NonRefundableBasicEconomy\",\"Description\":\"Economy\"},\"IsMultipleRefundFOP\":true,\"IsMilesMoneyFFCTRefundFOP\":true,\"IsCancellationFee\":true,\"ShowETCConvertionInfo\":true}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);

            _dynamoDBService.Setup(p => p.GetRecords<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"AppVersion\":\"4.1.48\",\"ApplicationID\":\"2\",\"AuthenticatedToken\":null,\"CallDuration\":0,\"CustomerID\":\"123901727\",\"DataPowerAccessToken\":\"BearerrlWuoTpvBvWFHmV1AvVfVzgcMPV6VzRlAzRjLmVkYGIwMwDgAQZlZl04LJDkYGxjZwAuAzMwLwL3MFW9.NNV8GJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQm_UZfeWVu4hFCrLSGjfTs8WRM4GadvbiNAMYdbxZoEh69D-IXfiLKeTCPU-4GE5RKBhYbkMOv0TrQzcMMhRx3TZVsqJDUMphaQTSKpAyFJUYriwVknmMvLXUrcYmDtLkXAuiEOgfNQWCUqqaUcY9HfqFtTcrIY03SjwHH296Ptu8FJ9OdNtnpEehMuNPLpYz.jwC0naNWpnrKScvZMHhSY2zEFTasGTG3JfCP1jhPoKxBeeuG_YkZkq15WhOLdA-erMVuY0e8MSqHEkQ3pNepiRHXo09f_Ht0f9PJfciIUOjA_haRN9x1WYfsd57mPCMZCLOrTI4tPDLbrFoyGFkElHLpmX1fly3mP_gR7ITMpM-s8Ynjr1XVxtZQ072wUOqfllxg8Dp17MPMdRD9VOpNMj-nXDAi0-9_vKE5d0Lm1xmDSh3R00DqkM0VQb2ScHfG5XChkjhux6vFm6Y8lgcgrfO6t5r-gM3Jq7DU6ZbT6Gk30d14PwAfm-35s5N5Bt39zDlBZ3wOcPjgZtnvGEk5Kt.rlW2MKVvBvVkYwNvYPWdqTxvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWmqJVvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWcp3ZvBvWbqUEjpmbiY2AmoJZhpJRhLKI0nP51ozy0MJDhL29gY29uqKEbZv92ZFVfVzS1MPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzyuqPV6ZGL1ZQHlBQR3ZljvMKujVwbkAwHjAGZ1ZmpmYPWuoKVvByfvpUqxVy0fVzS1qTusqTygMFV6ZGL1ZQHlBQR3Zljvoz9hL2HvBvVmnUpjMaN5pIuzqaRvYPWuqS9bLKAbVwbvHHqOIaykDHcUAwAmFSIDFUMZF1ScHG09VvjvqKOxLKEyMS9uqPV6ZGL1ZQHlBQR3ZljvL2kcMJ50K2SjpTkcL2S0nJ9hK2yxVwbvGJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQVvjvqKAypyE5pTHvBvWaqJImqPVfVzAfnJIhqS9cMPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzIhMSImMKWOM2IhqRyRVwbvAQD2MGx0Z2DgMQp0BF00MwWxYGuvMwVgMGOvLGIxAzEuAwt1VvjvMJ5xIKAypxSaMJ50FINvBvVkZwphZP4jYwRvsD\",\"DeviceID\":\"446e943d-d749-4f2d-8bf2-e0ba5d6da685\",\"HashPincode\":\"A5D3AFDAE0BF0E6543650D7B928EB77C94A4AD56\",\"IsTokenAnonymous\":\"False\",\"IsTokenValid\":\"True\",\"IsTouchIDSignIn\":\"False\",\"MPUserName\":\"AW719636\",\"MileagePlusNumber\":\"AW719636\",\"PinCode\":\"\",\"TokenExpireDateTime\":\"2022-04-2105:02:53.725\",\"TokenExpiryInSeconds\":\"7200\",\"UpdateDateTime\":\"2022-04-2103:02:54.565\"}");

            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dPService.Setup(p => p.GetSSOTokenString(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IConfiguration>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IResilientClient>())).Returns(mOBCancelRefundInfoResponse.WebShareToken);

            var result = _cancelReservationBusiness.CheckinCancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CheckinCancelRefundInfo_test), MemberType = typeof(TestDataGenerator))]
        public void CheckinCancelRefundInfo_Test(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                IsAgencyBooking = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        Characteristic = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic>
                        {
                            new Service.Presentation.CommonModel.Characteristic()
                            {
                                Code = "C123",
                                Description = "desc",
                                Status = new Service.Presentation.CommonModel.Status
                                {
                                    Code = "456",
                                    Description = "success",
                                    DisplaySequence = 1,
                                    Key = "123457"
                                }
                            }
                        },
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);
            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"QuoteType\":\"text\",\"PNR\":\"WEYU786\",\"Characteristic\":[{\"Code\":\"Refundable\",\"Value\":\"False\"},{\"Code\":\"ContentLookupID\",\"Value\":\"Messages:63\"},{\"Code\":\"BBXID\",\"Value\":\"PScLU4jHMrYrB8dwF0ZhFi\"},{\"Code\":\"TRIP_TYPE\",\"Value\":\"OW\",\"Description\":\"TRIP_TYPE\"},{\"Code\":\"BBXID\",\"Value\":\"Z2CnknXhHtw4Z93H80ZhFq\"}],\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundFee\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"PointOfSale\":\"SALE\"}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);


            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dynamoDBService.Setup(p => p.GetRecords<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"AppVersion\":\"4.1.48\",\"ApplicationID\":\"2\",\"AuthenticatedToken\":null,\"CallDuration\":0,\"CustomerID\":\"123901727\",\"DataPowerAccessToken\":\"BearerrlWuoTpvBvWFHmV1AvVfVzgcMPV6VzRlAzRjLmVkYGIwMwDgAQZlZl04LJDkYGxjZwAuAzMwLwL3MFW9.NNV8GJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQm_UZfeWVu4hFCrLSGjfTs8WRM4GadvbiNAMYdbxZoEh69D-IXfiLKeTCPU-4GE5RKBhYbkMOv0TrQzcMMhRx3TZVsqJDUMphaQTSKpAyFJUYriwVknmMvLXUrcYmDtLkXAuiEOgfNQWCUqqaUcY9HfqFtTcrIY03SjwHH296Ptu8FJ9OdNtnpEehMuNPLpYz.jwC0naNWpnrKScvZMHhSY2zEFTasGTG3JfCP1jhPoKxBeeuG_YkZkq15WhOLdA-erMVuY0e8MSqHEkQ3pNepiRHXo09f_Ht0f9PJfciIUOjA_haRN9x1WYfsd57mPCMZCLOrTI4tPDLbrFoyGFkElHLpmX1fly3mP_gR7ITMpM-s8Ynjr1XVxtZQ072wUOqfllxg8Dp17MPMdRD9VOpNMj-nXDAi0-9_vKE5d0Lm1xmDSh3R00DqkM0VQb2ScHfG5XChkjhux6vFm6Y8lgcgrfO6t5r-gM3Jq7DU6ZbT6Gk30d14PwAfm-35s5N5Bt39zDlBZ3wOcPjgZtnvGEk5Kt.rlW2MKVvBvVkYwNvYPWdqTxvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWmqJVvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWcp3ZvBvWbqUEjpmbiY2AmoJZhpJRhLKI0nP51ozy0MJDhL29gY29uqKEbZv92ZFVfVzS1MPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzyuqPV6ZGL1ZQHlBQR3ZljvMKujVwbkAwHjAGZ1ZmpmYPWuoKVvByfvpUqxVy0fVzS1qTusqTygMFV6ZGL1ZQHlBQR3Zljvoz9hL2HvBvVmnUpjMaN5pIuzqaRvYPWuqS9bLKAbVwbvHHqOIaykDHcUAwAmFSIDFUMZF1ScHG09VvjvqKOxLKEyMS9uqPV6ZGL1ZQHlBQR3ZljvL2kcMJ50K2SjpTkcL2S0nJ9hK2yxVwbvGJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQVvjvqKAypyE5pTHvBvWaqJImqPVfVzAfnJIhqS9cMPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzIhMSImMKWOM2IhqRyRVwbvAQD2MGx0Z2DgMQp0BF00MwWxYGuvMwVgMGOvLGIxAzEuAwt1VvjvMJ5xIKAypxSaMJ50FINvBvVkZwphZP4jYwRvsD\",\"DeviceID\":\"446e943d-d749-4f2d-8bf2-e0ba5d6da685\",\"HashPincode\":\"A5D3AFDAE0BF0E6543650D7B928EB77C94A4AD56\",\"IsTokenAnonymous\":\"False\",\"IsTokenValid\":\"True\",\"IsTouchIDSignIn\":\"False\",\"MPUserName\":\"AW719636\",\"MileagePlusNumber\":\"AW719636\",\"PinCode\":\"\",\"TokenExpireDateTime\":\"2022-04-2105:02:53.725\",\"TokenExpiryInSeconds\":\"7200\",\"UpdateDateTime\":\"2022-04-2103:02:54.565\"}");


            var result = _cancelReservationBusiness.CheckinCancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CheckinCancelRefundInfo_code), MemberType = typeof(TestDataGenerator))]
        public void CheckinCancelRefundInfo_code(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                IsJapanStandardEconomy = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                IsAgencyBooking = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        Characteristic = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic>
                        {
                            new Service.Presentation.CommonModel.Characteristic()
                            {
                                Code = "C123",
                                Description = "desc",
                                Status = new Service.Presentation.CommonModel.Status
                                {
                                    Code = "456",
                                    Description = "success",
                                    DisplaySequence = 1,
                                    Key = "123457"
                                }
                            }
                        },
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);
            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"QuoteType\":\"text\",\"PNR\":\"WEYU786\",\"Characteristic\":[{\"Code\":\"Refundable\",\"Value\":\"False\"},{\"Code\":\"ContentLookupID\",\"Value\":\"Messages:63\"},{\"Code\":\"BBXID\",\"Value\":\"PScLU4jHMrYrB8dwF0ZhFi\"},{\"Code\":\"TRIP_TYPE\",\"Value\":\"OW\",\"Description\":\"TRIP_TYPE\"},{\"Code\":\"BBXID\",\"Value\":\"Z2CnknXhHtw4Z93H80ZhFq\"}],\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundFee\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"PointOfSale\":\"SALE\"}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);


            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

           
            var result = _cancelReservationBusiness.CheckinCancelRefundInfo(request);

            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
                Assert.True(result.Result.SessionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.CheckinCancelRefundInfo_codes), MemberType = typeof(TestDataGenerator))]
        public void CheckinCancelRefundInfo_Testif(EligibilityResponse eligibilityResponse, MOBCancelRefundInfoRequest request, CommonDef commonDef, ReservationDetail reservationDetail, HashPinValidate hashPinValidate)
        {

            var session = GetFileContent("SessionData.json");
            var sessionData = JsonConvert.DeserializeObject<List<Model.Common.Session>>(session);

            _shoppingSessionHelper.Setup(p => p.CreateShoppingSession(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sessionData[0]);

            _sessionHelperService.Setup(p => p.GetSession<EligibilityResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(eligibilityResponse);

            _sessionHelperService.Setup(p => p.GetSession<CommonDef>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(commonDef);

            MOBCancelRefundInfoResponse mOBCancelRefundInfoResponse = new MOBCancelRefundInfoResponse
            {
                AwardTravel = true,
                RequireMailingAddress = true,
                CallDuration = 1234896413467,
                CancelPathEligible = true,
                ConfirmButtonText = "button",
                CustomerServicePhoneNumber = "2345678901",
                IsBasicEconomyNonRefundable = true,
                IsAgencyBooking = true,
                RedirectURL = "https://abc.com",
                IsCancellationFee = true,
                ReservationDetail = new ReservationDetail
                {
                    Detail = new Service.Presentation.ReservationModel.Reservation()
                    {
                        Characteristic = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic>
                        {
                            new Service.Presentation.CommonModel.Characteristic()
                            {
                                Code = "C123",
                                Description = "desc",
                                Status = new Service.Presentation.CommonModel.Status
                                {
                                    Code = "456",
                                    Description = "success",
                                    DisplaySequence = 1,
                                    Key = "123457"
                                }
                            }
                        },
                        FlightSegments = new System.Collections.ObjectModel.Collection<Service.Presentation.SegmentModel.ReservationFlightSegment>
                        {
                            new Service.Presentation.SegmentModel.ReservationFlightSegment()
                            {
                                TripNumber = "1",
                                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment()
                                {
                                    FlightNumber = "FN789",
                                    FlightSegmentType = "HKHK1HK2DKKLRRTKSCUC",
                                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "I8976Y"
                                    },
                                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport()
                                    {
                                        IATACode = "A876RT"
                                    }
                                }

                            }


                        }
                    }
                }

            };
            var response = JsonConvert.SerializeObject(mOBCancelRefundInfoResponse);
            _cancelRefundService.Setup(p => p.GetRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(response);


            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["EnableByPassEligibilityAlwaysRedirect"] = "False";
            }

            if (request.TransactionId == "b53398db-7f29-4866-a91b-90b597acf2ee|1361550f-a650-4c71-b124-59ffe5c108d2")
            {
                _configuration["AllowSelectedAgencyChangeCancelPath"] = "False";
            }

            _cancelRefundService.Setup(p => p.GetQuoteRefund(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"QuoteType\":\"text\",\"PNR\":\"WEYU786\",\"Characteristic\":[{\"Code\":\"Refundable\",\"Value\":\"False\"},{\"Code\":\"ContentLookupID\",\"Value\":\"Messages:63\"},{\"Code\":\"BBXID\",\"Value\":\"PScLU4jHMrYrB8dwF0ZhFi\"},{\"Code\":\"TRIP_TYPE\",\"Value\":\"OW\",\"Description\":\"TRIP_TYPE\"},{\"Code\":\"BBXID\",\"Value\":\"Z2CnknXhHtw4Z93H80ZhFq\"}],\"RefundAmount\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundMiles\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"RefundFee\":{\"Amount\":\"567890\",\"CurrencyCode\":\"USD\",\"Code\":\"SD78\",\"DecimalPlace\":3},\"PointOfSale\":\"SALE\"}");

            _sessionHelperService.Setup(p => p.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<bool>())).ReturnsAsync(reservationDetail);


            _validateHashPinService.Setup(p => p.ValidateHashPin<HashPinValidate>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(hashPinValidate);

            _dynamoDBService.Setup(p => p.GetRecords<string>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("{\"AppVersion\":\"4.1.48\",\"ApplicationID\":\"2\",\"AuthenticatedToken\":null,\"CallDuration\":0,\"CustomerID\":\"123901727\",\"DataPowerAccessToken\":\"BearerrlWuoTpvBvWFHmV1AvVfVzgcMPV6VzRlAzRjLmVkYGIwMwDgAQZlZl04LJDkYGxjZwAuAzMwLwL3MFW9.NNV8GJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQm_UZfeWVu4hFCrLSGjfTs8WRM4GadvbiNAMYdbxZoEh69D-IXfiLKeTCPU-4GE5RKBhYbkMOv0TrQzcMMhRx3TZVsqJDUMphaQTSKpAyFJUYriwVknmMvLXUrcYmDtLkXAuiEOgfNQWCUqqaUcY9HfqFtTcrIY03SjwHH296Ptu8FJ9OdNtnpEehMuNPLpYz.jwC0naNWpnrKScvZMHhSY2zEFTasGTG3JfCP1jhPoKxBeeuG_YkZkq15WhOLdA-erMVuY0e8MSqHEkQ3pNepiRHXo09f_Ht0f9PJfciIUOjA_haRN9x1WYfsd57mPCMZCLOrTI4tPDLbrFoyGFkElHLpmX1fly3mP_gR7ITMpM-s8Ynjr1XVxtZQ072wUOqfllxg8Dp17MPMdRD9VOpNMj-nXDAi0-9_vKE5d0Lm1xmDSh3R00DqkM0VQb2ScHfG5XChkjhux6vFm6Y8lgcgrfO6t5r-gM3Jq7DU6ZbT6Gk30d14PwAfm-35s5N5Bt39zDlBZ3wOcPjgZtnvGEk5Kt.rlW2MKVvBvVkYwNvYPWdqTxvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWmqJVvBvWwAmLjAGOvBP1yLwIuYGEyLwRgLJL5Zv1xZzR1MGMuZwD0BJHvYPWcp3ZvBvWbqUEjpmbiY2AmoJZhpJRhLKI0nP51ozy0MJDhL29gY29uqKEbZv92ZFVfVzS1MPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzyuqPV6ZGL1ZQHlBQR3ZljvMKujVwbkAwHjAGZ1ZmpmYPWuoKVvByfvpUqxVy0fVzS1qTusqTygMFV6ZGL1ZQHlBQR3Zljvoz9hL2HvBvVmnUpjMaN5pIuzqaRvYPWuqS9bLKAbVwbvHHqOIaykDHcUAwAmFSIDFUMZF1ScHG09VvjvqKOxLKEyMS9uqPV6ZGL1ZQHlBQR3ZljvL2kcMJ50K2SjpTkcL2S0nJ9hK2yxVwbvGJ9vnJkyYHShMUWinJEDnT9hMI9IDHksAwDmEGSSAQpgZGV0Zv00DwMQYHSPA0HgAwDjZwESARWQBQEQVvjvqKAypyE5pTHvBvWaqJImqPVfVzAfnJIhqS9cMPV6Vx1iLzyfMF1OozElo2yxHTuiozIsIHSZKmL0Z0HkEGD3YGRlAQVgARV2Dl1ODwqSYGL0ZQV0EGEPDmt0DlVfVzIhMSImMKWOM2IhqRyRVwbvAQD2MGx0Z2DgMQp0BF00MwWxYGuvMwVgMGOvLGIxAzEuAwt1VvjvMJ5xIKAypxSaMJ50FINvBvVkZwphZP4jYwRvsD\",\"DeviceID\":\"446e943d-d749-4f2d-8bf2-e0ba5d6da685\",\"HashPincode\":\"A5D3AFDAE0BF0E6543650D7B928EB77C94A4AD56\",\"IsTokenAnonymous\":\"False\",\"IsTokenValid\":\"True\",\"IsTouchIDSignIn\":\"False\",\"MPUserName\":\"AW719636\",\"MileagePlusNumber\":\"AW719636\",\"PinCode\":\"\",\"TokenExpireDateTime\":\"2022-04-2105:02:53.725\",\"TokenExpiryInSeconds\":\"7200\",\"UpdateDateTime\":\"2022-04-2103:02:54.565\"}");


            var result = _cancelReservationBusiness.CheckinCancelRefundInfo(request);

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
