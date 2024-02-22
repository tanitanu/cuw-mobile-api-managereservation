using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Profile;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Profile;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Payment;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.InteractionModel;
using United.Service.Presentation.PaymentModel;
using United.Service.Presentation.PaymentRequestModel;
using United.Service.Presentation.PaymentResponseModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using Address = United.Service.Presentation.CommonModel.Address;
using CslDataVaultResponse = United.Service.Presentation.PaymentResponseModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using FormofPaymentOption = United.Mobile.Model.Common.Shopping.FormofPaymentOption;
//using FormofPaymentOption = United.Mobile.Model.Common.Shopping;
using Genre = United.Service.Presentation.CommonModel.Genre;
using MOBFormofPaymentDetails = United.Mobile.Model.Shopping.FormofPayment.MOBFormofPaymentDetails;
using MOBShoppingCart = United.Mobile.Model.Shopping.MOBShoppingCart;
using Product = United.Service.Presentation.ProductModel.Product;
using ProfileFOPCreditCardResponse = United.Mobile.Model.Shopping.ProfileFOPCreditCardResponse;
using ProfileResponse = United.Mobile.Model.Shopping.ProfileResponse;
using Reservation = United.Mobile.Model.Shopping.Reservation;

namespace United.Common.Helper.FOP
{
    public class FormsOfPayment : IFormsOfPayment
    {
        private readonly ICacheLog<FormsOfPayment> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentUtility _paymentUtility;
        private readonly IHeaders _headers;

        public FormsOfPayment(ICacheLog<FormsOfPayment> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IShoppingCartService shoppingCartService
            , IPaymentService paymentService
            , IPaymentUtility paymentUtility
            , IHeaders headers)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _shoppingCartService = shoppingCartService;
            _paymentService = paymentService;
            _paymentUtility = paymentUtility;
            _headers = headers;
            ConfigUtility.UtilityInitialize(_configuration);
        }

