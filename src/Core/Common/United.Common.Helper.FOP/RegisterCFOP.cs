using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Profile;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.MemberSignIn;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.UpgradeCabin;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.CustomerResponseModel;
using United.Service.Presentation.PaymentModel;
using United.Services.Customer.Common;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using Address = United.Service.Presentation.CommonModel.Address;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using Country = United.Service.Presentation.CommonModel.Country;
using CreditCard = United.Service.Presentation.PaymentModel.CreditCard;
using CslDataVaultRequest = United.Service.Presentation.PaymentRequestModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using CslDataVaultResponse = United.Service.Presentation.PaymentResponseModel.DataVault<United.Service.Presentation.PaymentModel.Payment>;
using Currency = United.Service.Presentation.CommonModel.Currency;
using FlowType = United.Utility.Enum.FlowType;
using FormOfPayment = United.Services.FlightShopping.Common.FlightReservation.FormOfPayment;
using PostPurchasePage = United.Mobile.Model.Common.PostPurchasePage;
using Session = United.Mobile.Model.Common.Session;
using TripSegment = United.Mobile.Model.Shopping.TripSegment;

namespace United.Common.Helper.FOP
{
    public class RegisterCFOP : IRegisterCFOP
    {
        private readonly ICacheLog<RegisterCFOP> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IPaymentUtility _paymentUtility;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IDataVaultService _dataVaultService;
        private readonly string _UPGRADEMALL = "UPGRADEMALL";
        private readonly string _strPARTIALFAILURE = "PARTIAL";
        private readonly string _strMUA = "MUA";
        private readonly string _strSUCCESS = "SUCCESS";
        private readonly string _strPCU = "PCU";
        private readonly string _strUGC = "UGC";
        private readonly IMPTraveler _mPTraveler;
        private readonly IProfileCreditCard _profileCreditCard;
        private readonly ICustomerDataService _customerDataService;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly IHeaders _headers;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;

        public RegisterCFOP(ICacheLog<RegisterCFOP> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IPaymentUtility paymentUtility
            , IShoppingCartService shoppingCartService
            , IDynamoDBService dynamoDBService
            , IDataVaultService dataVaultService
            , ICustomerDataService customerDataService
            , IMPTraveler mPTraveler
            , IProfileCreditCard profileCreditCard
            , IProductInfoHelper productInfoHelper
            , IHeaders headers
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _paymentUtility = paymentUtility;
            _shoppingCartService = shoppingCartService;
            _dynamoDBService = dynamoDBService;
            _dataVaultService = dataVaultService;
            _customerDataService = customerDataService;
            _mPTraveler = mPTraveler;
            _profileCreditCard = profileCreditCard;
            _productInfoHelper = productInfoHelper;
            _headers = headers;
            ConfigUtility.UtilityInitialize(_configuration);
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
        }

        public async Task<CheckOutResponse> RegisterFormsOfPayments_CFOP(CheckOutRequest checkOutRequest)
        {
            CheckOutResponse checkOutResponse = new CheckOutResponse();

            Session session = new Session();
            session = await _sessionHelperService.GetSession<Session>(checkOutRequest.SessionId, session.ObjectName, new List<string> { checkOutRequest.SessionId, session.ObjectName }).ConfigureAwait(false);

            RegisterFormsOfPaymentRequest request = await GetRegisterFormsOfPaymentsRequest_CFOP(checkOutRequest);

            string jsonRequest = JsonConvert.SerializeObject(request);
            string actionName = "RegisterFormsOfPayment";
            var additionalHeaders = _paymentUtility.GetAdditionalHeadersForMosaic(checkOutRequest.Flow);
            var jsonResponse = await _shoppingCartService.GetFormsOfPayments<FlightReservationResponse>(session.Token, actionName, checkOutRequest.SessionId, jsonRequest, additionalHeaders).ConfigureAwait(false);

            if (jsonResponse != null)
            {
                MOBShoppingCart persistShoppingCart = new MOBShoppingCart();
                persistShoppingCart = await _sessionHelperService.GetSession<MOBShoppingCart>(checkOutRequest.SessionId, persistShoppingCart.ObjectName, new List<string> { session.SessionId, persistShoppingCart.ObjectName }).ConfigureAwait(false);
                if (persistShoppingCart == null)
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                if (IsSuccessOrValidReponse(jsonResponse))
                {


                    SeatChangeState state = new SeatChangeState();
                    state = await _sessionHelperService.GetSession<SeatChangeState>(session.SessionId, state.ObjectName, new List<string> { session.SessionId, state.ObjectName }).ConfigureAwait(false);

                    persistShoppingCart.Products = await _productInfoHelper.ConfirmationPageProductInfo(jsonResponse, true, checkOutRequest.Application, state);
                    persistShoppingCart.AlertMessages = await GetErrorMessagesForConfirmationScreen(jsonResponse, persistShoppingCart.Products);
                    var grandtotal = persistShoppingCart.Products != null && persistShoppingCart.Products.Any() ? persistShoppingCart.Products.Sum(p => string.IsNullOrEmpty(p.ProdTotalPrice) ? 0 : Convert.ToDecimal(p.ProdTotalPrice)) : 0;
                    persistShoppingCart.TotalPrice = String.Format("{0:0.00}", grandtotal);
                    //persistShoppingCart.DisplayTotalPrice = grandtotal > 0 || isAFSCouponApplied(jsonResponse.response?.DisplayCart) ? Decimal.Parse(grandtotal.ToString()).ToString("c") : string.Empty;
                    persistShoppingCart.DisplayTotalPrice = grandtotal > 0 || isAFSCouponApplied(jsonResponse?.DisplayCart) ? String.Format("{0:c}", Decimal.Parse(grandtotal.ToString())) : string.Empty;
                    persistShoppingCart.TermsAndConditions = await _productInfoHelper.GetProductBasedTermAndConditions(checkOutRequest.SessionId, jsonResponse, true);
                    persistShoppingCart.DisplayMessage = await  GetConfirmationMessageForWLPNRManageRes(jsonResponse, persistShoppingCart.AlertMessages, checkOutRequest.Flow);
                    bool isCompleteFarelockPurchase = _paymentUtility.IsCheckFareLockUsingProductCode(persistShoppingCart);
                    var emailAddress = string.Empty;
                    if (isCompleteFarelockPurchase)
                    {
                        if (!request.Emails.IsNullOrEmpty() && request.Emails.Any() && !string.IsNullOrEmpty(request.Emails[0].Address))
                        {
                            emailAddress = request.Emails[0].Address;
                        }
                        persistShoppingCart.Captions =await _paymentUtility.GetFareLockCaptions(jsonResponse.Reservation, emailAddress);
                        persistShoppingCart.FlightShareMessage = _paymentUtility.GetFlightShareMessageViewRes(jsonResponse.Reservation);
                    }

                    if (grandtotal > 0 || persistShoppingCart.Products != null && persistShoppingCart.Products.Any() && persistShoppingCart.Products.Any(p => !string.IsNullOrEmpty(p.ProdDisplayOtherPrice)))
                    {
                        if (persistShoppingCart.FormofPaymentDetails == null)
                            persistShoppingCart.FormofPaymentDetails = checkOutRequest.FormofPaymentDetails;

                        persistShoppingCart.FormofPaymentDetails.EmailAddress = IsFareLockApplePay(isCompleteFarelockPurchase, checkOutRequest.FormofPaymentDetails) && !string.IsNullOrEmpty(emailAddress) ? emailAddress : GetCheckOutEmail(checkOutRequest, jsonResponse);
                    }
                    else
                    {
                        persistShoppingCart.FormofPaymentDetails = null;
                    }
                    if (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) && persistShoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count > 0)
                    {
                        double etcBalanceAttentionAmount = persistShoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Sum(c => c.NewValueAfterRedeem);
                        if (Math.Round(etcBalanceAttentionAmount, 2) > 0 && persistShoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count > 1)
                        {
                            persistShoppingCart.ConfirmationPageAlertMessages = _paymentUtility.getEtcBalanceAttentionConfirmationMessages(etcBalanceAttentionAmount);
                        }
                        persistShoppingCart.FormofPaymentDetails.TravelCertificate.AllowedETCAmount = ConfigUtility.GetAlowedETCAmount(persistShoppingCart.Products, persistShoppingCart.Flow);
                        persistShoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates?.ForEach(c => c.RedeemAmount = 0);
                        ShopStaticUtility.AddRequestedCertificatesToFOPTravelerCertificates(persistShoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates, persistShoppingCart.ProfileTravelerCertificates, persistShoppingCart.FormofPaymentDetails.TravelCertificate);
                        var certificatePrice = persistShoppingCart.Prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "CERTIFICATE");
                        ConfigUtility.UpdateCertificatePrice(certificatePrice, persistShoppingCart.FormofPaymentDetails.TravelCertificate.TotalRedeemAmount);
                        _paymentUtility.AssignTotalAndCertificateItemsToPrices(persistShoppingCart);

                    }
                    await _sessionHelperService.SaveSession<MOBShoppingCart>(persistShoppingCart, session.SessionId, new List<string>() { session.SessionId, persistShoppingCart.ObjectName }, persistShoppingCart.ObjectName).ConfigureAwait(false);

                    checkOutResponse.ShoppingCart = persistShoppingCart;
                    checkOutResponse.PostPurchasePage = PostPurchasePage.Confirmation.ToString();
                    checkOutResponse.RecordLocator = jsonResponse.Reservation.ConfirmationID;
                    checkOutResponse.LastName = jsonResponse.Reservation.Travelers[0].Person.Surname;
                    PutPaymentInfoToPaymentTable(checkOutRequest, checkOutResponse, jsonResponse, persistShoppingCart);
                    if (persistShoppingCart.AlertMessages != null && persistShoppingCart.AlertMessages.Any())
                    {
                        _logger.LogError("RegisterFormsOfPayments_CFOP {MOBUnitedException} and {SessionId}", persistShoppingCart.AlertMessages, checkOutRequest.SessionId);
                    }

                    if (string.Equals(checkOutRequest.Flow, _UPGRADEMALL, StringComparison.OrdinalIgnoreCase))
                    {
                        checkOutResponse.ShoppingCart.UpgradeCabinProducts
                            = await GetConfirmedUpgradeProducts(checkOutRequest.SessionId, jsonResponse);
                    }

                    if (_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && persistShoppingCart.DisplayMessage != null && !GeneralHelper.IsApplicationVersionGreater(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major, "PCURefundMessageForIOSOldVersion", "PCURefundMessageForAndroidOldVersion", "", "", true, _configuration))
                    {
                        persistShoppingCart.AlertMessages = persistShoppingCart.AlertMessages != null && persistShoppingCart.AlertMessages.Any() ? null : persistShoppingCart.DisplayMessage;
                    }

                    return checkOutResponse;
                }

                //Merch proccessing failed
                if (jsonResponse?.Errors?.Any(e => e?.MinorCode == "90585") ?? false)
                {
                    checkOutResponse.ShoppingCart = new MOBShoppingCart();
                    checkOutResponse.ShoppingCart.AlertMessages = await GetErrorMessagesForConfirmationScreen(jsonResponse);
                    checkOutResponse.PostPurchasePage = PostPurchasePage.Confirmation.ToString();
                    checkOutResponse.RecordLocator = jsonResponse.Reservation.ConfirmationID;
                    checkOutResponse.LastName = jsonResponse.Reservation.Travelers[0].Person.Surname;
                    if (checkOutResponse.ShoppingCart.AlertMessages != null && checkOutResponse.ShoppingCart.AlertMessages.Any())
                    {
                        _logger.LogError("RegisterFormsOfPayments_CFOP {MOBUnitedException} {PostPurchasePage} {RecordLocator} {LastName} {SessionId} {TransactionId}", checkOutResponse.ShoppingCart.AlertMessages, checkOutResponse.PostPurchasePage, checkOutResponse.RecordLocator, checkOutResponse.LastName, checkOutRequest.SessionId, checkOutRequest.TransactionId);
                    }

                    if (checkOutResponse.ShoppingCart.AlertMessages == null)
                    {
                        throw new MOBUnitedException("There was a problem completing your purchase");
                    }
                    return checkOutResponse;
                }

                //Credit card authorization failed
                if (jsonResponse?.Errors?.Any(e => e?.MinorCode == "90546") ?? false)
                {
                    throw new MOBUnitedException(await GetTextFromDatabase("CreditCardAuthorizationFailure").ConfigureAwait(false));
                }