        public async Task<(List<FormofPaymentOption> response, bool isDefault)> EligibleFormOfPayments(FOPEligibilityRequest request, Session session, bool isDefault, bool IsMilesFOPEnabled = false, List<LogEntry> eligibleFoplogEntries = null)
        {
            var response = new List<FormofPaymentOption>();

            try
            {
                string requestXml = string.Empty;
                if (!string.IsNullOrEmpty(request.Flow) && request.Flow == "RESHOP")
                {
                    request.Flow = "EXCHANGE";
                }
                ShoppingCart shoppingCart = await BuildEligibleFormOfPaymentsRequest(request, session);
                bool isMigrateToJSONService = _configuration.GetValue<bool>("EligibleFopMigrateToJSonService");
                var xmlRequest = isMigrateToJSONService
                                 ? JsonConvert.SerializeObject(shoppingCart)
                                 : XmlSerializerHelper.Serialize<ShoppingCart>(shoppingCart);

                string path = "/FormOfPayment/EligibleFormOfPaymentByShoppingCart";
                string token = session.Token;
                string xmlResponse = await _paymentService.GetEligibleFormOfPayments(token, path, xmlRequest, _headers.ContextValues.SessionId).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(xmlResponse))
                {
                    Service.Presentation.PaymentResponseModel.EligibleFormOfPayment eligibleFormOfPayment = isMigrateToJSONService
                                                                                                            ? DataContextJsonSerializer.DeserializeUseContract<Service.Presentation.PaymentResponseModel.EligibleFormOfPayment>(xmlResponse)
                                                                                                            : XmlSerializerHelper.Deserialize<Service.Presentation.PaymentResponseModel.EligibleFormOfPayment>(xmlResponse);

                    if ((request.Products != null) && (request.Products.Count == 1) && (ConfigUtility.IsETCchangesEnabled(request.Application.Id, request.Application.Version.Major) ? request.Flow != FlowType.BOOKING.ToString() : true))
                    {
                        response = eligibleFormOfPayment.ProductFormOfPayment[0].FormsOfPayment.GroupBy(p => p.Payment.Type.Key).Select(x => x.FirstOrDefault()).Select(x => new FormofPaymentOption { Category = x.Payment.Category, FullName = (x.Payment.Category == "CC") ? "Credit Card" : x.Payment.Type.Description, Code = x.Payment.Type.Key, FoPDescription = x.Payment.Type.Description }).ToList();
                    }
                    else
                    {
                        response = eligibleFormOfPayment.FormsOfPayment.GroupBy(p => p.Payment.Type.Key).Select(x => x.FirstOrDefault()).Select(x => new FormofPaymentOption { Category = x.Payment.Category, FullName = (x.Payment.Category == "CC") ? "Credit Card" : x.Payment.Type.Description, Code = x.Payment.Type.Key, FoPDescription = x.Payment.Type.Description }).ToList();
                    }

                    isDefault = false;
                    if (IsMilesFOPEnabled && ConfigUtility.IsMilesFOPEnabled())
                    {
                        response.Insert(1, new FormofPaymentOption { Category = "MILES", FullName = "Use miles", Code = null, FoPDescription = null });
                    }

                    //If Payment service enables ETC fop for SEATS and PCU it will automatically shows ETC as FOP .So,disabling etc for lower versions .
                    if (request.Flow == FlowType.VIEWRES.ToString() && !GeneralHelper.IsApplicationVersionGreaterorEqual(request.Application.Id, request.Application.Version.Major, _configuration.GetValue<string>("Android_EnableETCManageRes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCManageRes_AppVersion"))
                    || (_configuration.GetValue<bool>("LoadVIewResVormetricForVIEWRES_SEATMAPFlowToggle") && request.Flow == FlowType.VIEWRES_SEATMAP.ToString()))
                    {
                        if (response.Exists(x => x.Category == "CERT"))
                        {
                            response = response.Where(x => x.Category != "CERT").ToList();
                        }
                    }

                    if ((request.Flow == FlowType.BOOKING.ToString() && !ConfigUtility.IncludeMoneyPlusMiles(request.Application.Id, request.Application.Version.Major)) || session.IsAward || session.IsCorporateBooking
                        || (!_configuration.GetValue<bool>("DisableFixForMMCATripTipSeatChange_MOBILE17339") && request.Flow == FlowType.VIEWRES_SEATMAP.ToString()))
                    {
                        if (response.Exists(x => x.Category == "MILES"))
                        {
                            response = response.Where(x => x.Category != "MILES").ToList();
                        }
                    }
                    if (!_paymentUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major) ||
                        request.Flow != FlowType.BOOKING.ToString() ||
                        session.IsAward ||
                        (await _paymentUtility.GetTravelBankBalance(session.SessionId)) == 0.00)
                    {
                        if (response.Exists(x => x.Category == "TRAVELBANK"))
                        {
                            response = response.Where(x => x.Category != "TRAVELBANK").ToList();
                        }
                    }

                    if (request.Flow == FlowType.BOOKING.ToString()
                        && _paymentUtility.IncludeTravelCredit(request.Application.Id, request.Application.Version.Major)
                        && !session.IsAward && !session.IsCorporateBooking
                        && response.Exists(x => x.Category == "CERT"))
                    {
                        response.Add(
                            new FormofPaymentOption
                            {
                                Category = "TRAVELCREDIT",
                                Code = "TC",
                                DeleteOrder = false,
                                FoPDescription = "Travel Credit",
                                FullName = "Travel Credit"
                            });
                        response.RemoveAll(x => x.Category == "CERT");
                    }
                }
            }
            catch (System.Net.WebException wex)
            {
                if (wex.Response != null)
                {
                    _logger.LogError("EligibleFormOfPayments - Exception {ErrorMessageResponse} and {SessionId}", wex.Response, request.SessionId);

                    response.Add(new FormofPaymentOption { Category = "CC", FullName = "Credit Card", Code = null, FoPDescription = null });
                    response.Add(new FormofPaymentOption { Category = "PP", FullName = "PayPal", Code = null, FoPDescription = null });
                    response.Add(new FormofPaymentOption { Category = "PPC", FullName = "PayPal Credit", Code = null, FoPDescription = null });
                    response.Add(new FormofPaymentOption { Category = "MPS", FullName = "Masterpass", Code = null, FoPDescription = null });
                    response.Add(new FormofPaymentOption { Category = "APP", FullName = "Apple Pay", Code = null, FoPDescription = null });
                    isDefault = true;
                    if (IsMilesFOPEnabled && ConfigUtility.IsMilesFOPEnabled())
                    {
                        response.Insert(1, new FormofPaymentOption { Category = "MILES", FullName = "Use miles", Code = null, FoPDescription = null });
                    }
                }
            }
            catch (MOBUnitedException coex)
            {
                response = null;
                _logger.LogWarning("EligibleFormOfPayments - Exception {@UnitedException}", JsonConvert.SerializeObject(coex));
            }
            catch (System.Exception ex)
            {
                response = null;
                ExceptionWrapper exceptionWrapper = new ExceptionWrapper(ex);
                _logger.LogError("EligibleFormOfPayments - Exception {@Exception}", JsonConvert.SerializeObject(exceptionWrapper));
            }

            return (response, isDefault);
        }

        private async Task<LoadReservationAndDisplayCartResponse> GetCartInformation(string sessionId, Mobile.Model.MOBApplication application, string device, string cartId, string token, WorkFlowType workFlowType = WorkFlowType.InitialBooking)
        {
            LoadReservationAndDisplayCartRequest loadReservationAndDisplayCartRequest = new LoadReservationAndDisplayCartRequest();
            LoadReservationAndDisplayCartResponse loadReservationAndDisplayResponse = new LoadReservationAndDisplayCartResponse();
            loadReservationAndDisplayCartRequest.CartId = cartId;
            loadReservationAndDisplayCartRequest.WorkFlowType = workFlowType;
            string jsonRequest = JsonConvert.SerializeObject(loadReservationAndDisplayCartRequest);

            loadReservationAndDisplayResponse = await _shoppingCartService.GetCartInformation<LoadReservationAndDisplayCartResponse>(token, "LoadReservationAndDisplayCart", jsonRequest, _headers.ContextValues.SessionId).ConfigureAwait(false);

            return loadReservationAndDisplayResponse;
        }

        private async Task<ShoppingCart> BuildEligibleFormOfPaymentsRequest(FOPEligibilityRequest request, Session session)
        {
            string channel = (request.Application.Id == 1 ? "MOBILE-IOS" : "MOBILE-Android");
            string productContext = string.Format
                (@" <Reservation xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/United.Service.Presentation.ReservationModel"">            <Channel>{0}</Channel>            <Type xmlns:d2p1=""http://schemas.datacontract.org/2004/07/United.Service.Presentation.CommonModel"">            <d2p1:Genre>            <d2p1:Key>{1}</d2p1:Key>            </d2p1:Genre>            </Type>            </Reservation> ", channel, request.Flow);
            ShoppingCart shoppingCart = new ShoppingCart
            {
                ID = new UniqueIdentifier
                {
                    ID = (request.CartId != null) ? request.CartId : Guid.NewGuid().ToString()
                },
                Items = new Collection<ShoppingCartItem>()
            };
            var shoppingCartItem = new ShoppingCartItem
            {
                Product = new Collection<Product>()
            };

            foreach (var product in request.Products)
            {
                Product productObj = new Product
                {
                    Code = (_configuration.GetValue<bool>("EnableFareLockPurchaseViewRes") && product.Code != null && product.Code.Equals("FLK_VIEWRES", StringComparison.OrdinalIgnoreCase)) ? "FLK" : product.Code,
                    Description = product.ProductDescription
                };
                shoppingCartItem.Product.Add(productObj);
            }

            if (ConfigUtility.IsETCchangesEnabled(request.Application.Id, request.Application.Version.Major) && request.Flow == FlowType.BOOKING.ToString()
                || (request.Flow == FlowType.VIEWRES.ToString() && ConfigUtility.IsManageResETCEnabled(request.Application.Id, request.Application.Version.Major)))
            {
                channel = (request.Application.Id == 1 ? _configuration.GetValue<string>("eligibleFopMobileioschannelname") : _configuration.GetValue<string>("eligibleFopMobileandriodchannelname"));
                string newProductContext = string.Empty;
                newProductContext = await BuildProductContextForEligibleFoprequest(request, channel, session);
                productContext = string.IsNullOrEmpty(newProductContext) ? productContext : newProductContext;
                //FareLock code should be FLK Instead of FareLock
                if (shoppingCartItem.Product.Exists(x => x.Code == "FareLock"))
                {
                    shoppingCartItem.Product.Where(x => x.Code == "FareLock").FirstOrDefault().Code = "FLK";
                }
            }
            shoppingCartItem.ProductContext = new Collection<string>
            {
                productContext
            };
            shoppingCart.Items.Add(shoppingCartItem);
            shoppingCart.PointOfSale = new Service.Presentation.CommonModel.PointOfSale
            {
                Country = new Service.Presentation.CommonModel.Country
                {
                    CountryCode = "US"
                }
            };

            return shoppingCart;
        }

        private string GetMobilePathforEligibleFop(string flow)
        {
            string mobilePath = string.Empty;
            switch (flow)
            {
                case "BOOKING":
                    mobilePath = "INITIAL";
                    break;
                case "RESHOP":
                    mobilePath = "EXCHANGE";
                    break;
                default:
                    return flow;
            }
            return mobilePath;
        }

        private async Task<string> BuildProductContextForEligibleFoprequest(FOPEligibilityRequest request, string channel, Session session)
        {
            var reservation = new Service.Presentation.ReservationModel.Reservation();
            string mobilePath = GetMobilePathforEligibleFop(request.Flow);
            reservation.Channel = channel;
            reservation.Type = new Collection<Genre>
            {
                new Genre { Key = mobilePath }
            };
            switch (request.Flow)
            {
                case "BOOKING":
                    Reservation persistedReservation = new Reservation();
                    persistedReservation = await _sessionHelperService.GetSession<Reservation>(_headers.ContextValues.SessionId, persistedReservation.ObjectName, new List<string> { _headers.ContextValues.SessionId, persistedReservation.ObjectName }).ConfigureAwait(false);
                    List<ReservationFlightSegment> segments = DataContextJsonSerializer.DeserializeUseContract<List<ReservationFlightSegment>>(persistedReservation.CSLReservationJSONFormat);
                    if (persistedReservation != null)
                    {
                        reservation.FlightSegments = segments.ToCollection();//ETC is offered for only united operated flights.Need to send the segment details with operating carrier code.
                        if (ConfigUtility.IncludeMoneyPlusMiles(request.Application.Id, request.Application.Version.Major))
                        {
                            reservation = await _sessionHelperService.GetSession<Service.Presentation.ReservationModel.Reservation>(_headers.ContextValues.SessionId, reservation.GetType().FullName, new List<string> { _headers.ContextValues.SessionId, reservation.GetType().FullName }).ConfigureAwait(false);

                            if (reservation == null)
                            {
                                var cartInfo = await GetCartInformation(request.SessionId, request.Application, request.DeviceId, request.CartId, session.Token);
                                reservation = cartInfo.Reservation;
                                await _sessionHelperService.SaveSession<Service.Presentation.ReservationModel.Reservation>(cartInfo.Reservation, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, typeof(Service.Presentation.ReservationModel.Reservation).FullName }, typeof(Service.Presentation.ReservationModel.Reservation).FullName).ConfigureAwait(false);
                            }

                            reservation.Channel = channel;
                            reservation.Type = new Collection<Genre>
                            {
                                new Genre { Key = mobilePath }
                            };
                        }
                    }
                    return ProviderHelper.SerializeXml(reservation);
                case "VIEWRES":
                    ReservationDetail reservationDetail = new ReservationDetail();
                    reservationDetail = await _sessionHelperService.GetSession<ReservationDetail>(_headers.ContextValues.SessionId, (new ReservationDetail()).GetType().FullName, new List<string>() { _headers.ContextValues.SessionId, (new ReservationDetail()).GetType().FullName }).ConfigureAwait(false);

                    if (reservationDetail != null)
                    {

                        reservation.FlightSegments = reservationDetail.Detail.FlightSegments;//ETC is offered for only united operated flights.Need to send the segment details with operating carrier code.
                    }
                    return ProviderHelper.SerializeXml(reservation);
                default: return string.Empty;
            }
        }

        public string GetPaymentTargetForRegisterFop(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isCompleteFarelockPurchase = false)
        {
            if (string.IsNullOrEmpty(_configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes")))
            {
                return string.Empty;
            }

            if (isCompleteFarelockPurchase)
            {
                return "RES";
            }

            if (flightReservationResponse == null || flightReservationResponse.ShoppingCart == null || flightReservationResponse.ShoppingCart.Items == null)
            {
                return string.Empty;
            }

            var productCodes = flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES").Select(x => x.Product.FirstOrDefault().Code).ToList();
            if (productCodes == null || !productCodes.Any())
            {
                return string.Empty;
            }

            return string.Join(",", productCodes.Distinct());
        }

        public async Task<(List<FormofPaymentOption> response, bool isDefault)> GetEligibleFormofPayments(MOBRequest request, Session session, MOBShoppingCart shoppingCart, string cartId, string flow, bool isDefault, MOBSHOPReservation reservation = null, bool IsMilesFOPEnabled = false, SeatChangeState persistedState = null)
        {
            List<FormofPaymentOption> response = new List<FormofPaymentOption>();
            SeatChangeState state = persistedState;

            if (state == null && flow == FlowType.VIEWRES.ToString() && ConfigUtility.IsManageResETCEnabled(request.Application.Id, request.Application.Version.Major))
            {
                state = await _sessionHelperService.GetSession<SeatChangeState>(_headers.ContextValues.SessionId, new SeatChangeState().ObjectName, new List<string> { _headers.ContextValues.SessionId, new SeatChangeState().ObjectName }).ConfigureAwait(false);
            }
            if(_configuration.GetValue<bool>("EnableTravelOptionsInViewRes") && flow == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString())
            {
                flow = FlowType.VIEWRES.ToString();
            }
            if (_configuration.GetValue<bool>("GetFoPOptionsAlongRegisterOffers") && shoppingCart.Products != null && shoppingCart.Products.Count > 0)
            {
                FOPEligibilityRequest eligiblefopRequest = new FOPEligibilityRequest()
                {
                    TransactionId = request.TransactionId,
                    DeviceId = request.DeviceId,
                    AccessCode = request.AccessCode,
                    LanguageCode = request.LanguageCode,
                    Application = request.Application,
                    CartId = cartId,
                    SessionId = session.SessionId,
                    Flow = flow,
                    Products = ConfigUtility.GetProductsForEligibleFopRequest(shoppingCart, state)
                };

                var tupleResponse = await EligibleFormOfPayments(eligiblefopRequest, session, isDefault, IsMilesFOPEnabled);
                response = tupleResponse.response;
                isDefault = tupleResponse.isDefault;

                if ((reservation?.IsMetaSearch ?? false) && _configuration.GetValue<bool>("CreditCardFOPOnly_MetaSearch"))
                {
                    if (!_configuration.GetValue<bool>("EnableETCFopforMetaSearch"))
                    {
                        response = response.Where(x => x.Category == "CC").ToList();
                    }
                    else
                    {
                        response = response.Where(x => x.Category == "CC" || x.Category == "CERT").ToList();
                    }

                }

                var upliftFop = _paymentUtility.UpliftAsFormOfPayment(reservation, shoppingCart);

                if (upliftFop != null && response != null)
                {
                    response.Add(upliftFop);
                }

                // Added as part of Money + Miles changes: For MM user have to pay money only using CC - MOBILE-14735;  MOBILE-14833; MOBILE-14925 // MM is only for Booking
                if (ConfigUtility.IncludeMoneyPlusMiles(request.Application.Id, request.Application.Version.Major) && flow == FlowType.BOOKING.ToString())
                {
                    if (response.Exists(x => x.Category == "MILES") && !_configuration.GetValue<bool>("DisableMMFixForSemiLogin_MOBILE17070")
                       && !reservation.IsSignedInWithMP)
                    {
                        response = response.Where(x => x.Category != "MILES").ToList();
                    }
                    if (shoppingCart.FormofPaymentDetails?.MoneyPlusMilesCredit?.SelectedMoneyPlusMiles != null) // selected miles will not be empty only after Applied Miles
                    {
                        response = response.Where(x => x.Category == "CC").ToList();
                    }
                }

                response = TravelBankEFOPFilter(request, shoppingCart?.FormofPaymentDetails?.TravelBankDetails, flow, response);

                if (ConfigUtility.IsETCchangesEnabled(request.Application.Id, request.Application.Version.Major) && flow == FlowType.BOOKING.ToString())
                {
                    response = ConfigUtility.BuildEligibleFormofPaymentsResponse(response, shoppingCart, session, request, reservation?.IsMetaSearch ?? false);
                }
                else if (flow == FlowType.VIEWRES.ToString() && ConfigUtility.IsManageResETCEnabled(request.Application.Id, request.Application.Version.Major))
                {
                    response = ConfigUtility.BuildEligibleFormofPaymentsResponse(response, shoppingCart, request);
                }

                await _sessionHelperService.SaveSession<List<FormofPaymentOption>>(response, session.SessionId, new List<string> { session.SessionId, new FormofPaymentOption().ObjectName }, new FormofPaymentOption().ObjectName).ConfigureAwait(false);//change session
            }

            return (response, isDefault);
        }

        private List<FormofPaymentOption> TravelBankEFOPFilter(MOBRequest request, Mobile.Model.Shopping.FormofPayment.MOBFOPTravelBankDetails travelBankDet, string flow, List<FormofPaymentOption> response)
        {
            if (travelBankDet?.TBApplied > 0 && _paymentUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major) && flow == FlowType.BOOKING.ToString())
            {
                response = response.Where(x => x.Category == "CC").ToList();
            }

            return response;
        }
    }
}