                //Any other errors
                if (jsonResponse?.Errors?.Any() ?? false)
                {
                    checkOutResponse.IsUpgradePartialSuccess = IsUpgradePartialSuccessUPGRADEMALL(checkOutRequest.Flow, jsonResponse.Warnings);

                    if (jsonResponse.Errors.Any(x => (x.MinorDescription?.Contains("FltResRegisterFormsOfPayment") ?? false) || (x.MinorDescription?.Contains("ServiceErrorSessionNotFound") ?? false)))
                        throw new MOBUnitedException("There was a problem completing your purchase");

                    try
                    {
                       await GenerateTPISecondaryPaymentInfoFOP(checkOutRequest, checkOutResponse, jsonResponse, persistShoppingCart);

                        if (checkOutResponse.IsTPIFailed)
                            return checkOutResponse;
                    }
                    catch { }


                    if (session.IsReshopChange && jsonResponse.Errors.Exists(error => error.MajorCode == "30006.14"))
                    {
                        if (jsonResponse.Errors.Exists(error => error.MinorCode == "90518" || error.MinorCode == "90510"))
                        {
                            var errorCsl = jsonResponse.Errors.FirstOrDefault(error => error.MinorCode == "90518" || error.MinorCode == "90510");
                            throw new MOBUnitedException(errorCsl.MinorCode);
                        }
                    }

                    string errorMessage = string.Empty;

                    foreach (var error in jsonResponse.Errors)
                    {
                        errorMessage = errorMessage + " " + error.Message;
                    }


                    if (errorMessage.ToUpper().Contains("CREDIT"))
                    {
                        throw new MOBUnitedException("We were unable to charge your card as the authorization has been denied. Please contact your financial provider or use a different card.");
                    }
                    else
                    {
                        throw new MOBUnitedException("There was a problem completing your purchase");
                    }
                }
            }

            return checkOutResponse;
        }

        private bool isAFSCouponApplied(DisplayCart displayCart)
        {
            if (displayCart != null && displayCart.SpecialPricingInfo != null && displayCart.SpecialPricingInfo.MerchOfferCoupon != null && !string.IsNullOrEmpty(displayCart.SpecialPricingInfo.MerchOfferCoupon.PromoCode) && displayCart.SpecialPricingInfo.MerchOfferCoupon.IsCouponEligible.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private async Task<RegisterFormsOfPaymentRequest> GetRegisterFormsOfPaymentsRequest_CFOP(CheckOutRequest checkOutRequest)
        {
            var persistedShoppingCart = new MOBShoppingCart();
            persistedShoppingCart = await _sessionHelperService.GetSession<MOBShoppingCart>(checkOutRequest.SessionId, persistedShoppingCart.ObjectName, new List<string> { checkOutRequest.SessionId, persistedShoppingCart.ObjectName }).ConfigureAwait(false);
            if (persistedShoppingCart == null)
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));

            checkOutRequest.PaymentAmount = persistedShoppingCart.TotalPrice;

            bool isCompleteFarelockPurchase = _paymentUtility.IsCheckFareLockUsingProductCode(persistedShoppingCart);
            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(checkOutRequest.SessionId, session.ObjectName, new List<string> { checkOutRequest.SessionId, session.ObjectName }).ConfigureAwait(false);

            RegisterFormsOfPaymentRequest registerFormsOfPaymentRequest = new RegisterFormsOfPaymentRequest();

            if (string.Equals(checkOutRequest.Flow, _UPGRADEMALL, StringComparison.OrdinalIgnoreCase))
            { registerFormsOfPaymentRequest.WorkFlowType = WorkFlowType.UpgradesPurchase; }

            registerFormsOfPaymentRequest.Reservation = null;
            registerFormsOfPaymentRequest.CartId = checkOutRequest.CartId;
            registerFormsOfPaymentRequest.Channel = _configuration.GetValue<string>("Shopping - ChannelType");
            registerFormsOfPaymentRequest.Emails = new Collection<Service.Presentation.CommonModel.EmailAddress>();
            AddEmailToRegisterFormsOfPaymentRequest(checkOutRequest.FormofPaymentDetails.EmailAddress, registerFormsOfPaymentRequest);
            registerFormsOfPaymentRequest.AdditionalData = string.IsNullOrEmpty(checkOutRequest.AdditionalData) ? checkOutRequest.DeviceId : checkOutRequest.AdditionalData;
            registerFormsOfPaymentRequest.FareLockAutoTicket = false;
            registerFormsOfPaymentRequest.DontInvokeCheckout = false;
            if (isCompleteFarelockPurchase)
            {
                registerFormsOfPaymentRequest.FareLockPurchase = true;
                registerFormsOfPaymentRequest.PostPurchase = false;
            }
            else
            {
                registerFormsOfPaymentRequest.PostPurchase = true;
            }
            // Added as part of Bug 128996 - Android: Unable to add PA, OTP, E+ (Ancillary Products) on RTI screen when we take GUAM as billing country : Issuf
            if (checkOutRequest != null && checkOutRequest.FormofPaymentDetails.BillingAddress != null && checkOutRequest.FormofPaymentDetails.BillingAddress.Country != null && !string.IsNullOrEmpty(checkOutRequest.FormofPaymentDetails.BillingAddress.Country.Code))
            {
                registerFormsOfPaymentRequest.CountryCode = checkOutRequest.FormofPaymentDetails.BillingAddress.Country.Code;
            }
            else
                registerFormsOfPaymentRequest.CountryCode = "US";
            registerFormsOfPaymentRequest.RemoveFareLock = false;

            registerFormsOfPaymentRequest.FormsOfPayment = new List<FormOfPayment>();
            FormOfPayment formOfPayment = new FormOfPayment();
            formOfPayment = new FormOfPayment();
            formOfPayment.Payment = new Service.Presentation.PaymentModel.FormOfPayment();
            if (ConfigUtility.IsPOMOffer(persistedShoppingCart.PaymentTarget) && !_configuration.GetValue<bool>("DisableDeviceType"))
            {
                switch (checkOutRequest.Application.Id)
                {
                    case 1:
                        registerFormsOfPaymentRequest.DeviceType = "iOS";
                        break;
                    case 2:
                        registerFormsOfPaymentRequest.DeviceType = "Android";
                        break;
                }
            }

            Address billingAddress = null;

            #region - Fix by Nizam for #2169 - Temporary fix on client behalf
            if (checkOutRequest.PaymentAmount != "0" && checkOutRequest.FormofPaymentDetails.CreditCard == null && checkOutRequest.FormofPaymentDetails.PayPal == null && checkOutRequest.FormofPaymentDetails.PayPalPayor == null &&
                     checkOutRequest.FormofPaymentDetails.masterPass == null && checkOutRequest.FormofPaymentDetails.MasterPassSessionDetails == null && checkOutRequest.FormofPaymentDetails.ApplePayInfo == null && checkOutRequest.FormofPaymentDetails.Uplift == null &&
                      (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) ? (checkOutRequest.FormofPaymentDetails.TravelCertificate == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates?.Count == 0) : true)
                     && !checkOutRequest.FormofPaymentDetails.FormOfPaymentType.IsNullOrEmpty())
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            else if (checkOutRequest.PaymentAmount != "0" && checkOutRequest.FormofPaymentDetails.CreditCard == null && checkOutRequest.FormofPaymentDetails.PayPal == null && checkOutRequest.FormofPaymentDetails.PayPalPayor == null &&
                    checkOutRequest.FormofPaymentDetails.masterPass == null && checkOutRequest.FormofPaymentDetails.MasterPassSessionDetails == null && checkOutRequest.FormofPaymentDetails.ApplePayInfo == null && checkOutRequest.FormofPaymentDetails.Uplift == null
                    && (persistedShoppingCart.Products.Any(x => x.Code == "SEATASSIGNMENTS") &&
                     (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) ? (checkOutRequest.FormofPaymentDetails.TravelCertificate == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates?.Count == 0) : true)
                    && !checkOutRequest.FormofPaymentDetails.FormOfPaymentType.IsNullOrEmpty()))
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            #endregion

            //Fix for iOS only issue when MPAccount has no saved cards added in ViewRes Payment screen (No FormOfPaymentType) -- MOBILE-8915 -- Shashank
            else if (_configuration.GetValue<bool>("NoFormOfPaymentErrorToggle") && checkOutRequest.FormofPaymentDetails.FormOfPaymentType.IsNullOrEmpty() && (checkOutRequest.PaymentAmount != "0" && checkOutRequest.PaymentAmount != "0.00")
                && checkOutRequest.FormofPaymentDetails.CreditCard == null && checkOutRequest.FormofPaymentDetails.PayPal == null && checkOutRequest.FormofPaymentDetails.PayPalPayor == null
                && checkOutRequest.FormofPaymentDetails.masterPass == null && checkOutRequest.FormofPaymentDetails.MasterPassSessionDetails == null && checkOutRequest.FormofPaymentDetails.ApplePayInfo == null && checkOutRequest.FormofPaymentDetails.Uplift == null
                && (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) ? (checkOutRequest.FormofPaymentDetails.TravelCertificate == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates == null || checkOutRequest.FormofPaymentDetails.TravelCertificate?.Certificates?.Count == 0) : true))
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("NoFormOfPaymentErrorMessage"));
            }

            else if (!(checkOutRequest.FormofPaymentDetails.CreditCard == null && checkOutRequest.FormofPaymentDetails.PayPal == null && checkOutRequest.FormofPaymentDetails.PayPalPayor == null &&
                checkOutRequest.FormofPaymentDetails.masterPass == null && checkOutRequest.FormofPaymentDetails.MasterPassSessionDetails == null && checkOutRequest.FormofPaymentDetails.ApplePayInfo == null && checkOutRequest.FormofPaymentDetails.Uplift == null
                && checkOutRequest.FormofPaymentDetails.FormOfPaymentType.IsNullOrEmpty()))
            {
                if ((checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.CreditCard.ToString().ToUpper() ||
                    checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.VisaCheckout.ToString().ToUpper()) && checkOutRequest.FormofPaymentDetails.CreditCard != null ||
                    checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.Uplift.ToString().ToUpper() && checkOutRequest.FormofPaymentDetails.Uplift != null)
                {
                    formOfPayment.Payment.CreditCard = await MapToCslCreditCard(checkOutRequest, persistedShoppingCart.CurrencyCode, session, persistedShoppingCart.PaymentTarget);
                }
                else if ((checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.PayPal.ToString().ToUpper()) || (checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.PayPalCredit.ToString().ToUpper()))
                {
                    #region //**TBD-paypal**//
                    _logger.LogInformation("CheckOut {PayPal_ClientJSONRequest} and {SessionId}", checkOutRequest, session.SessionId);

                    formOfPayment.Payment.PayPal = PayPalPayLoad(persistedShoppingCart.FormofPaymentDetails.PayPalPayor, Convert.ToDouble(checkOutRequest.PaymentAmount), checkOutRequest.FormofPaymentDetails.PayPal);
                    billingAddress = formOfPayment.Payment.PayPal.BillingAddress;
                    AddEmailToRegisterFormsOfPaymentRequest(persistedShoppingCart.FormofPaymentDetails.PayPalPayor.PayPalContactEmailAddress, registerFormsOfPaymentRequest);
                    #endregion //**TBD-paypal**//
                }
                else if (checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.Masterpass.ToString().ToUpper())
                {
                    #region //**TBD-Masterpass**//
                    _logger.LogInformation("CheckOut {Masterpass_ClientJSONRequest} and {SessionId}", checkOutRequest, session.SessionId);
                    formOfPayment.Payment.CreditCard = await MasterpassPayLoad_CFOP(persistedShoppingCart.FormofPaymentDetails.MasterPassSessionDetails, Convert.ToDouble(checkOutRequest.PaymentAmount), Guid.NewGuid().ToString().ToUpper().Replace("-", ""), checkOutRequest.Application, checkOutRequest.DeviceId);
                    billingAddress = formOfPayment.Payment.CreditCard.BillingAddress;
                    AddEmailToRegisterFormsOfPaymentRequest(persistedShoppingCart.FormofPaymentDetails.MasterPassSessionDetails.ContactEmailAddress, registerFormsOfPaymentRequest);
                    #endregion
                }
                else if (checkOutRequest.FormofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.ApplePay.ToString().ToUpper())
                {
                    #region //**Apple Pay**//
                    bool applePayDatavaultTokenToggle = _configuration.GetValue<bool>("ApplePayDatavaultTokenToggle");
                    _logger.LogInformation("CheckOut {ApplePay_ClientJSONRequest} and {SessionId}", checkOutRequest, session.SessionId);
                    if (applePayDatavaultTokenToggle)
                    {
                        formOfPayment.Payment.CreditCard = await ApplePayLoadDataVault_CFOP(checkOutRequest.FormofPaymentDetails.ApplePayInfo, Convert.ToDouble(checkOutRequest.PaymentAmount), session, checkOutRequest.Application, checkOutRequest.DeviceId);
                    }
                    else
                    {
                        formOfPayment.Payment.CreditCard = ApplePayLoad_CFOP(checkOutRequest.FormofPaymentDetails.ApplePayInfo, Convert.ToDouble(checkOutRequest.PaymentAmount), session, checkOutRequest.Application, checkOutRequest.DeviceId);
                    }
                    billingAddress = formOfPayment.Payment.CreditCard.BillingAddress;
                   await _sessionHelperService.SaveSession<CreditCard>(formOfPayment.Payment.CreditCard, session.SessionId, new List<string>() { session.SessionId, formOfPayment.Payment.CreditCard.GetType().FullName }, formOfPayment.Payment.CreditCard.GetType().FullName).ConfigureAwait(false);
                    AddEmailToRegisterFormsOfPaymentRequest(checkOutRequest.FormofPaymentDetails.ApplePayInfo.EmailAddress.EmailAddress, registerFormsOfPaymentRequest);
                    //formOfPayment.PaymentTarget = string.Join(",", persistedShoppingCart.Products.Select(x => x.Code).ToList());
                    #endregion //**Apple Pay**//
                }
            }

            if (string.IsNullOrEmpty(persistedShoppingCart.PaymentTarget))
            {
                formOfPayment.PaymentTarget = isCompleteFarelockPurchase ? "RES" : string.Join(",", persistedShoppingCart.Products.Select(x => x.Code).ToList());
            }
            else
            {
                formOfPayment.PaymentTarget = persistedShoppingCart.PaymentTarget;
            }

            //Setting the PostPurchase to False for the PreOrderMeals Purchase
            if (_configuration.GetValue<bool>("EnableInflightMealsRefreshment") && formOfPayment.PaymentTarget == "POM")
                registerFormsOfPaymentRequest.PostPurchase = false;

            registerFormsOfPaymentRequest.FormsOfPayment.Add(formOfPayment);

            #region Adding certificate, if exist in FOP object
            if (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) && checkOutRequest.Flow == FlowType.VIEWRES.ToString())
            {
                AddCertificateFOP(registerFormsOfPaymentRequest.FormsOfPayment, persistedShoppingCart);
            }
            #endregion

            if (_configuration.GetValue<bool>("EnableCSLCloudMigrationToggle"))
            {
                registerFormsOfPaymentRequest.WorkFlowType = ConfigUtility.GetWorkFlowType(checkOutRequest.Flow, formOfPayment.PaymentTarget);
            }

            if (checkOutRequest.Flow == FlowType.MOBILECHECKOUT.ToString())
            {
                registerFormsOfPaymentRequest.WorkFlowType = (WorkFlowType)persistedShoppingCart.CslWorkFlowType;
            }

            return registerFormsOfPaymentRequest;
        }

        private void AddEmailToRegisterFormsOfPaymentRequest(string email, RegisterFormsOfPaymentRequest registerFormsOfPaymentRequest)
        {
            if (!string.IsNullOrEmpty(email))
            {
                EmailAddress emailAddress = new EmailAddress();
                emailAddress.Address = email;
                registerFormsOfPaymentRequest.Emails.Add(emailAddress);
            }
        }

        private async Task<CreditCard> ApplePayLoadDataVault_CFOP(MOBApplePay applePayInfo,
            double paymentAmount, Session session, MOBApplication application, string deviceId)
        {
            _logger.LogInformation("MakeReservation {ApplePayLoadJSON} and {SessionId}", applePayInfo.ApplePayLoadJSON, session.SessionId);
            ApplePayLoad applePayLoad = JsonConvert.DeserializeObject<ApplePayLoad>(applePayInfo.ApplePayLoadJSON);

            CreditCard creditCard = null;

            creditCard = await GenerateApplepayTokenWithDataVault(applePayLoad, session.SessionId, session.Token, application, deviceId);

            creditCard.Amount = paymentAmount;
            if (_configuration.GetValue<bool>("EDDtoEMDToggle"))
            {
                creditCard.OperationID = Guid.NewGuid().ToString(); // This one we can pass the session id which we using in bookign path.
            }
            else
            {
                creditCard.OperationID = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }
            creditCard.WalletCategory = Service.Presentation.CommonEnumModel.WalletCategory.ApplePay;

            creditCard.Payor = new Service.Presentation.PersonModel.Person();
            creditCard.Name = GetCardHolderFullname(applePayInfo.CardHolderName);
            creditCard.Payor.GivenName = applePayInfo.CardHolderName.First;
            creditCard.Payor.Surname = applePayInfo.CardHolderName.Last;
            creditCard.AccountNumberLastFourDigits = applePayInfo.LastFourDigits;
            creditCard.AccountNumberMasked = "*****" + applePayInfo.LastFourDigits;
            creditCard.BillingAddress = AssignCreditCardBillingAddress(applePayInfo.BillingAddress);
            AssignCSLCreditCardCode(applePayInfo, creditCard);

            creditCard.Currency = new Service.Presentation.CommonModel.Currency();
            creditCard.Currency.Code = applePayInfo.CurrencyCode;  // " PaymentCryptogram ": "AnQeed0ACbTkwQRZF0hUMAACAAA=",

            return creditCard;
        }

        private string GetCardHolderFullname(MOBName cardHolderName)
        {
            return string.Format("{0} {1} {2}", cardHolderName.First, cardHolderName.Middle, cardHolderName.Last).Replace("  ", " ").Trim();
        }

        private Address AssignCreditCardBillingAddress(MOBAddress billingAddress)
        {
            Service.Presentation.CommonModel.Address address = null;
            if (billingAddress != null)
            {
                address = new Service.Presentation.CommonModel.Address();
                var addressLines = new Collection<string>();
                AddAddressLinesToCslBillingAddress(billingAddress.Line1, ref addressLines);
                AddAddressLinesToCslBillingAddress(billingAddress.Line2, ref addressLines);
                AddAddressLinesToCslBillingAddress(billingAddress.Line3, ref addressLines);
                address.AddressLines = addressLines;
                address.City = billingAddress.City;
                if (billingAddress.State != null)
                {
                    address.StateProvince = new Service.Presentation.CommonModel.StateProvince();
                    address.StateProvince.StateProvinceCode = !string.IsNullOrEmpty(billingAddress.State.Code) ? billingAddress.State.Code : billingAddress.State.Name;
                    address.StateProvince.Name = billingAddress.State.Name;
                    address.StateProvince.ShortName = billingAddress.State.Name;
                }
                if (billingAddress.Country != null)
                {
                    address.Country = new Service.Presentation.CommonModel.Country();
                    address.Country.CountryCode = billingAddress.Country.Code.ToUpper();
                }
                address.PostalCode = billingAddress.PostalCode;
            }
            return address;
        }

        private Collection<string> AddAddressLinesToCslBillingAddress(string line, ref Collection<string> lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var linesAfterSplit = line.Replace("\r\n", "|").Replace("\n", "|").Split('|').ToCollection();
                lines = lines.Concat(linesAfterSplit).ToCollection();
            }
            return lines;
        }

        private void AssignCSLCreditCardCode(MOBApplePay mobApplePay, CreditCard creditCard)
        {
            Dictionary<string, string> dictPaymentTypes = new Dictionary<string, string>();
            dictPaymentTypes.Add("VISA", "VI");
            dictPaymentTypes.Add("AMEX", "AX");
            dictPaymentTypes.Add("DISCOVER", "DS");
            dictPaymentTypes.Add("MASTERCARD", "MC");
            dictPaymentTypes.Add("AMERICANEXPRESS", "AX");
            dictPaymentTypes.Add("UNIONPAY", "UP");

            creditCard.Code = dictPaymentTypes[mobApplePay.CardName.ToUpper()];
        }

        private async Task<CreditCard> GenerateApplepayTokenWithDataVault(ApplePayLoad applePayLoad, string sessionID, string token,
            MOBApplication applicationDetails, string deviceID)
        {

            #region
            CreditCard cc = null;
            CslDataVaultRequest dataVaultRequest = GetDataValutRequest(applePayLoad, sessionID);
            string jsonRequest = JsonConvert.SerializeObject(dataVaultRequest);
            string actionName = "AddPayment";
            var jsonResponse =await _dataVaultService.GetCSLWithDataVault<CreditCard>(token, actionName, sessionID, jsonRequest).ConfigureAwait(false);

            if (jsonResponse.response != null)
            {
                var payment = DeserializeDatavaultResponse(sessionID, applicationDetails, deviceID, jsonResponse.response.ToString());
                cc = (CreditCard)payment;
            }
            else
            {
                string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                {
                    exceptionMessage = exceptionMessage + " - due to jsonResponse is empty at DAL  GenerateCCTokenWithDataVault(MOBUpdateTravelerRequest request, ref string ccDataVaultToken)";
                }
                throw new MOBUnitedException(exceptionMessage);
            }

            return cc;
            #endregion
        }

        private CslDataVaultRequest GetDataValutRequest(ApplePayLoad applePayLoad, string sessionID)
        {
            #region
            var dataVaultRequest = new CslDataVaultRequest
            {
                Items = new Collection<Payment>(),
                Types = new Collection<Characteristic>(),
                CallingService = new ServiceClient { Requestor = new Requestor { AgentAAA = "WEB", ApplicationSource = "mobile services" } }
            };
            InsertCreditCardRequest creditCardInsertRequest = new InsertCreditCardRequest();
            if (applePayLoad != null)
            {
                var cc = new CreditCard();
                cc.AccountNumberEncrypted = applePayLoad.Data;
                if (_configuration.GetValue<bool>("PassMobileSessionIDInsteadOfDifferntGuidEveryTime"))
                {
                    cc.OperationID = sessionID; // This one we can pass the session id which we using in bookign path.
                }
                else if (_configuration.GetValue<bool>("EDDtoEMDToggle"))
                {
                    cc.OperationID = Guid.NewGuid().ToString(); // This one we can pass the session id which we using in bookign path.
                }
                else
                {
                    cc.OperationID = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                }
                dataVaultRequest.Items.Add(cc);

                dataVaultRequest.Types = new Collection<Characteristic>();
                dataVaultRequest.Types.Add(new Characteristic { Code = "ENCRYPTION", Value = "APPLEPAY" });
                dataVaultRequest.Types.Add(new Characteristic { Code = "APPLEPAY_PUBLIC_KEY", Value = applePayLoad.Header.EphemeralPublicKey });
            }
            return dataVaultRequest;
            #endregion
        }

        private Payment DeserializeDatavaultResponse(string sessionID, MOBApplication applicationDetails, string deviceID, string jsonResponse)
        {
            Payment payment = null;
            CslDataVaultResponse response = JsonConvert.DeserializeObject<CslDataVaultResponse>(jsonResponse);
            _logger.LogInformation("GenerateCCTokenWithDataVault - DeSerialized Response {DeSerialized Response}", sessionID, applicationDetails.Id, applicationDetails.Version.Major, deviceID, jsonResponse);

            if (response != null && response.Responses != null && response.Responses[0].Error == null && response.Responses[0].Message != null && response.Responses[0].Message.Count > 0 && response.Responses[0].Message[0].Code.Trim() == "0")
            {
                if (response.Items != null && response.Items.Count > 0)
                {
                    payment = response.Items[0];

                }
            }
            else
            {
                if (response.Responses[0].Error != null && response.Responses[0].Error.Count > 0)
                {
                    string errorMessage = string.Empty;
                    foreach (var error in response.Responses[0].Error)
                    {
                        errorMessage = errorMessage + " " + error.Text;
                    }
                    throw new MOBUnitedException(errorMessage);
                }
                else
                {
                    string exceptionMessage = _configuration.GetValue<string>("UnableToInsertCreditCardToProfileErrorMessage");
                    if (_configuration.GetValue<string>("ReturnActualExceptionMessageBackForTesting") != null && _configuration.GetValue<bool>("ReturnActualExceptionMessageBackForTesting"))
                    {
                        exceptionMessage = exceptionMessage + " response.Status not success and response.Errors.Count == 0 - at DAL InsertTravelerCreditCard(MOBUpdateTravelerRequest request)";
                    }
                    throw new MOBUnitedException(exceptionMessage);
                }
            }

            return payment;
        }

        private CreditCard ApplePayLoad_CFOP(MOBApplePay applePayInfo, double paymentAmount, Session session, MOBApplication application, string deviceId)
        {
            _logger.LogInformation("MakeReservation: {ApplePayLoadJSON} and {sessionId}", applePayInfo.ApplePayLoadJSON, session.SessionId);
            ApplePayLoad applePayLoad = JsonConvert.DeserializeObject<ApplePayLoad>(applePayInfo.ApplePayLoadJSON);

            var mobApplePayData = GetApplePayDecryptedData(application, session.SessionId, applePayLoad.Data, applePayLoad.Header.EphemeralPublicKey, deviceId);
            Service.Presentation.PaymentModel.CreditCard creditCard = new Service.Presentation.PaymentModel.CreditCard();
            creditCard.Amount = paymentAmount;
            if (_configuration.GetValue<bool>("EDDtoEMDToggle"))
            {
                creditCard.OperationID = Guid.NewGuid().ToString(); // This one we can pass the session id which we using in bookign path.
            }
            else
            {
                creditCard.OperationID = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }
            creditCard.WalletCategory = Service.Presentation.CommonEnumModel.WalletCategory.ApplePay;
            creditCard.Payor = new Service.Presentation.PersonModel.Person();
            creditCard.Name = GetCardHolderFullname(applePayInfo.CardHolderName);
            creditCard.Payor.GivenName = applePayInfo.CardHolderName.First;
            creditCard.Payor.Surname = applePayInfo.CardHolderName.Last;
            creditCard.AccountNumber = mobApplePayData.ApplicationPrimaryAccountNumber;
            creditCard.AccountNumberLastFourDigits = applePayInfo.LastFourDigits;
            creditCard.AccountNumberMasked = "*****" + applePayInfo.LastFourDigits;

            creditCard.BillingAddress = AssignCreditCardBillingAddress(applePayInfo.BillingAddress);
            AssignCSLCreditCardCode(applePayInfo, creditCard);

            creditCard.Currency = new Service.Presentation.CommonModel.Currency();
            creditCard.Currency.Code = applePayInfo.CurrencyCode;
            creditCard.EciIndicator = mobApplePayData.PaymentData.EciIndicator;
            creditCard.ExpirationDate = DateTime.ParseExact(mobApplePayData.ApplicationExpirationDate, "yyMMdd", CultureInfo.InvariantCulture).ToString("MM/yy");
            creditCard.OnlinePaymentCryptogram = mobApplePayData.PaymentData.OnlinePaymentCryptogram; // " PaymentCryptogram ": "AnQeed0ACbTkwQRZF0hUMAACAAA=",
            return creditCard;
        }

        private ApplePayData GetApplePayDecryptedData(MOBApplication application, string sessionId, string data, string base64Pubkey, string deviceID)
        {
            string certPath = _configuration.GetValue<string>("ApplePayCertPath");
            string certPassword = _configuration.GetValue<string>("ApplePayCertPassword") ?? "";
            _logger.LogInformation("GetApplePayDecryptedData {ApplePay_data} and {SessionId}", data, sessionId);
            _logger.LogInformation("GetApplePayDecryptedData {ApplePay_base64Pubkey} and {SessionId}", string.Format("{0}", base64Pubkey), sessionId);

            ApplePayData mobApplePayData = null;
            var applePayInfo = new Mobile.Model.UnitedClubPasses.ApplePayInfo().Init(base64Pubkey, certPath, certPassword);

            var jsonStr = applePayInfo.DecryptedByAES256GCM(data);

            _logger.LogInformation("GetApplePayDecryptedData {ApplePay_DecryptedJSON} and {SessionId}", jsonStr, sessionId);

            if (!String.IsNullOrEmpty(jsonStr))
                mobApplePayData = JsonConvert.DeserializeObject<ApplePayData>(jsonStr);

            return mobApplePayData;
        }

        private bool IsSuccessOrValidReponse(FlightReservationResponse flightReservationResponse)
        {
            if (flightReservationResponse != null && flightReservationResponse.Status.Equals(United.Services.FlightShopping.Common.StatusType.Success) && (flightReservationResponse.Errors == null || flightReservationResponse.Errors.Count() == 0))
                return true;

            if (flightReservationResponse.Errors != null && flightReservationResponse.Errors.Any(e => e != null && (e.MinorCode == "90506" || e.MinorCode == "90584")))
                return true;

            return false;
        }

        private async Task<List<Section>> GetErrorMessagesForConfirmationScreen(FlightReservationResponse flightReservationResponse, List<ProdDetail> prodDetails = null)
        {
            var alertMessages = new List<Section>();
            if (flightReservationResponse.Errors != null && flightReservationResponse.Errors.Any(e => e != null && (e.MinorCode == "90506" || e.MinorCode == "90585")))
            {
                List<string> refundedSegmentNums = null;
                var isRefundSuccess = false;
                var isPartialSuccess = false;
                var isMerchProcessFailed = false;
                Section pcuAlertMessages = null;
                bool isEnableBEBuyOut = _configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes");
                if (flightReservationResponse.Errors.Any(e => e != null && (e.MinorCode == "90585") && flightReservationResponse.DisplayCart.TravelOptions != null && flightReservationResponse.DisplayCart.TravelOptions.Any(x => x != null && (x.Key == "PCU" || (isEnableBEBuyOut && x.Key == "BEB")))))
                {
                    isMerchProcessFailed = true;
                    pcuAlertMessages = await GetAlertMessage(flightReservationResponse, isRefundSuccess, isPartialSuccess, refundedSegmentNums, isMerchProcessFailed);
                }
                else if (flightReservationResponse.Errors.Any(e => e != null && (e.MinorCode == "90506")))
                {
                    if (isEnableBEBuyOut && flightReservationResponse.DisplayCart.TravelOptions != null && flightReservationResponse.DisplayCart.TravelOptions.Any(t => t != null && t.Key == "BEB"))
                    {
                        isRefundSuccess = IsEMDRefundSuccess(flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items);
                    }
                    else
                    {
                        bool DisableFixForPCUPurchaseFailMsg_MOBILE15837 = _configuration.GetValue<bool>("DisableFixForPCUPurchaseFailMsg_MOBILE15837");
                        isRefundSuccess = ShopStaticUtility.IsRefundSuccess(flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items, out refundedSegmentNums, DisableFixForPCUPurchaseFailMsg_MOBILE15837);
                        isPartialSuccess = isRefundSuccess ? flightReservationResponse.DisplayCart.TravelOptions.Where(t => t.Key == "PCU").SelectMany(x => x.SubItems).Where(x => x.Amount != 0).SelectMany(s => s.SegmentNumber).Distinct().Count() != refundedSegmentNums.Count() : false;
                        if (!isPartialSuccess && isRefundSuccess)
                        {
                            //if (_configuration.GetValue<bool>("EnableCSL30ManageResSelectSeatMap"))
                            //{
                                if (flightReservationResponse.DisplayCart.DisplaySeats != null && flightReservationResponse.DisplayCart.DisplaySeats.Any(s => (s.Seat == "---" || string.IsNullOrEmpty(s.SeatAssignMessage) || !s.SeatAssignMessage.Equals("SEATS ASSIGNED", StringComparison.OrdinalIgnoreCase))))
                                {
                                    isPartialSuccess = true;
                                }
                            //}
                            //else
                            //{
                            //    if (flightReservationResponse.DisplayCart.DisplaySeats != null && flightReservationResponse.DisplayCart.DisplaySeats.Any(s => (s.Seat == "---" || !string.IsNullOrEmpty(s.SeatAssignMessage))))
                            //    {
                            //        isPartialSuccess = true;
                            //    }
                            //}

                            if (!isPartialSuccess)
                            {
                                isPartialSuccess = HasAnySuccessfullProduct(prodDetails);
                            }
                        }
                    }

                    pcuAlertMessages = await GetAlertMessage(flightReservationResponse, isRefundSuccess, isPartialSuccess, refundedSegmentNums, isMerchProcessFailed);
                }

                if (pcuAlertMessages != null)
                {
                    alertMessages.Add(pcuAlertMessages);
                }
            }

            if (IsThereSeatAssigmentFailure(flightReservationResponse))
            {
                var seatFailureAlertMessage = new Section()
                {
                    Text1 = "Seat assignment failed",
                    Text2 = _configuration.GetValue<string>("SeatsUnAssignedMessage").Trim()
                };

                alertMessages.Add(seatFailureAlertMessage);
            }
            return alertMessages.Any() ? alertMessages : null;
        }

        private async Task<Section> GetAlertMessage(FlightReservationResponse flightReservationResponse, bool isRefundSuccess, bool isPartialSuccess, List<string> refundedSegmentNums, bool isGenericError)
        {
            Section alertMessage = new Section();

            if (isGenericError)
            {
                var errorMessages = await _productInfoHelper.GetCaptions("PCU_UpgradeFailed_GenericError");
                alertMessage.Text1 = errorMessages.Where(x => x.Id == "HEADER").Select(x => x.CurrentValue).FirstOrDefault().ToString();
                alertMessage.Text2 = errorMessages.Where(x => x.Id == "BODY").Select(x => x.CurrentValue).FirstOrDefault().ToString();
            }
            else if (isRefundSuccess && isPartialSuccess)
            {
                var errorMessages = await _productInfoHelper.GetCaptions("PCU_UpgradePartialFailure_Refunded");
                alertMessage.Text1 = errorMessages.Where(x => x.Id == "HEADER").Select(x => x.CurrentValue).FirstOrDefault().ToString();
                alertMessage.Text2 = errorMessages.Where(x => x.Id == "BODY").Select(x => x.CurrentValue).FirstOrDefault().ToString() + BuildRefundedSegmentsMessage(flightReservationResponse, refundedSegmentNums, errorMessages); ;
            }
            else if (!isPartialSuccess && isRefundSuccess)
            {
                var errorMessages = await _productInfoHelper.GetCaptions("PCU_UpgradeFailed_Refunded");
                alertMessage.Text1 = errorMessages.Where(x => x.Id == "HEADER").Select(x => x.CurrentValue).FirstOrDefault().ToString();
                alertMessage.Text2 = errorMessages.Where(x => x.Id == "BODY").Select(x => x.CurrentValue).FirstOrDefault().ToString();
            }
            else if (!isRefundSuccess)
            {
                var errorMessages = await _productInfoHelper.GetCaptions("PCU_UpgradeFailed");
                alertMessage.Text1 = errorMessages.Where(x => x.Id == "HEADER").Select(x => x.CurrentValue).FirstOrDefault().ToString();
                alertMessage.Text2 = errorMessages.Where(x => x.Id == "BODY").Select(x => x.CurrentValue).FirstOrDefault().ToString();
            }

            return alertMessage;
        }

        private bool IsEMDRefundSuccess(Collection<ShoppingCartItemResponse> items)
        {
            var item = items?.FirstOrDefault(i => i != null && i.Item != null && !string.IsNullOrEmpty(i.Item.Category) && i.Item.Category.Equals("Reservation.Merchandise.BEB"));
            if (item == null) return false;

            var couponStatus = item?.Item?.Product?.FirstOrDefault()?.EmdDetails?.FirstOrDefault(p => p != null && !string.IsNullOrEmpty(p.CouponStatus)).CouponStatus;
            if (!string.IsNullOrEmpty(couponStatus) && couponStatus.Equals("REFUND:SUCCESS", StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }

        private bool HasAnySuccessfullProduct(List<ProdDetail> prodDetails)
        {
            return prodDetails != null && prodDetails.Any() && prodDetails.Any(p => p != null && p.Segments != null && p.Segments.Any(s => s != null && s.SubSegmentDetails != null && s.SubSegmentDetails.Any(subSeg => subSeg != null && !subSeg.IsPurchaseFailure)));
        }

        private bool IsThereSeatAssigmentFailure(FlightReservationResponse flightReservationResponse)
        {
            //if (_configuration.GetValue<bool>("EnableCSL30ManageResSelectSeatMap"))
            //{
                if (flightReservationResponse.DisplayCart.DisplaySeats != null && flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s.Seat == "---" || string.IsNullOrEmpty(s.SeatAssignMessage) || !s.SeatAssignMessage.Equals("SEATS ASSIGNED", StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            //}
            /*
            else
            {
                if (flightReservationResponse.Errors != null && flightReservationResponse.Errors.Any(e => e != null && (e.MinorCode == "90584")))
                {
                    if (flightReservationResponse.Errors.Any(e => e != null && (e.Message == "SeatAssignmentFailed . ")))
                    {
                        return true;
                    }
                    if (_configuration.GetValue<bool>("EnableCSL30ManageResSelectSeatMap"))
                    {
                        if (flightReservationResponse.DisplayCart.DisplaySeats != null && flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s.Seat == "---" || string.IsNullOrEmpty(s.SeatAssignMessage) || !s.SeatAssignMessage.Equals("SEATS ASSIGNED", StringComparison.OrdinalIgnoreCase)))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (flightReservationResponse.DisplayCart.DisplaySeats != null && flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s.Seat == "---" || !string.IsNullOrEmpty(s.SeatAssignMessage)))
                        {
                            return true;
                        }
                    }
                }
            } 
            */

            return false;
        }

        private string BuildRefundedSegmentsMessage(FlightReservationResponse flightReservationResponse, List<string> refundedSegmentNums, List<MOBItem> errorMessages)
        {
            if (refundedSegmentNums == null || !refundedSegmentNums.Any() || errorMessages == null || !errorMessages.Any())
                return string.Empty;

            string subcontent = errorMessages.Where(x => x.Id == "OPTIONALSUBCONTENT").Select(x => x.CurrentValue).FirstOrDefault().ToString();

            List<string> refundMessages = new List<string>();
            foreach (var flightSegment in flightReservationResponse.Reservation.FlightSegments)
            {
                if (refundedSegmentNums.Contains(flightSegment.SegmentNumber.ToString()))
                {
                    var segmentDescription = flightSegment.FlightSegment.DepartureAirport.IATACode + " - " + flightSegment.FlightSegment.ArrivalAirport.IATACode;
                    var refundedPrice = flightReservationResponse.DisplayCart.TravelOptions.Where(t => t.Key == "PCU").SelectMany(x => x.SubItems).Where(x => x.Amount != 0 && x.SegmentNumber == flightSegment.SegmentNumber.ToString()).Select(x => x.Amount).Sum().ToString("c");
                    refundMessages.Add(string.Format(subcontent, segmentDescription, refundedPrice));
                }
            }
            return refundMessages.Any() ? string.Join("<br>", refundMessages) : string.Empty;
        }

        private async Task<List<Section>> GetConfirmationMessageForWLPNRManageRes(FlightReservationResponse flightReservationResponse, List<Section> AlertMessages, string Flow)
        {
            var message = new List<Section>();
            if (!flightReservationResponse.Errors.Any() || !AlertMessages.Any())
            {
                bool isPCUPurchase = flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES").Select(x => x.Product.FirstOrDefault().Code).Distinct().Any(x => x.Equals("PCU"));
                // PCU Waitlist Confirmation Message
                if (_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isPCUPurchase && IsTherePCUWaitListSegment(flightReservationResponse))
                {
                    var getPCUSegmentFromTravelOptions = _paymentUtility.getPCUSegments(flightReservationResponse, Flow);
                    var isUPP = getPCUSegmentFromTravelOptions != null ? _paymentUtility.isPCUUPPWaitListSegment(getPCUSegmentFromTravelOptions, flightReservationResponse.Reservation.FlightSegments) : false;
                    var refundMessages = isUPP ? await _productInfoHelper.GetCaptions("PC_PCUWaitList_RefundMessage_UPPMessage") : await _productInfoHelper.GetCaptions("PC_PCUWaitList_RefundMessage_GenericMessage");
                    message = _paymentUtility.AssignRefundMessage(refundMessages);
                }
            }
            return message.Any() ? message : null;
        }

        private static bool IsTherePCUWaitListSegment(FlightReservationResponse flightReservationResponse)
        {
            return flightReservationResponse.CheckoutResponse.ShoppingCartResponse.UpgradeDetails.Any(p => p != null && p.Flight != null && !p.Flight.FlightStatus.IsNullOrEmpty() && p.Flight.FlightStatus.Equals("WAITLISTUPGRADEREQUESTED", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string> GetTextFromDatabase(string key)
        {
            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(key, _headers.ContextValues.SessionId, false).ConfigureAwait(false);

            if (docs == null || !docs.Any()) return null;
            var doc = docs.FirstOrDefault();
            return doc == null ? null : doc.LegalDocument;
        }

        private bool IsFareLockApplePay(bool isCompleteFareLockPurchase, MOBFormofPaymentDetails formofPaymentDetails)
        {
            if (isCompleteFareLockPurchase && formofPaymentDetails != null && !formofPaymentDetails.FormOfPaymentType.IsNullOrEmpty())
            {
                if (formofPaymentDetails.FormOfPaymentType.ToUpper() == MOBFormofPayment.ApplePay.ToString().ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        private void PutPaymentInfoToPaymentTable(CheckOutRequest checkOutRequest, CheckOutResponse checkOutResponse, FlightReservationResponse flightReservationResponse, MOBShoppingCart persistShoppingCart)
        {
            if (!(checkOutRequest.FormofPaymentDetails.CreditCard == null && checkOutRequest.FormofPaymentDetails.PayPal == null && checkOutRequest.FormofPaymentDetails.PayPalPayor == null &&
                                checkOutRequest.FormofPaymentDetails.masterPass == null && checkOutRequest.FormofPaymentDetails.MasterPassSessionDetails == null && checkOutRequest.FormofPaymentDetails.ApplePayInfo == null
                                && checkOutRequest.FormofPaymentDetails.FormOfPaymentType == null))
            {
                try
                {
                    bool isCompleteFarelockPurchase = persistShoppingCart != null && persistShoppingCart.Products != null ? _paymentUtility.IsCheckFareLockUsingProductCode(persistShoppingCart) : false;

                    if (flightReservationResponse.DisplayCart.TravelOptions != null && flightReservationResponse.DisplayCart.TravelOptions.Any())
                    {
                        var productCodes = flightReservationResponse.DisplayCart.TravelOptions.Select(t => t.Type != "SEATASSIGNMENTS" ? t.Key : t.Type).Distinct();
                        string xmlRemark = JsonConvert.SerializeObject(checkOutResponse);
                        
                        if (_configuration.GetValue<bool>("EnableMobileCheckoutChanges"))
                        {
                            productCodes = productCodes.Select(p => $"{checkOutRequest.Flow}-{p}").ToList();
                        }
                        if (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) ? checkOutRequest.FormofPaymentDetails.FormOfPaymentType != "ETC" : true)
                        {
                            AddPaymentNew(checkOutRequest.SessionId, checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major, string.Join(",", productCodes), Convert.ToDouble(persistShoppingCart.TotalPrice), "USD", 0, xmlRemark, checkOutRequest.Application.Id.ToString(), _configuration.GetValue<bool>("IsBookingTest"),
                       checkOutRequest.SessionId,
                       checkOutRequest.DeviceId,
                       flightReservationResponse.Reservation.ConfirmationID,
                       null,
                       checkOutRequest.FormofPaymentDetails.FormOfPaymentType,
                       GetRestAPIVersionBasedonFlowType(checkOutRequest.Flow));

                        }

                        if (ConfigUtility.IsManageResETCEnabled(checkOutRequest.Application.Id, checkOutRequest.Application.Version.Major) && persistShoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count > 0)
                        {
                            AddPaymentNew(
                              checkOutRequest.SessionId,
                              checkOutRequest.Application.Id,
                              checkOutRequest.Application.Version.Major,
                              string.Join(",", productCodes),
                              Convert.ToDouble(persistShoppingCart.Prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "CERTIFICATE")?.Value),
                             "USD",
                              0,
                              xmlRemark,
                              checkOutRequest.Application.Id.ToString(),
                              _configuration.GetValue<bool>("IsBookingTest"),
                              checkOutRequest.SessionId,
                              checkOutRequest.DeviceId,
                              flightReservationResponse.Reservation.ConfirmationID,
                              null,
                              "ETC",
                              GetRestAPIVersionBasedonFlowType(checkOutRequest.Flow));
                        }
                    }
                    else if (isCompleteFarelockPurchase)
                    {
                        var productCodes = "FLK";
                        string xmlRemark = JsonConvert.SerializeObject(checkOutResponse);
                        AddPaymentNew(
                            checkOutRequest.SessionId,
                            checkOutRequest.Application.Id,
                            checkOutRequest.Application.Version.Major,
                            string.Join(",", productCodes),
                            Convert.ToDouble(persistShoppingCart.TotalPrice),
                           "USD",
                            0,
                            xmlRemark,
                            checkOutRequest.Application.Id.ToString(),
                            _configuration.GetValue<bool>("IsBookingTest"),
                            checkOutRequest.SessionId,
                            checkOutRequest.DeviceId,
                            flightReservationResponse.Reservation.ConfirmationID,
                            null,
                            checkOutRequest.FormofPaymentDetails.FormOfPaymentType,
                            GetRestAPIVersionBasedonFlowType(checkOutRequest.Flow));
                    }
                }
                catch { }
            }
        }

        private string GetRestAPIVersionBasedonFlowType(string flowType)
        {
            string flow = string.Empty;

            if (!string.IsNullOrEmpty(flowType))
            {
                switch (flowType.ToUpper())
                {
                    case "VIEWRES":
                    case "VIEWRES_SEATMAP":
                        flow = "ViewRES_CFOP";
                        break;
                }
            }
            return flow;
        }

        public void AddPaymentNew(string transactionId,
                                  int applicationId,
                                  string applicationVersion,
                                  string paymentType,
                                  double amount,
                                  string currencyCode,
                                  int mileage,
                                  string remark,
                                  string insertBy,
                                  bool isTest,
                                  string sessionId,
                                  string deviceId,
                                  string recordLocator,
                                  string mileagePlusNumber,
                                  string formOfPayment,
                                  string restAPIVersion = "")
        {
            if (_configuration.GetValue<string>("SendPayment") != null && _configuration.GetValue<string>("SendPayment").ToUpper().Trim() == "TRUE")
            {
                var seatMapDynamoDB = new SeatMapDynamoDB(_configuration, _dynamoDBService);
                var savePaymentNew = new PaymentDB()
                {
                    TransactionId = transactionId,
                    ApplicationId = applicationId,
                    ApplicationVersion = applicationVersion,
                    PaymentType = "OTP Purchase using datavault, ME and OTP Service",
                    Amount = amount,
                    CurrencyCode = "USD",
                    Mileage = 0,
                    Remark = remark,
                    InsertBy = insertBy,
                    IsTest = _configuration.GetValue<bool>("IsBookingTest") ? "Y" : "N",
                    SessionId = sessionId,
                    DeviceId = deviceId,
                    RecordLocator = _configuration.GetValue<string>("LogExceptionOnly") != null ? _configuration.GetValue<string>("RESTWEBAPIVersion") : string.Empty,
                    MileagePlusNumber = mileagePlusNumber,
                    FormOfPayment = "CreditCard",
                    RestAPIVersion = restAPIVersion
                };
                var key = transactionId + "::" + applicationId;
                var returnValue = seatMapDynamoDB.AddPaymentNew<PaymentDB>(savePaymentNew, key, sessionId);
            }
        }

        internal void AddCertificateFOP(List<FormOfPayment> formOfPayment, MOBShoppingCart shoppingCart)
        {
            if (shoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count > 0)
            {
                MOBFOPCertificate[] usedCertificates = new MOBFOPCertificate[shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Count];
                shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.CopyTo(usedCertificates);
                var usedCertificatesList = usedCertificates.ToList();
                AssignRESAmountToCSLFop(formOfPayment, shoppingCart, usedCertificatesList);
                string[] CombinebilityETCAppliedAncillaryCodes;
                if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && shoppingCart.Flow == FlowType.VIEWRES.ToString())
                {
                    CombinebilityETCAppliedAncillaryCodes = _configuration.GetValue<string>("VIewResETCEligibleProducts").Split('|');
                }
                else
                {
                    string configCombinebilityETCAppliedAncillaryCodes = _configuration.GetValue<string>("CombinebilityETCAppliedAncillaryCodes");
                    if (_configuration.GetValue<bool>("ETCForAllProductsToggle"))
                    {
                        string nonBundleProductCode = _configuration.GetValue<string>("NonBundleProductCode");
                        List<ProdDetail> bundleProducts = shoppingCart.Products.FindAll(p => (nonBundleProductCode.IndexOf(p.Code) == -1) && !string.IsNullOrEmpty(p.ProdTotalPrice));
                        if (bundleProducts != null && bundleProducts.Count > 0)
                        {
                            string bundleProductCodes = string.Join("|", bundleProducts.Select(p => p.Code));
                            bundleProductCodes = bundleProductCodes.Trim('|');
                            configCombinebilityETCAppliedAncillaryCodes += "|" + bundleProductCodes;
                        }
                    }
                    CombinebilityETCAppliedAncillaryCodes = configCombinebilityETCAppliedAncillaryCodes.Split('|');
                }
                double? allowedETCAncillaryAmount = GetAlowedETCAncillaryAmount(shoppingCart.Products, shoppingCart.Flow);
                if (allowedETCAncillaryAmount > 0 && usedCertificatesList.Count > 0)
                {
                    AssignAncillaryAmountToCSLFOP(formOfPayment, shoppingCart, allowedETCAncillaryAmount, usedCertificatesList, CombinebilityETCAppliedAncillaryCodes);
                }
            }
        }

        private void AssignRESAmountToCSLFop(List<FormOfPayment> formOfPayment, MOBShoppingCart shoppingCart, List<MOBFOPCertificate> usedCertificatesList)
        {
            double allowedETCRESAmount = shoppingCart.Products.Exists(c => c.Code.Equals("RES")) ? Convert.ToDouble(shoppingCart.Products.Find(c => c.Code.Equals("RES")).ProdTotalPrice) : 0;
            foreach (var certificate in usedCertificatesList)
            {
                if (allowedETCRESAmount == 0)
                    break;

                foreach (var sctraveler in shoppingCart.SCTravelers)
                {
                    if (certificate.RedeemAmount == 0 || allowedETCRESAmount == 0)
                        break;

                    IEnumerable<Services.FlightShopping.Common.FlightReservation.FormOfPayment> UserAssignedCSLFOPs = formOfPayment.Where(fp => fp?.Payment?.Certificate?.Payor.Key == sctraveler.TravelerNameIndex);
                    decimal etcAmountAddedForCustomer = 0;
                    if (UserAssignedCSLFOPs != null && UserAssignedCSLFOPs.Count() > 0)
                    {
                        etcAmountAddedForCustomer = UserAssignedCSLFOPs.Sum(fpAmt => fpAmt.Amount);
                    }
                    if (etcAmountAddedForCustomer < (decimal)sctraveler.IndividualTotalAmount)
                    {
                        double certificateAmount = AssignRedeemAmountWhichIsGreater(certificate.RedeemAmount, (sctraveler.IndividualTotalAmount - (double)etcAmountAddedForCustomer));

                        allowedETCRESAmount -= certificateAmount;
                        certificate.RedeemAmount -= certificateAmount;

                        if (certificateAmount > 0)
                        {
                            Services.FlightShopping.Common.FlightReservation.FormOfPayment fop = BuildsRESCslFOP(shoppingCart, certificate, sctraveler, certificateAmount);
                            var ccRESFop = formOfPayment.Find(p => p.PaymentTarget == "RES" && p.Payment.CreditCard != null);
                            if (ccRESFop != null)
                            {
                                ccRESFop.Payment.CreditCard.Amount -= certificateAmount;
                                ccRESFop.Payment.CreditCard.Amount = Math.Round(ccRESFop.Payment.CreditCard.Amount, 2, MidpointRounding.AwayFromZero);
                                if (ccRESFop.Payment.CreditCard.Amount == 0)
                                {
                                    formOfPayment.Remove(ccRESFop);
                                }
                            }
                            formOfPayment.Add(fop);
                        }
                    }
                }
            }
        }

        private double AssignRedeemAmountWhichIsGreater(double certificateRedeemAmount, double redeemAmountFromCertificate)
        {
            return certificateRedeemAmount > redeemAmountFromCertificate ? redeemAmountFromCertificate : certificateRedeemAmount;
        }

        private FormOfPayment BuildsRESCslFOP(MOBShoppingCart shoppingCart, MOBFOPCertificate certificate, MOBCPTraveler sctraveler, double certificateAmount)
        {
            var fop = GetCSLFOP(shoppingCart.FormofPaymentDetails, shoppingCart.SCTravelers, certificate);
            fop.Amount = (Math.Round((decimal)certificateAmount, 2));
            fop.Payment.Certificate.Amount = Math.Round(certificateAmount, 2);
            if (sctraveler.TravelerTypeCode.ToUpper().Equals("INF"))
            {
                MOBCPTraveler mOBCPTraveler = shoppingCart.SCTravelers.FirstOrDefault(st => !st.TravelerTypeCode.ToUpper().Equals("INF"));
                fop.Payment.Certificate.Payor.Key = mOBCPTraveler != null ? mOBCPTraveler.TravelerNameIndex : sctraveler.TravelerNameIndex;
            }
            else
            {
                fop.Payment.Certificate.Payor.Key = sctraveler.TravelerNameIndex;
            }
            fop.Payment.Certificate.Payor.Type = string.IsNullOrEmpty(sctraveler.CslReservationPaxTypeCode) ? sctraveler.TravelerTypeCode : sctraveler.CslReservationPaxTypeCode;
            fop.PaymentTarget = "RES";
            return fop;
        }

        private FormOfPayment GetCSLFOP(MOBFormofPaymentDetails fopDtl, List<MOBCPTraveler> scTravelers, MOBFOPCertificate certificate)
        {
            Services.FlightShopping.Common.FlightReservation.FormOfPayment fop = new Services.FlightShopping.Common.FlightReservation.FormOfPayment();
            fop.Payment = new Service.Presentation.PaymentModel.FormOfPayment();
            fop.PaymentTarget = "RES";
            fop.Amount = (decimal)certificate.RedeemAmount;
            fop.Payment.Certificate = new Certificate();
            fop.Payment.Certificate = GetCSLCertificate(certificate, fopDtl.BillingAddress, fopDtl.Email, fopDtl.Phone, scTravelers);
            return fop;
        }

        private Certificate GetCSLCertificate(MOBFOPCertificate certificate, MOBAddress billingAddress, MOBEmail email, MOBCPPhone phone, List<MOBCPTraveler> scTravelers)
        {
            Certificate cslCertificate = new Certificate();
            cslCertificate.Amount = certificate.RedeemAmount;
            cslCertificate.BillingAddress = GetCSLBillingAddress(billingAddress);
            cslCertificate.OperationID = Guid.NewGuid().ToString();
            cslCertificate.PinCode = certificate.PinCode;
            cslCertificate.PromoCode = certificate.YearIssued.Substring(2, 2) + "TCVA";
            cslCertificate.Payor = new Service.Presentation.PersonModel.Person();
            cslCertificate.Payor.GivenName = certificate.RecipientsLastName;
            if (_configuration.GetValue<bool>("CombinebilityETCToggle"))
            {
                cslCertificate.Currency = new Currency();
                cslCertificate.Currency.Code = "USD";
            }
            if (_configuration.GetValue<bool>("MTETCToggle") && certificate.CertificateTraveler != null && certificate.CertificateTraveler?.TravelerNameIndex != "0")
            {
                MOBCPTraveler mOBCPTraveler1 = scTravelers.Find(t => t.TravelerNameIndex == certificate.CertificateTraveler.TravelerNameIndex);
                cslCertificate.Payor.Type = string.IsNullOrEmpty(mOBCPTraveler1.CslReservationPaxTypeCode) ? mOBCPTraveler1.TravelerTypeCode : mOBCPTraveler1.CslReservationPaxTypeCode;
                if (cslCertificate.Payor.Type.ToUpper().Equals("INF"))
                {
                    MOBCPTraveler mOBCPTraveler = scTravelers.FirstOrDefault(st => !st.TravelerTypeCode.ToUpper().Equals("INF"));
                    cslCertificate.Payor.Key = mOBCPTraveler != null ? mOBCPTraveler.TravelerNameIndex : certificate.CertificateTraveler.TravelerNameIndex;
                }
                else
                {
                    cslCertificate.Payor.Key = certificate.CertificateTraveler.TravelerNameIndex;
                }
            }
            else
            {
                if (!certificate.IsForAllTravelers)
                {
                    cslCertificate.Payor.Key = certificate.TravelerNameIndex;
                    cslCertificate.Payor.Type = scTravelers.Find(t => t.TravelerNameIndex == certificate.TravelerNameIndex)?.TravelerTypeCode;
                }
                else if (scTravelers.Count > 0)
                {
                    cslCertificate.Payor.Key = scTravelers[0].TravelerNameIndex;
                    cslCertificate.Payor.Type = scTravelers[0].TravelerTypeCode;
                }
            }
            if (email != null || phone != null)
            {
                cslCertificate.Payor.Contact = new Service.Presentation.PersonModel.Contact();
                if (email != null)
                {
                    cslCertificate.Payor.Contact.Emails = new System.Collections.ObjectModel.Collection<EmailAddress>();
                    EmailAddress cslEmail = new EmailAddress();
                    cslEmail.Address = email.EmailAddress;
                    cslCertificate.Payor.Contact.Emails.Add(cslEmail);
                }
                if (phone != null)
                {
                    cslCertificate.Payor.Contact.PhoneNumbers = new System.Collections.ObjectModel.Collection<Telephone>();
                    Telephone cslPhone = new Telephone();
                    cslPhone.PhoneNumber = phone.AreaNumber + phone.PhoneNumber;
                    cslPhone.CountryAccessCode = phone.CountryCode;
                }
            }

            return cslCertificate;
        }

        private Address GetCSLBillingAddress(MOBAddress billingAddress)
        {
            Address cslAddress = new Address();
            cslAddress.AddressLines = new System.Collections.ObjectModel.Collection<string>();
            cslAddress.AddressLines.Add(billingAddress.Line1);
            cslAddress.AddressLines.Add(billingAddress.Line2);
            cslAddress.AddressLines.Add(billingAddress.Line3);
            cslAddress.Country = new Country();
            if (billingAddress.Country != null)
            {
                cslAddress.Country.CountryCode = billingAddress.Country.Code;
            }
            cslAddress.City = billingAddress.City;
            cslAddress.PostalCode = billingAddress.PostalCode;
            cslAddress.StateProvince = new StateProvince();
            cslAddress.StateProvince.StateProvinceCode = billingAddress.State.Code;
            cslAddress.StateProvince.ShortName = billingAddress.State.Code;

            return cslAddress;
        }

        private double? GetAlowedETCAncillaryAmount(List<ProdDetail> products, string flow)
        {
            string allowedETCAncillaryProducts = string.Empty;
            if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && flow == FlowType.VIEWRES.ToString())
            {
                allowedETCAncillaryProducts = _configuration.GetValue<string>("VIewResETCEligibleProducts");
            }
            else
            {
                allowedETCAncillaryProducts = _configuration.GetValue<string>("CombinebilityETCAppliedAncillaryCodes");
            }
            double? totalAncillaryAmount = products == null ? 0 : products.Where(p => (allowedETCAncillaryProducts.IndexOf(p.Code) > -1) && !string.IsNullOrEmpty(p.ProdTotalPrice))?.Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            if (_configuration.GetValue<bool>("ETCForAllProductsToggle"))
            {
                totalAncillaryAmount += GetBundlesAmount(products, flow);
            }
            return totalAncillaryAmount;
        }

        private double GetBundlesAmount(List<ProdDetail> products, string flow)
        {
            string nonBundleProductCode = _configuration.GetValue<string>("NonBundleProductCode");
            double bundleAmount = products == null ? 0 : products.Where(p => (nonBundleProductCode.IndexOf(p.Code) == -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            return bundleAmount;
        }

        private void AssignAncillaryAmountToCSLFOP(List<FormOfPayment> formOfPayment, MOBShoppingCart shoppingCart, double? allowedETCAncillaryAmount, List<MOBFOPCertificate> usedCertificatesList, string[] CombinebilityETCAppliedAncillaryCodes)
        {
            #region logic for finding the PCUSeat.(There is no separate for product for PCU in shoppingcart when purchase through seats flow in manageres.
            double pcuTotalPrice = _paymentUtility.getPCUProductPrice(shoppingCart);
            bool isPCUSeat = pcuTotalPrice > 0 && shoppingCart.Products.Exists(p => p.Code == "SEATASSIGNMENTS");
            #endregion logic for finding the PCUSeat
            foreach (var CombinebilityETCAppliedAncillaryCode in CombinebilityETCAppliedAncillaryCodes)
            {
                var product = shoppingCart.Products.Find(p => p.Code == CombinebilityETCAppliedAncillaryCode);
                if (product != null || (isPCUSeat && CombinebilityETCAppliedAncillaryCode == "PCU"))
                {
                    var productAmount = product != null ? (isPCUSeat ? Convert.ToDouble(product.ProdTotalPrice) - pcuTotalPrice : Convert.ToDouble(product.ProdTotalPrice)) : pcuTotalPrice;
                    foreach (var certificate in usedCertificatesList)
                    {
                        if (certificate.RedeemAmount > 0 && productAmount > 0)
                        {
                            double certificateAmount = AssignRedeemAmountWhichIsGreater(certificate.RedeemAmount, productAmount);

                            certificate.RedeemAmount -= certificateAmount;
                            productAmount -= certificateAmount;
                            allowedETCAncillaryAmount -= certificateAmount;

                            certificate.CertificateTraveler = null;
                            certificate.IsForAllTravelers = true;
                            int i = 0;
                            if (certificateAmount > 0)
                            {
                                Services.FlightShopping.Common.FlightReservation.FormOfPayment fop = BuildAncillaryCslFOP(shoppingCart, CombinebilityETCAppliedAncillaryCode, certificate, certificateAmount);
                                if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") ? shoppingCart.Flow != null && shoppingCart.Flow != FlowType.VIEWRES.ToString() : true)
                                {
                                    var ccCombinebilityETCAppliedAncillaryCodeFop = formOfPayment.Find(p => p.PaymentTarget == CombinebilityETCAppliedAncillaryCode && p.Payment.CreditCard != null);
                                    if (ccCombinebilityETCAppliedAncillaryCodeFop != null)
                                    {
                                        ccCombinebilityETCAppliedAncillaryCodeFop.Payment.CreditCard.Amount -= certificateAmount;
                                        ccCombinebilityETCAppliedAncillaryCodeFop.Payment.CreditCard.Amount = Math.Round(ccCombinebilityETCAppliedAncillaryCodeFop.Payment.CreditCard.Amount, 2, MidpointRounding.AwayFromZero);
                                        if (ccCombinebilityETCAppliedAncillaryCodeFop.Payment.CreditCard.Amount == 0)
                                        {
                                            formOfPayment.Remove(ccCombinebilityETCAppliedAncillaryCodeFop);
                                        }
                                    }
                                }
                                //[MOBILE-8683]:  mapp: ETC + CC Seats checkout is failing and we getting Seats unable to assign message on the confirmation screen
                                //Making fix on behalf of csl service team to send the certificate object at the beginning of the list.
                                if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && shoppingCart.Flow == FlowType.VIEWRES.ToString())
                                {

                                    formOfPayment.Insert(i, fop);
                                    i++;
                                }
                                else
                                {
                                    formOfPayment.Add(fop);
                                }
                            }
                        }
                    }
                    if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && shoppingCart.Flow != null && shoppingCart.Flow == FlowType.VIEWRES.ToString())
                        formOfPayment.RemoveAll(x => x.Payment?.CreditCard == null && x.Payment?.Certificate == null);
                }
            }
            #region Building FormofPayment for PCUSeat
            if (isPCUSeat)
            {
                var creditCardFop = formOfPayment.Find(p => p.Payment != null && p.Payment.CreditCard != null);
                #region Building CreditCard fop for Seats not amount covered with certificate
                var seatProduct = shoppingCart.Products.Find(p => p.Code == "SEATASSIGNMENTS");
                if (seatProduct != null && creditCardFop != null)
                {
                    var seatPrice = Convert.ToDouble(seatProduct.ProdTotalPrice) - pcuTotalPrice;
                    var seatPricefop = formOfPayment.Where(p => p.PaymentTarget == "SEATASSIGNMENTS" && p.Payment.Certificate != null);
                    if (seatPricefop != null)
                    {
                        var seatsPriceCoveredwithcert = seatPricefop.Sum(f => f.Amount);
                        if (seatPrice - (double)seatsPriceCoveredwithcert > 0)
                        {
                            Services.FlightShopping.Common.FlightReservation.FormOfPayment seatFop = creditCardFop.Clone();
                            seatFop.Payment.CreditCard.Amount = seatPrice - (double)seatsPriceCoveredwithcert;
                            seatFop.PaymentTarget = "SEATASSIGNMENTS";
                            formOfPayment.Add(seatFop);
                        }
                    }
                }
                #endregion Building CreditCard fop for Seats not amount covered with certificate

                #region Building CreditCard fop for pcu Seats not amount covered with certificate

                var pcuSeatPricefop = formOfPayment.Where(p => p.PaymentTarget == "PCU" && p.Payment.Certificate != null);
                if (creditCardFop != null && pcuSeatPricefop != null)
                {
                    var pcuSeatPriceCoveredwithcert = pcuSeatPricefop.Sum(f => f.Amount);
                    if (pcuTotalPrice - (double)pcuSeatPriceCoveredwithcert > 0)
                    {
                        Services.FlightShopping.Common.FlightReservation.FormOfPayment pcuSeatFop = creditCardFop.Clone();
                        pcuSeatFop.Payment.CreditCard.Amount = pcuTotalPrice - (double)pcuSeatPriceCoveredwithcert;
                        pcuSeatFop.PaymentTarget = "PCU";
                        formOfPayment.Add(pcuSeatFop);
                    }

                }
                #endregion Building CreditCard fop for pcu Seats not amount covered with certificate
                formOfPayment.Remove(formOfPayment.Find(p => p.PaymentTarget == "SEATASSIGNMENTS,PCU"));
            }
            #endregion Building FormofPayment for PCUSeat
        }

        private FormOfPayment BuildAncillaryCslFOP(MOBShoppingCart shoppingCart, string CombinebilityETCAppliedAncillaryCode, MOBFOPCertificate certificate, double certificateAmount)
        {
            var fop = GetCSLFOP(shoppingCart.FormofPaymentDetails, shoppingCart.SCTravelers, certificate);
            fop.Amount = (Math.Round((decimal)certificateAmount, 2));
            fop.Payment.Certificate.Amount = Math.Round(certificateAmount, 2);
            fop.PaymentTarget = CombinebilityETCAppliedAncillaryCode;
            fop.Payment.Certificate.Payor.Surname = certificate.RecipientsLastName;
            fop.Payment.Certificate.Payor.FirstName = certificate.RecipientsFirstName;
            fop.Payment.Certificate.Payor.GivenName = certificate.RecipientsFirstName;
            return fop;
        }

        private async Task<CreditCard> MasterpassPayLoad_CFOP(MasterpassSessionDetails masterpassSession, double paymentAmount, string sessionId, MOBApplication application, string deviceId)
        {
            CreditCard creditCard = new CreditCard();
            creditCard.Payor = new Service.Presentation.PersonModel.Person();

            creditCard.Amount = paymentAmount;
            creditCard.OperationID = sessionId;
            creditCard.AccountNumberToken = masterpassSession.AccountNumberToken;
            creditCard.ExpirationDate = masterpassSession.ExpirationDate;
            creditCard.Code = masterpassSession.Code;
            creditCard.Name = masterpassSession.Name;
            creditCard.Payor.GivenName = masterpassSession.GivenName;
            creditCard.Payor.Surname = masterpassSession.SurName;
            creditCard.Currency = new Service.Presentation.CommonModel.Currency();
            creditCard.Currency.Code = "USD";
            creditCard.Type = new Service.Presentation.CommonModel.Genre();
            creditCard.Type.DefaultIndicator = masterpassSession.MasterpassType.DefaultIndicator;
            creditCard.Type.Description = masterpassSession.MasterpassType.Description;
            creditCard.Type.Key = masterpassSession.MasterpassType.Key;
            creditCard.Type.Value = masterpassSession.MasterpassType.Val;
            creditCard.IsBinRangeValidation = "FALSE";
            creditCard.BillingAddress = AssignCreditCardBillingAddress(masterpassSession.BillingAddress);
            creditCard.CreditCardTypeCode = (United.Service.Presentation.CommonEnumModel.CreditCardTypeCode)masterpassSession.CreditCardTypeCode;
            creditCard.Description = (United.Service.Presentation.CommonEnumModel.CreditCardType)masterpassSession.Description;
            creditCard.Payor.Contact = new Service.Presentation.PersonModel.Contact();
            creditCard.Payor.Contact.Emails = new Collection<EmailAddress> { new EmailAddress { Address = masterpassSession.ContactEmailAddress } };
            //MOBILE-1683/MOBILE-1669/MOBILE-1671: PA,PB,PCU- Masterpass : Richa
            MOBVormetricKeys vormetricKeys = await _paymentUtility.AssignPersistentTokenToCC(masterpassSession.AccountNumberToken, masterpassSession.PersistentToken, string.Empty, masterpassSession.Code, sessionId, "MasterpassPayLoad_CFOP", application.Id, deviceId);
            if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
            {
                creditCard.PersistentToken = vormetricKeys.PersistentToken;
                if (!string.IsNullOrEmpty(vormetricKeys.SecurityCodeToken))
                {
                    creditCard.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                    creditCard.SecurityCode = vormetricKeys.SecurityCodeToken;
                }

                if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(creditCard.Code))
                {
                    creditCard.Code = vormetricKeys.CardType;
                }
            }
            return creditCard;
        }

        private async Task<CreditCard> MapToCslCreditCard(CheckOutRequest checkOutRequest, string currencyCode, Session session, string paymentTarget = "")
        {
            var cslCreditCard = new Service.Presentation.PaymentModel.CreditCard();
            cslCreditCard.Amount = Convert.ToDouble(checkOutRequest.PaymentAmount);
            cslCreditCard.Payor = new Service.Presentation.PersonModel.Person();


            var creditCard = checkOutRequest.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.Uplift.ToString() ? checkOutRequest.FormofPaymentDetails.Uplift : checkOutRequest.FormofPaymentDetails.CreditCard;

            if (!string.IsNullOrEmpty(creditCard.EncryptedCardNumber) && string.IsNullOrEmpty(creditCard.Key) && string.IsNullOrEmpty(creditCard.AccountNumberToken))
            {
                string ccDataVaultToken = string.Empty;
                var tupleResponse = await _profileCreditCard.GenerateCCTokenWithDataVault(creditCard, checkOutRequest.SessionId, session.Token, checkOutRequest.Application, checkOutRequest.DeviceId, ccDataVaultToken);
                ccDataVaultToken = tupleResponse.ccDataVaultToken;
                if (tupleResponse.Item1)
                {
                    cslCreditCard.ExpirationDate = string.Format("{0}/{1}", Convert.ToInt16(creditCard.ExpireMonth).ToString(), creditCard.ExpireYear.Substring(creditCard.ExpireYear.Length - 2));
                    cslCreditCard.Code = creditCard.CardType;
                    cslCreditCard.Name = creditCard.cCName;
                    cslCreditCard.Payor.GivenName = creditCard.cCName;
                    cslCreditCard.AccountNumberToken = ccDataVaultToken;
                    cslCreditCard.SecurityCode = creditCard.cIDCVV2;
                    cslCreditCard.Payor.Contact = new Service.Presentation.PersonModel.Contact();
                    cslCreditCard.Payor.Contact.Emails = new Collection<EmailAddress> { new EmailAddress { Address = checkOutRequest.FormofPaymentDetails.EmailAddress } };
                }
            }
            else
            {
                MOBCreditCard cc = new MOBCreditCard();
                if (string.IsNullOrEmpty(creditCard.AccountNumberToken))
                {
                    MakeReservationRequest makeReservationRequest = new MakeReservationRequest();
                    if (makeReservationRequest.FormOfPayment == null)
                        makeReservationRequest.FormOfPayment = new MOBSHOPFormOfPayment();
                    makeReservationRequest.FormOfPayment.CreditCard = creditCard;
                    cc = await GetUnencryptedCardNumber(session.CartId, session.Token, makeReservationRequest);
                }
                else
                {
                    cc = creditCard;
                }
                cslCreditCard.AccountNumberToken = cc.AccountNumberToken; // CC Token fix - Venkat , Dec 15,2014

                MOBVormetricKeys vormetricKeys = null;
                if (checkOutRequest.Flow == FlowType.BOOKING.ToString() || checkOutRequest.Flow == FlowType.RESHOP.ToString() || ShopStaticUtility.IsCheckinFlow(checkOutRequest.Flow))
                    vormetricKeys = await _paymentUtility.GetVormetricPersistentTokenForBooking(creditCard, session.SessionId, session.Token);
                else if (checkOutRequest.Flow == FlowType.POSTBOOKING.ToString() || checkOutRequest.Flow == FlowType.VIEWRES.ToString())
                    vormetricKeys = await _paymentUtility.GetVormetricPersistentTokenForViewRes(creditCard, session.SessionId, session.Token);
                else if (_configuration.GetValue<bool>("LoadVIewResVormetricForVIEWRES_SEATMAPFlowToggle"))
                {
                    vormetricKeys = await _paymentUtility.GetVormetricPersistentTokenForViewRes(creditCard, session.SessionId, session.Token);
                }

                if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                {
                    cslCreditCard.PersistentToken = vormetricKeys.PersistentToken;
                    if (!string.IsNullOrEmpty(vormetricKeys.SecurityCodeToken))
                    {
                        cslCreditCard.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                        cslCreditCard.SecurityCode = vormetricKeys.SecurityCodeToken;
                    }
                    if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cslCreditCard.CardType))
                    {
                        cslCreditCard.CardType = vormetricKeys.CardType;
                    }
                    //Mobile AppMOBILE-1813 Booking – BE purchase – Choose OTP – CC payment
                    creditCard.PersistentToken = vormetricKeys.PersistentToken;
                    creditCard.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                    creditCard.CardType = vormetricKeys.CardType;
                }
                cslCreditCard.ExpirationDate = string.Format("{0}/{1}", Convert.ToInt16(cc.ExpireMonth).ToString(), cc.ExpireYear.Substring(cc.ExpireYear.Length - 2));
                cslCreditCard.Code = cc.CardType;
                cslCreditCard.Name = !string.IsNullOrEmpty(creditCard.cCName) ? creditCard.cCName : cc.cCName;
                cslCreditCard.Payor.GivenName = !string.IsNullOrEmpty(creditCard.cCName) ? creditCard.cCName : cc.cCName;
                cslCreditCard.Payor.Contact = new Service.Presentation.PersonModel.Contact();
                cslCreditCard.Payor.Contact.Emails = new Collection<EmailAddress> { new EmailAddress { Address = checkOutRequest.FormofPaymentDetails.EmailAddress } };
            }
            cslCreditCard.Currency = new Service.Presentation.CommonModel.Currency();
            cslCreditCard.Currency.Code = string.IsNullOrEmpty(currencyCode) ? "USD" : currencyCode;

            if (_configuration.GetValue<bool>("EDDtoEMDToggle"))
            {
                cslCreditCard.OperationID = Guid.NewGuid().ToString(); // This one we can pass the session id which we using in bookign path.
            }
            else
            {
                cslCreditCard.OperationID = Guid.NewGuid().ToString().ToUpper().Replace("-", ""); //As Per Aruna operation ID is a unique GUID generated per transactions so I assgined booking session ID as its unique per booking session - Venakt Dec 19 2014
            }

            if (checkOutRequest.FormofPaymentDetails.FormOfPaymentType == MOBFormofPayment.Uplift.ToString())
            {
                cslCreditCard.PaymentCharacteristics = new Collection<Characteristic> { new Characteristic { Code = "IsUplift", Value = "true" } };
            }

            if (checkOutRequest.FormofPaymentDetails.BillingAddress != null)
            {
                cslCreditCard.BillingAddress = new Service.Presentation.CommonModel.Address();
                cslCreditCard.BillingAddress.AddressLines = new Collection<string>();
                cslCreditCard.BillingAddress.AddressLines.Add(checkOutRequest.FormofPaymentDetails.BillingAddress.Line1.Length > 35 ? checkOutRequest.FormofPaymentDetails.BillingAddress.Line1.Substring(0, 35) : checkOutRequest.FormofPaymentDetails.BillingAddress.Line1);
                cslCreditCard.BillingAddress.City = checkOutRequest.FormofPaymentDetails.BillingAddress.City;
                cslCreditCard.BillingAddress.StateProvince = new Service.Presentation.CommonModel.StateProvince();
                cslCreditCard.BillingAddress.StateProvince.ShortName = checkOutRequest.FormofPaymentDetails.BillingAddress.State.Code;
                // Added as part of Bug 87614:Booking: PNR TAE field does not contain the State : Issuf
                cslCreditCard.BillingAddress.StateProvince.StateProvinceCode = checkOutRequest.FormofPaymentDetails.BillingAddress.State.Code;
                cslCreditCard.BillingAddress.PostalCode = checkOutRequest.FormofPaymentDetails.BillingAddress.PostalCode;
                cslCreditCard.BillingAddress.Country = new Service.Presentation.CommonModel.Country();
                cslCreditCard.BillingAddress.Country.CountryCode = checkOutRequest.FormofPaymentDetails.BillingAddress.Country.Code;
            }
            cslCreditCard.AccountNumberLastFourDigits = (ConfigUtility.IsPOMOffer(paymentTarget) && !_configuration.GetValue<bool>("DisableLast4DigitsForPaymentCreditCard")) ? GetLast4DigitsOfCreditCard(creditCard) : null;
            return cslCreditCard;
        }

        private async Task<MOBCreditCard> GetUnencryptedCardNumber(string cartId, string token, MakeReservationRequest makeReservationRequest)
        {
            MOBCPProfileRequest request = new MOBCPProfileRequest();
            request.AccessCode = makeReservationRequest.AccessCode;
            request.Application = makeReservationRequest.Application;
            request.TransactionId = makeReservationRequest.TransactionId;
            request.LanguageCode = makeReservationRequest.LanguageCode;
            request.DeviceId = makeReservationRequest.DeviceId;
            request.ProfileOwnerOnly = true;
            request.IncludeCreditCards = true;
            request.SessionId = makeReservationRequest.SessionId;
            request.MileagePlusNumber = makeReservationRequest.MileagePlusNumber;
            request.CartId = cartId;
            request.Token = token;

            MOBCreditCard cc = await GetCreditCardWithKey(request, makeReservationRequest.FormOfPayment.CreditCard.Key);
            if (cc == null)
            {
                throw new MOBUnitedException("The credit card with specified key was not found.");
            }
            else
                return cc;
        }

        private async Task<MOBCreditCard> GetCreditCardWithKey(MOBCPProfileRequest request, string creditCardKey)
        {
            MOBCreditCard creditCardDetails = new MOBCreditCard();
            if (request == null)
            {
                throw new MOBUnitedException("Profile request cannot be null.");
            }
            List<MOBCPProfile> profiles = null;

            United.Services.Customer.Common.ProfileRequest profileRequest = new Profile.ProfileRequest(_configuration, false).GetProfileRequest(request);
            string jsonRequest = JsonConvert.SerializeObject(profileRequest);

            var jsonResponse = await _customerDataService.GetCustomerData<Services.Customer.Common.ProfileResponse>(request.Token, request.SessionId, jsonRequest).ConfigureAwait(false);

            if (jsonResponse.response != null)
            {
                if (jsonResponse.response != null && jsonResponse.response.Status.Equals(United.Services.Customer.Common.Constants.StatusType.Success) && jsonResponse.response.Profiles != null)
                {
                    creditCardDetails = await PopulateCreditCardDetails(request.SessionId, request.MileagePlusNumber, request.CustomerId, jsonResponse.response.Profiles, creditCardKey, request);
                }
                else
                {
                    if (jsonResponse.response.Errors != null && jsonResponse.response.Errors.Count > 0)
                    {
                        string errorMessage = string.Empty;
                        foreach (var error in jsonResponse.response.Errors)
                        {
                            errorMessage = errorMessage + " " + error.UserFriendlyMessage;
                        }

                        throw new MOBUnitedException(errorMessage);
                    }
                    else
                    {
                        throw new MOBUnitedException("Unable to get Credit Card from profile.");//**// Get approved error message for this message.
                    }
                }
            }
            else
            {
                throw new MOBUnitedException("Unable to get Credit Card details from profile.");//**// Get approved error message for this message.
            }

            _logger.LogInformation("GetProfile -Client Response for GetCreditCardWithKey {Response} and sessionId", profiles, request.SessionId);

            return creditCardDetails;
        }

        private async Task<MOBCreditCard> PopulateCreditCardDetails(string sessionId, string mileagePlusNumber, int customerId, List<United.Services.Customer.Common.Profile> profiles, string creditCardKey, MOBCPProfileRequest request)
        {
            MOBCreditCard creditCard = null;
            if (profiles != null && profiles.Count > 0)
            {
                foreach (var profile in profiles)
                {
                    MOBCPProfile mobProfile = new MOBCPProfile();
                    bool isProfileOwnerTSAFlagOn = false;
                    List<MOBKVP> mpList = new List<MOBKVP>();
                    var tupleRes =await _mPTraveler.PopulateTravelers(profile.Travelers, mileagePlusNumber, isProfileOwnerTSAFlagOn, true, request, sessionId);
                    mobProfile.Travelers = tupleRes.mobTravelersOwnerFirstInList;
                    isProfileOwnerTSAFlagOn = tupleRes.isProfileOwnerTSAFlagOn;
                    mpList = tupleRes.savedTravelersMPList;
                    foreach (MOBCPTraveler traveler in mobProfile.Travelers)
                    {
                        if (traveler.CreditCards != null && traveler.CreditCards.Count > 0)
                        {
                            foreach (MOBCreditCard cc in traveler.CreditCards)
                            {
                                if (cc.Key.Trim() == creditCardKey.Trim())
                                {
                                    creditCard = cc;
                                    return cc;
                                }
                            }
                        }
                    }
                }
            }
            return creditCard;
        }

        private static string GetLast4DigitsOfCreditCard(MOBCreditCard creditCard)
        {
            string cardDisplayNumber = creditCard?.DisplayCardNumber;
            if (!string.IsNullOrEmpty(cardDisplayNumber) && cardDisplayNumber.Length > 3)
                return cardDisplayNumber.Substring(cardDisplayNumber.Length - 4);
            return string.Empty;
        }

        private async Task<List<UpgradeOption>> GetConfirmedUpgradeProducts
            (string sessionid, FlightReservationResponse cslresponse)
        {
            try
            {
                var requestedupgradeitem
                    = await _sessionHelperService.GetSession<List<UpgradeOption>>
                    (sessionid, typeof(List<UpgradeOption>).FullName, new List<string> { sessionid, typeof(List<UpgradeOption>).FullName }).ConfigureAwait(false); //change session

                var confimedupgradeitems = new List<UpgradeOption>();

                if (cslresponse.CheckoutResponse != null
                    && cslresponse.CheckoutResponse.ShoppingCartResponse != null
                    && cslresponse.CheckoutResponse.ShoppingCartResponse.UpgradeDetails != null
                    && cslresponse.CheckoutResponse.ShoppingCartResponse.UpgradeDetails.Any())
                {
                    requestedupgradeitem.ForEach(item =>
                    {
                        bool isSuccess = false;

                        if (string.Equals(item.UpgradeType, _strMUA, StringComparison.OrdinalIgnoreCase))
                        {
                            isSuccess = cslresponse.CheckoutResponse.ShoppingCartResponse.UpgradeDetails
                            .Any(x => string.Equals(Convert.ToString(x.Number), item.TripRefId, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(x.Type, _strSUCCESS, StringComparison.OrdinalIgnoreCase));
                        }
                        else if (string.Equals(item.UpgradeType, _strPCU, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(item.UpgradeType, _strUGC, StringComparison.OrdinalIgnoreCase))
                        {
                            isSuccess = cslresponse.CheckoutResponse.ShoppingCartResponse.UpgradeDetails
                            .Any(x => string.Equals(Convert.ToString(x.Number), item.TripRefId, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(Convert.ToString(x.Flight?.SegmentNumber), item.SegmentRefId, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(x.Type, _strSUCCESS, StringComparison.OrdinalIgnoreCase));
                        }
                        if (isSuccess)
                            confimedupgradeitems.Add(item);
                    });
                }

                return confimedupgradeitems.Any() ? confimedupgradeitems : null;
            }
            catch { return null; }
        }

        private string GetCheckOutEmail(CheckOutRequest checkOutRequest, FlightReservationResponse flightReservationResponse)
        {
            return (!checkOutRequest.FormofPaymentDetails.IsNullOrEmpty() && !string.IsNullOrEmpty(checkOutRequest.FormofPaymentDetails.EmailAddress)) ? checkOutRequest.FormofPaymentDetails.EmailAddress.ToString()
                       : (!flightReservationResponse.Reservation.IsNullOrEmpty() && !flightReservationResponse.Reservation.EmailAddress.IsNullOrEmpty() && flightReservationResponse.Reservation.EmailAddress.Count() > 0) ? flightReservationResponse.Reservation.EmailAddress.Where(x => x.Address != null).Select(x => x.Address).FirstOrDefault().ToString() : null;
        }

        private bool IsUpgradePartialSuccessUPGRADEMALL(string flow, List<Services.FlightShopping.Common.ErrorInfo> warnings)
        {
            return string.Equals(flow, _UPGRADEMALL, StringComparison.OrdinalIgnoreCase) &&
                   (warnings?.Any(x => x?.Message?.IndexOf(_strPARTIALFAILURE, StringComparison.OrdinalIgnoreCase) > -1) ?? false);
        }

        private async System.Threading.Tasks.Task GenerateTPISecondaryPaymentInfoFOP(CheckOutRequest checkOutRequest, CheckOutResponse checkOutResponse, FlightReservationResponse flightReservationResponse, MOBShoppingCart persistedShoppingCart)
        {
            if (persistedShoppingCart != null && persistedShoppingCart.Products.Any(x => x.Code == "TPI") &&
                flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Any(x => x.Item.Product[0].Code == "TPI") &&
                flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Where(x => x.Item.Product[0].Code == "TPI").Select(x => x.Item).Any(y => y.Status.Contains("FAILED"))
                && !ValidateFormOfPaymentTypeCFOP(checkOutRequest.FormofPaymentDetails))
            {
                checkOutResponse.IsTPIFailed = true;

                if (flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => x.Product[0].Code != "RES").Count() == 1)
                {
                    checkOutResponse.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
                else
                {
                    MOBSection alertMsg = await GetTPIAlertMessage(false);

                    if (checkOutResponse.ShoppingCart == null)
                        checkOutResponse.ShoppingCart = new MOBShoppingCart();

                    checkOutResponse.ShoppingCart.Products = new List<ProdDetail> { new ProdDetail {
                        Code = "TPI",
                        ProdDescription = "Travel Insurance",
                        Segments = null,
                        TermsAndCondition = new MOBMobileCMSContentMessages { Title = alertMsg.Text1, ContentFull = alertMsg.Text2  }
                    } };
                    checkOutResponse.ShoppingCart.TotalPrice = String.Format("{0:0.00}", flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => x.Product[0].Code == "TPI" && x.Product[0].Price != null && x.Product[0].Price.Totals != null).Select(x => x.Product[0].Price.Totals[0].Amount).FirstOrDefault().ToString());
                    checkOutResponse.ShoppingCart.DisplayTotalPrice = Decimal.Parse(flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => x.Product[0].Code == "TPI" && x.Product[0].Price != null && x.Product[0].Price.Totals != null).Select(x => x.Product[0].Price.Totals[0].Amount).FirstOrDefault().ToString()).ToString("c");
                    checkOutResponse.PostPurchasePage = PostPurchasePage.SecondaryFormOfPayment.ToString();
                    checkOutResponse.EnabledSecondaryFormofPayment = true;
                }
            }
        }

        private async Task<MOBSection> GetTPIAlertMessage(bool isPurchaseMessage)
        {
            var tPIMessageKey = isPurchaseMessage ? "TPI_PURCHASE_FAILED_MESSAGE"
                                                      : "TPI_BILLED_SEPARATE_TEXT";

            List<MOBItem> alertMsgDB = await GetMessagesFromDb(tPIMessageKey);
            return AssignAlertMessage(alertMsgDB);
        }

        private async Task<List<MOBItem>> GetMessagesFromDb(string seatMessageKey)
        {
            return await (seatMessageKey.IsNullOrEmpty()
                    ? null
                    : new MPDynamoDB(_configuration, _dynamoDBService,null,_headers). GetMPPINPWDTitleMessages(new List<string> { seatMessageKey }));
        }

        private MOBSection AssignAlertMessage(List<MOBItem> seatAssignmentMessage)
        {
            MOBSection alertMsg = new MOBSection() { };
            if (seatAssignmentMessage != null && seatAssignmentMessage.Count > 0)
            {
                foreach (var msg in seatAssignmentMessage)
                {
                    if (msg != null)
                    {
                        switch (msg.Id.ToUpper())
                        {
                            case "HEADER":
                                alertMsg.Text1 = msg.CurrentValue.Trim();
                                break;
                            case "BODY":
                                alertMsg.Text2 = msg.CurrentValue.Trim();
                                break;
                            case "FOOTER":
                                alertMsg.Text3 = msg.CurrentValue.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return alertMsg;
        }

        private bool ValidateFormOfPaymentTypeCFOP(MOBFormofPaymentDetails formOfPayment)
        {
            return formOfPayment != null && formOfPayment.FormOfPaymentType.ToUpper() == MOBFormofPayment.CreditCard.ToString().ToUpper() && formOfPayment.CreditCard != null && !string.IsNullOrEmpty(formOfPayment.CreditCard.CardType) &&
                                    _profileCreditCard.IsValidFOPForTPIpayment(formOfPayment.CreditCard.CardType);
        }

        private PayPal PayPalPayLoad(MOBPayPalPayor payPalPayor, double paymentAmount, MOBPayPal mobPayPal)
        {
            PayPal paypal = new PayPal();
            paypal.Amount = paymentAmount;

            paypal.BillingAddress = AssignCreditCardBillingAddress(payPalPayor.PayPalBillingAddress);
            paypal.BillingAddress.Characteristic = new Collection<Characteristic>();
            paypal.BillingAddress.Status = new Service.Presentation.CommonModel.Status();
            paypal.BillingAddress.Status.Description = "NONE"; //**TBD-paypal**//
            paypal.Currency = new Currency();
            paypal.Currency.Code = mobPayPal.CurrencyCode;
            paypal.PayerID = mobPayPal.PayerID;
            paypal.TokenID = mobPayPal.PayPalTokenID;
            paypal.Payor = new Service.Presentation.PersonModel.Person();
            paypal.Payor.Contact = new Service.Presentation.PersonModel.Contact();
            paypal.Payor.Contact.Emails = new Collection<EmailAddress>();
            EmailAddress email = new EmailAddress();
            email.Address = payPalPayor.PayPalContactEmailAddress;
            paypal.Payor.Contact.Emails.Add(email);
            paypal.Payor.CustomerID = payPalPayor.PayPalCustomerID;
            paypal.Payor.GivenName = payPalPayor.PayPalGivenName;
            paypal.Payor.Surname = payPalPayor.PayPalSurName;
            paypal.Payor.Status = new Service.Presentation.CommonModel.Status();
            paypal.Payor.Status.Description = payPalPayor.PayPalStatus;
            paypal.Type = new Service.Presentation.CommonModel.Genre();
            paypal.Type.Key = "PP"; // check if we need to move this to web config.

            return paypal;
        }

    }
}
