using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.CMSContent;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Utility.Helper;
using MOBFOPTravelCredit = United.Mobile.Model.Shopping.FormofPayment.MOBFOPTravelCredit;
using Reservation = United.Mobile.Model.Shopping.Reservation;

namespace United.Common.Helper.Shopping
{
    public class FFCShopping : IFFCShoppingcs
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<FFCShopping> _logger;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ICMSContentService _cMSContentService;
        private readonly ICachingService _cachingService;

        
        public FFCShopping(IConfiguration configuration
            , ICacheLog<FFCShopping> logger
            , ISessionHelperService sessionHelperService
            , ICMSContentService cMSContentService
            , ICachingService cachingService)
        {
            _configuration = configuration;
            _logger = logger;
            _sessionHelperService = sessionHelperService;
            _cMSContentService = cMSContentService;
            _cachingService = cachingService;

        }

        private bool IncludeFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }
        private bool IncludeMOBILE12570ResidualFix(int appId, string appVersion)
        {
            bool isApplicationGreater = GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidMOBILE12570ResidualVersion", "iPhoneMOBILE12570ResidualVersion", "", "", true, _configuration);
            return (_configuration.GetValue<bool>("eableMOBILE12570Toggle") && isApplicationGreater);
        }

        public async Task AssignFFCValues(string sessionid, MOBShoppingCart shoppingCart, MOBRequest request, MOBFormofPaymentDetails formOfPaymentDetails, MOBSHOPReservation reservation)
        {
            if (IncludeFFCResidual(request.Application.Id, request.Application.Version.Major))
            {
                var futureFlightCreditResponse = _sessionHelperService.GetSession<FutureFlightCreditResponse>(sessionid, new FutureFlightCreditResponse().ObjectName, new List<string> { sessionid, new FutureFlightCreditResponse().ObjectName });
                if (shoppingCart.FormofPaymentDetails != null)
                {
                    formOfPaymentDetails.TravelFutureFlightCredit = shoppingCart.FormofPaymentDetails.TravelFutureFlightCredit;
                    formOfPaymentDetails.FormOfPaymentType = shoppingCart.FormofPaymentDetails.FormOfPaymentType;
                }
                if (formOfPaymentDetails.TravelFutureFlightCredit == null)
                    formOfPaymentDetails.TravelFutureFlightCredit = new FOPTravelFutureFlightCredit();

                var session =await _sessionHelperService.GetSession<Session>(sessionid, new Session().ObjectName, new List<string> { sessionid, new Session().ObjectName }).ConfigureAwait(false);

                List<CMSContentMessage> lstMessages = await GetSDLContentByGroupName(request, sessionid, session?.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_BookingRTI_Messages"), "BookingPathRTI_CMSContentMessagesCached_StaticGUID");
                formOfPaymentDetails.TravelFutureFlightCredit.LookUpFFCMessages = ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.LookupFFC");
                formOfPaymentDetails.TravelFutureFlightCredit.LookUpFFCMessages.AddRange(ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.LookupFFC.BtnText"));
                formOfPaymentDetails.TravelFutureFlightCredit.FindFFCMessages = ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.FindFFC");
                formOfPaymentDetails.TravelFutureFlightCredit.FindFFCMessages.AddRange(ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.FindFFC.BtnText"));
                //formOfPaymentDetails.TravelFutureFlightCredit.EmailConfirmationFFCMessages = AssignEmailMessageForFFCRefund(lstMessages, reservation.Prices, formOfPaymentDetails.EmailAddress);

                if (shoppingCart.FormofPaymentDetails?.TravelFutureFlightCredit?.FutureFlightCredits?.Count > 0)
                {
                    string email = (string.IsNullOrEmpty(formOfPaymentDetails.EmailAddress) ? shoppingCart.FormofPaymentDetails?.EmailAddress : formOfPaymentDetails.EmailAddress);
                    formOfPaymentDetails.TravelFutureFlightCredit.EmailConfirmationFFCMessages = AssignEmailMessageForFFCRefund(lstMessages, reservation.Prices, email, formOfPaymentDetails.TravelFutureFlightCredit, request.Application);
                    ShopStaticUtility.AddGrandTotalIfNotExistInPricesAndUpdateCertificateValue(reservation.Prices, formOfPaymentDetails);
                    bool isAncillaryFFCEnable =ConfigUtility.IsIncludeWithThisToggle(request.Application.Id, request.Application.Version.Major, "EnableTravelCreditAncillary", "AndroidTravelCreditVersionAncillary", "iPhoneTravelCreditVersionAncillary");
                    formOfPaymentDetails.TravelFutureFlightCredit.AllowedFFCAmount = GetAllowedFFCAmount(shoppingCart.Products, isAncillaryFFCEnable);                  
                    Reservation bookingPathReservation = new Reservation();
                    bookingPathReservation =await _sessionHelperService.GetSession<Reservation>(sessionid, bookingPathReservation.ObjectName, new List<string> { sessionid, bookingPathReservation.ObjectName }).ConfigureAwait(false);

                    bool isDirty = UpdateFFCAmountAsPerChangedPrice(formOfPaymentDetails.TravelFutureFlightCredit, reservation.TravelersCSL, sessionid);
                    if (isDirty)
                    {
                        if (bookingPathReservation.TravelersCSL != null && bookingPathReservation.TravelersCSL.Count > 0)
                        {
                            bookingPathReservation.TravelersCSL = new SerializableDictionary<string, MOBCPTraveler>();
                            foreach (var travelerCSL in reservation.TravelersCSL)
                            {
                                bookingPathReservation.TravelersCSL.Add(travelerCSL.PaxIndex.ToString(), travelerCSL);
                            }
                        }
                    }
                    UpdatePricesInReservation(formOfPaymentDetails.TravelFutureFlightCredit, reservation.Prices);
                    bookingPathReservation.Prices = reservation.Prices;
                   await _sessionHelperService.SaveSession<Reservation>(bookingPathReservation, sessionid, new List<string> { sessionid, bookingPathReservation.ObjectName }, bookingPathReservation.ObjectName).ConfigureAwait(false);

                    AssignIsOtherFOPRequired(formOfPaymentDetails, reservation.Prices);
                    AssignFormOfPaymentType(formOfPaymentDetails, reservation.Prices, formOfPaymentDetails?.SecondaryCreditCard != null);
                }
            }
        }
        public void AssignNullToETCAndFFCCertificates(MOBFormofPaymentDetails fopDetails, MOBRequest request)
        {
            if (!_configuration.GetValue<bool>("disableSFOPClearFFCAndETCCertificatesToggle") && fopDetails.SecondaryCreditCard != null)
            {
                if (fopDetails?.TravelCertificate?.Certificates?.Count() > 0)
                {
                    fopDetails.TravelCertificate.Certificates = null;
                }
                if (fopDetails?.TravelFutureFlightCredit?.FutureFlightCredits?.Count() > 0)
                {
                    fopDetails.TravelFutureFlightCredit.FutureFlightCredits = null;
                }
                //After checkout empty moneymiles data from the shoppingcart.
                if (IncludeMoneyPlusMilesV2(request.Application.Id, request.Application.Version.Major))
                {
                    fopDetails.MoneyPlusMilesCredit = null;
                }
                if (IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                {
                    fopDetails.TravelBankDetails = null;
                }
            }
        }

        public void AssignFormOfPaymentType(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, bool IsSecondaryFOP = false, bool isRemoveAll = false)
        {
            //need to update only when TravelFutureFlightCredit is added as formofpayment.          
            if (formofPaymentDetails?.TravelFutureFlightCredit?.FutureFlightCredits?.Count() > 0 || isRemoveAll)
            {
                if (!formofPaymentDetails.IsOtherFOPRequired && !IsSecondaryFOP)
                {
                    formofPaymentDetails.FormOfPaymentType = MOBFormofPayment.FFC.ToString();
                    if (!string.IsNullOrEmpty(formofPaymentDetails.CreditCard?.Message) &&
                        _configuration.GetValue<string>("CreditCardDateExpiredMessage").IndexOf(formofPaymentDetails.CreditCard?.Message, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        formofPaymentDetails.CreditCard = null;
                    }
                }
                else
                {
                    formofPaymentDetails.FormOfPaymentType = MOBFormofPayment.CreditCard.ToString();
                }
            }
        }

        public void AssignIsOtherFOPRequired(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices)
        {
            var grandTotalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL"));
            if (grandTotalPrice != null)
                formofPaymentDetails.IsOtherFOPRequired = (grandTotalPrice.Value > 0);
        }

        public void UpdatePricesInReservation(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBSHOPPrice> prices)
        {

            var ffcPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "FFC");
            var totalCreditFFC = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "REFUNDPRICE");
            //var scRESProduct = response.ShoppingCart.Products.Find(p => p.Code == "RES");
            var grandtotal = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "GRAND TOTAL");

            if (ffcPrice == null && travelFutureFlightCredit.TotalRedeemAmount > 0)
            {
                ffcPrice = new MOBSHOPPrice();
                prices.Add(ffcPrice);
            }
            else if (ffcPrice != null)
            {
                ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, ffcPrice.Value, false);
            }

            if (totalCreditFFC != null)
                prices.Remove(totalCreditFFC);

            if (travelFutureFlightCredit.TotalRedeemAmount > 0)
            {
                UpdateCertificatePrice(ffcPrice, travelFutureFlightCredit.TotalRedeemAmount, "FFC", "Future Flight Credit", isAddNegative: true);
                //Build Total Credit item
                double totalCreditValue = travelFutureFlightCredit.FutureFlightCredits.Sum(ffc => ffc.NewValueAfterRedeem);
                if (totalCreditValue > 0)
                {
                    totalCreditFFC = new MOBSHOPPrice();
                    prices.Add(totalCreditFFC);
                    UpdateCertificatePrice(totalCreditFFC, totalCreditValue, "REFUNDPRICE", "Total Credit", "RESIDUALCREDIT");
                }
                ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, travelFutureFlightCredit.TotalRedeemAmount);
            }
            else
            {
                prices.Remove(ffcPrice);
            }
        }

        public MOBSHOPPrice UpdateCertificatePrice(MOBSHOPPrice ffc, double totalAmount, string priceType, string priceTypeDescription, string status = "", bool isAddNegative = false)
        {
            ffc.CurrencyCode = "USD";
            ffc.DisplayType = priceType;
            ffc.PriceType = priceType;
            ffc.Status = status;
            ffc.PriceTypeDescription = priceTypeDescription;
            ffc.Value = totalAmount;
            ffc.Value = Math.Round(ffc.Value, 2, MidpointRounding.AwayFromZero);
            ffc.FormattedDisplayValue = (isAddNegative ? "-" : "") + (ffc.Value).ToString("C2", CultureInfo.CurrentCulture);
            ffc.DisplayValue = string.Format("{0:#,0.00}", ffc.Value);
            return ffc;
        }

        public void AssignTravelerTotalFFCNewValueAfterReDeem(MOBCPTraveler traveler)
        {
            if (traveler.FutureFlightCredits?.Count > 0)
            {
                var sumOfNewValueAfterRedeem = traveler.FutureFlightCredits.Sum(ffc => ffc.NewValueAfterRedeem);
                if (sumOfNewValueAfterRedeem > 0)
                {
                    sumOfNewValueAfterRedeem = Math.Round(sumOfNewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                    traveler.TotalFFCNewValueAfterRedeem = (sumOfNewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);
                }
                else
                {
                    traveler.TotalFFCNewValueAfterRedeem = "";
                }
            }
            else
            {
                traveler.TotalFFCNewValueAfterRedeem = "";
            }
        }

        public List<MOBMobileCMSContentMessages> AssignEmailMessageForFFCRefund(List<CMSContentMessage> lstMessages, List<MOBSHOPPrice> prices, string email, FOPTravelFutureFlightCredit futureFlightCredit, MOBApplication application)
        {
            List<MOBMobileCMSContentMessages> ffcHeaderMessage = null;
            if (prices.Exists(p => p.DisplayType.ToUpper() == "REFUNDPRICE"))
            {
                ffcHeaderMessage = ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.EmailConfirmation");
                if (IncludeMOBILE12570ResidualFix(application.Id, application.Version.Major))
                {
                    ffcHeaderMessage[0].ContentFull = "";
                    ffcHeaderMessage[0].ContentShort = "";
                }
                else
                {
                    ffcHeaderMessage[0].ContentFull = string.Format(ffcHeaderMessage[0].ContentFull, email);
                    ffcHeaderMessage[0].ContentShort = ffcHeaderMessage[0].ContentFull;
                }
            }
            return ffcHeaderMessage;
        }
        public bool UpdateFFCAmountAsPerChangedPrice(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBCPTraveler> travelersCSL, string sessionid)
        {
            bool isDirty = false;
            foreach (var traveler in travelersCSL)
            {
                var travelerFFCs = traveler.FutureFlightCredits;
                traveler.IndividualTotalAmount = Math.Round(traveler.IndividualTotalAmount, 2, MidpointRounding.AwayFromZero);
                if (travelerFFCs != null &&
                    travelerFFCs.Count > 0 &&
                    ((travelerFFCs.Sum(ffc => ffc.NewValueAfterRedeem) > 0)
                        ||
                        (!_configuration.GetValue<bool>("disable21GFFCToggle") && travelerFFCs.Sum(ffc => ffc.CurrentValue) > traveler.IndividualTotalAmount)
                    ) &&
                    travelerFFCs.Sum(ffc => ffc.RedeemAmount) != traveler.IndividualTotalAmount)
                {
                    isDirty = true;

                    double travelerIndividualTotalAmount = traveler.IndividualTotalAmount;
                    double ffcAppliedToTraveler = 0;
                    foreach (var travelerFFC in travelerFFCs)
                    {
                        if (travelerIndividualTotalAmount > 0)
                        {
                            travelerFFC.RedeemAmount = travelerIndividualTotalAmount > travelerFFC.CurrentValue ? travelerFFC.CurrentValue : travelerIndividualTotalAmount;
                            travelerFFC.RedeemAmount = Math.Round(travelerFFC.RedeemAmount, 2, MidpointRounding.AwayFromZero);
                            travelerFFC.DisplayRedeemAmount = (travelerFFC.RedeemAmount).ToString("C2", CultureInfo.CurrentCulture);
                            travelerFFC.NewValueAfterRedeem = travelerFFC.CurrentValue - travelerFFC.RedeemAmount;
                            travelerFFC.NewValueAfterRedeem = Math.Round(travelerFFC.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                            travelerFFC.DisplayNewValueAfterRedeem = (travelerFFC.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);

                            travelerIndividualTotalAmount -= travelerFFC.RedeemAmount;
                            ffcAppliedToTraveler += travelerFFC.RedeemAmount;
                        }
                    }
                    travelerFFCs.RemoveAll(ffc => ffc.RedeemAmount == 0);
                    travelFutureFlightCredit.FutureFlightCredits.RemoveAll(ffc => (ffc.TravelerNameIndex == traveler.TravelerNameIndex && ffc.PaxId == traveler.PaxID));
                    if (travelerFFCs != null)
                    {
                        travelFutureFlightCredit.FutureFlightCredits.AddRange(travelerFFCs);
                        AssignTravelerTotalFFCNewValueAfterReDeem(traveler);
                    }
                }
            }
            return isDirty;
        }
        public async  Task<List<CMSContentMessage>> GetSDLContentByGroupName(MOBRequest request, string sessionId, string token, string groupName, string docNameConfigEntry, bool useCache = false)
        {
            CSLContentMessagesResponse response = null;

            try
            {
                var getSDL = await _cachingService.GetCache<string>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", request.TransactionId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(getSDL))
                {
                    response = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(getSDL);
                }
                if (response != null && response.Messages != null) { return response.Messages; }
            }
            catch { }

            MOBCSLContentMessagesRequest sdlReqeust = new MOBCSLContentMessagesRequest
            {
                Lang = "en",
                Pos = "us",
                Channel = "mobileapp",
                Listname = new List<string>(),
                LocationCodes = new List<string>(),
                Groupname = groupName,
                Usecache = useCache
            };

            string jsonRequest = JsonConvert.SerializeObject(sdlReqeust);

            response =await _cMSContentService.GetSDLContentByGroupName<CSLContentMessagesResponse>(token, "message", jsonRequest, sessionId).ConfigureAwait(false);

            if (response == null)
            {
                _logger.LogError("GetSDLContentByGroupName Failed to deserialize CSL response");
                return null;
            }

            if (response.Errors.Count > 0)
            {
                string errorMsg = String.Join(" ", response.Errors.Select(x => x.Message));
                _logger.LogError("GetSDLContentByGroupName {@CSLCallError}", errorMsg);
                return null;
            }

            if (response != null && (Convert.ToBoolean(response.Status) && response.Messages != null))
            {
                if (!_configuration.GetValue<bool>("DisableSDLEmptyTitleFix"))
                {
                    response.Messages = response.Messages.Where(l => l.Title != null).ToList();
                }
                var saveSDL = await _cachingService.SaveCache<CSLContentMessagesResponse>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", response, request.TransactionId, new TimeSpan(1, 30, 0)).ConfigureAwait(false);

            }

            return response.Messages;
        }

        public void UpdateTravelCreditAmountWithSelectedETCOrFFC(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, List<MOBCPTraveler> travelers)
        {
            if (formofPaymentDetails?.TravelCreditDetails?.TravelCredits?.Count > 0)
            {
                bool isETC = (formofPaymentDetails?.TravelCertificate?.Certificates?.Count > 0);
                bool isFFC = (formofPaymentDetails?.TravelFutureFlightCredit?.FutureFlightCredits?.Count > 0);
                bool isTravelerFFC_Check = !_configuration.GetValue<bool>("DisableMPSignedInInsertUpdateTraveler");

                foreach (var travelCredit in formofPaymentDetails.TravelCreditDetails.TravelCredits)
                {
                    double redeemAmount = 0;
                    if (isETC)
                    {
                        var cert = formofPaymentDetails.TravelCertificate.Certificates.Where(c => c.PinCode == travelCredit.PinCode).ToList();
                        if (cert != null)
                        {
                            redeemAmount = cert.Sum(c => c.RedeemAmount);
                        }
                    }

                    if (isFFC)
                    {
                        var ffcs = formofPaymentDetails.TravelFutureFlightCredit.FutureFlightCredits.Where(c => c.PinCode == travelCredit.PinCode).ToList();
                        if (ffcs != null)
                        {
                            redeemAmount = ffcs.Sum(c => c.RedeemAmount);
                        }
                    }
                    UpdateTravelCreditRedeemAmount(travelCredit, redeemAmount);
                }
                if (isFFC)
                {
                    IEnumerable<MOBFOPTravelCredit> ancillaryTCs = formofPaymentDetails.TravelCreditDetails.TravelCredits.Where(tc => tc.IsApplied);
                    foreach (var ancillaryTC in ancillaryTCs)
                    {
                        foreach (var scTraveler in isTravelerFFC_Check ? travelers.Where(trav => trav.FutureFlightCredits != null).Where(tc => tc.FutureFlightCredits.Exists(f => f.PinCode == ancillaryTC.PinCode)) : travelers.Where(trav => trav.FutureFlightCredits.Exists(f => f.PinCode == ancillaryTC.PinCode)))
                        {
                            var travelerFFC = scTraveler.FutureFlightCredits.FirstOrDefault(ffc => ffc.PinCode == ancillaryTC.PinCode);
                            if (travelerFFC != null)
                            {
                                travelerFFC.NewValueAfterRedeem = 0;
                                travelerFFC.NewValueAfterRedeem = Math.Round(travelerFFC.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                                travelerFFC.DisplayNewValueAfterRedeem = (travelerFFC.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);

                                AssignTravelerTotalFFCNewValueAfterReDeem(scTraveler);
                            }
                        }
                        foreach (var tnIndex in ancillaryTC.EligibleTravelers)
                        {
                            MOBCPTraveler traveler = isTravelerFFC_Check ? travelers.Where(tc => tc.FutureFlightCredits != null).FirstOrDefault(trav => trav.TravelerNameIndex == tnIndex) : travelers.FirstOrDefault(trav => trav.TravelerNameIndex == tnIndex);
                            var travelerFFC = traveler?.FutureFlightCredits.FirstOrDefault(ffc => ffc.PinCode == ancillaryTC.PinCode);
                            if (travelerFFC != null)
                            {
                                travelerFFC.NewValueAfterRedeem = ancillaryTC.NewValueAfterRedeem / ancillaryTC.EligibleTravelers.Count;
                                travelerFFC.NewValueAfterRedeem = Math.Round(travelerFFC.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                                travelerFFC.DisplayNewValueAfterRedeem = (travelerFFC.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);

                                AssignTravelerTotalFFCNewValueAfterReDeem(traveler);
                            }
                        }
                    }

                    var newRefundValueAfterReeedeem = ancillaryTCs.Sum(tcAmount => tcAmount.NewValueAfterRedeem);
                    var refundPrice = prices.FirstOrDefault(p => p.PriceType == "REFUNDPRICE");
                    if (refundPrice != null)
                    {
                        if (newRefundValueAfterReeedeem > 0)
                        {
                            UpdateCertificatePrice(refundPrice, newRefundValueAfterReeedeem, "REFUNDPRICE", "Total Credit", "RESIDUALCREDIT");
                        }
                        else
                        {
                            prices.RemoveAll(p => p.PriceType == "REFUNDPRICE");
                        }
                    }
                }
            }
        }
        private void UpdateTravelCreditRedeemAmount(MOBFOPTravelCredit travelCredit, double redeemAmount)
        {
            travelCredit.RedeemAmount = redeemAmount;
            travelCredit.RedeemAmount = Math.Round(travelCredit.RedeemAmount, 2, MidpointRounding.AwayFromZero);
            travelCredit.DisplayRedeemAmount = (redeemAmount).ToString("C2", CultureInfo.CurrentCulture);
            travelCredit.NewValueAfterRedeem = travelCredit.CurrentValue - redeemAmount;
            travelCredit.NewValueAfterRedeem = Math.Round(travelCredit.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
            travelCredit.DisplayNewValueAfterRedeem = (travelCredit.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);
            travelCredit.IsApplied = redeemAmount > 0;
        }
        public List<MOBMobileCMSContentMessages> BuildReviewFFCHeaderMessage(FOPTravelFutureFlightCredit travelFutureFlightCredit, List<MOBCPTraveler> travelers, List<CMSContentMessage> lstMessages)
        {
            List<MOBMobileCMSContentMessages> ffcHeaderMessage = null;
            if (travelFutureFlightCredit?.FutureFlightCredits?.Count() > 1 && travelFutureFlightCredit.FutureFlightCredits.Sum(ffc => ffc.NewValueAfterRedeem) > 0)
            {
                ffcHeaderMessage = ShopStaticUtility.GetSDLMessageFromList(lstMessages, "RTI.FutureFlightCredits.ReviewFFC");
                string contentFullMessage = ffcHeaderMessage[0].ContentFull;
                string travlerBalanceFFCMessageTemplete = "{0}:{1}, travel-by date {2}";
                string travlerBalanceFFCMessage = "";
                foreach (var traveler in travelers)
                {
                    if (traveler.FutureFlightCredits != null && traveler.FutureFlightCredits.Count > 1 && traveler.FutureFlightCredits.Sum(ffc => ffc.NewValueAfterRedeem) > 0)
                    {
                        travlerBalanceFFCMessage += (travlerBalanceFFCMessage != "" ? " and " : "") + string.Format(travlerBalanceFFCMessageTemplete,
                                                                                traveler.FirstName + " " + traveler.LastName,
                                                                                traveler.FutureFlightCredits.Sum(ffc => ffc.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture),
                                                                                traveler.FutureFlightCredits.Min(ffc => Convert.ToDateTime(ffc.ExpiryDate).ToString("MM/dd/yyyy")));
                    }
                }
                if (!string.IsNullOrEmpty(travlerBalanceFFCMessage))
                {
                    ffcHeaderMessage[0].ContentFull = string.Format(ffcHeaderMessage[0].ContentFull, travlerBalanceFFCMessage);
                }
                else
                {
                    ffcHeaderMessage = null;
                }
            }

            return ffcHeaderMessage;
        }
        public void ApplyFFCToAncillary(List<ProdDetail> products, MOBApplication application, MOBFormofPaymentDetails mobFormofPaymentDetails, List<MOBSHOPPrice> prices, bool isAncillaryON = false)
        {
            bool isAncillaryFFCEnable = (application == null ? isAncillaryON : IsInclueWithThisToggle(application.Id, application.Version.Major, "EnableTravelCreditAncillary", "AndroidTravelCreditVersionAncillary", "iPhoneTravelCreditVersionAncillary"));
            var futureFlightCredits = mobFormofPaymentDetails.TravelFutureFlightCredit?.FutureFlightCredits;

            if (isAncillaryFFCEnable && futureFlightCredits?.Count > 0)
            {
                mobFormofPaymentDetails.TravelFutureFlightCredit.AllowedFFCAmount = GetAllowedFFCAmount(products, isAncillaryFFCEnable);
                mobFormofPaymentDetails.TravelFutureFlightCredit.totalAllowedAncillaryAmount = GetAncillaryAmount(products, isAncillaryFFCEnable);

                var travelCredits = mobFormofPaymentDetails.TravelCreditDetails.TravelCredits.Where(tc => futureFlightCredits.Exists(ffc => ffc.PinCode == tc.PinCode)).ToList();
                int index = 0;

                foreach (var travelCredit in travelCredits)
                {
                    double ffcAppliedToAncillary = 0;
                    ffcAppliedToAncillary = futureFlightCredits.Where(ffc => ffc.TravelerNameIndex == "ANCILLARY").Sum(t => t.RedeemAmount);
                    ffcAppliedToAncillary = Math.Round(ffcAppliedToAncillary, 2, MidpointRounding.AwayFromZero);
                    var existedFFC = futureFlightCredits.FirstOrDefault(f => f.TravelerNameIndex == "ANCILLARY" && f.PinCode == travelCredit.PinCode);
                    double alreadyAppliedAmount = futureFlightCredits.Where(f => f.PinCode == travelCredit.PinCode).Sum(p => p.RedeemAmount);
                    var balanceAfterAppliedToRESAndAncillary = travelCredit.CurrentValue - alreadyAppliedAmount;

                    if (balanceAfterAppliedToRESAndAncillary > 0 &&
                        ffcAppliedToAncillary < mobFormofPaymentDetails.TravelFutureFlightCredit?.totalAllowedAncillaryAmount &&
                        existedFFC == null)
                    {
                        index++;
                        var mobFFC = new MOBFOPFutureFlightCredit();
                        mobFFC.CreditAmount = travelCredit.CreditAmount;
                        mobFFC.ExpiryDate = Convert.ToDateTime(travelCredit.ExpiryDate).ToString("MMMMM dd, yyyy");
                        mobFFC.IsCertificateApplied = true;
                        mobFFC.InitialValue = travelCredit.InitialValue;
                        mobFFC.Index = index;
                        mobFFC.PinCode = travelCredit.PinCode;
                        mobFFC.PromoCode = travelCredit.PromoCode;
                        mobFFC.RecordLocator = travelCredit.RecordLocator;
                        mobFFC.TravelerNameIndex = "ANCILLARY";
                        double remainingBalanceAfterAppliedFFC = mobFormofPaymentDetails.TravelFutureFlightCredit.totalAllowedAncillaryAmount - ffcAppliedToAncillary;
                        mobFFC.RedeemAmount = remainingBalanceAfterAppliedFFC > balanceAfterAppliedToRESAndAncillary ? balanceAfterAppliedToRESAndAncillary : remainingBalanceAfterAppliedFFC;
                        mobFFC.RedeemAmount = Math.Round(mobFFC.RedeemAmount, 2, MidpointRounding.AwayFromZero);
                        mobFFC.DisplayRedeemAmount = (mobFFC.RedeemAmount).ToString("C2", CultureInfo.CurrentCulture);
                        mobFFC.NewValueAfterRedeem = travelCredit.CurrentValue - (mobFFC.RedeemAmount + alreadyAppliedAmount);
                        mobFFC.NewValueAfterRedeem = Math.Round(mobFFC.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                        mobFFC.DisplayNewValueAfterRedeem = (mobFFC.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);
                        mobFFC.IsCertificateApplied = true;
                        mobFFC.CurrentValue = travelCredit.CurrentValue;
                        futureFlightCredits.Add(mobFFC);
                    }
                    else if (existedFFC != null)
                    {
                        double remainingBalanceAfterAppliedFFC = (mobFormofPaymentDetails.TravelFutureFlightCredit.totalAllowedAncillaryAmount - ffcAppliedToAncillary) + existedFFC.RedeemAmount;
                        existedFFC.NewValueAfterRedeem += existedFFC.RedeemAmount;
                        existedFFC.RedeemAmount = 0;
                        existedFFC.RedeemAmount = remainingBalanceAfterAppliedFFC > existedFFC.NewValueAfterRedeem ? existedFFC.NewValueAfterRedeem : remainingBalanceAfterAppliedFFC;
                        existedFFC.RedeemAmount = Math.Round(existedFFC.RedeemAmount, 2, MidpointRounding.AwayFromZero);
                        existedFFC.DisplayRedeemAmount = (existedFFC.RedeemAmount).ToString("C2", CultureInfo.CurrentCulture);
                        existedFFC.NewValueAfterRedeem -= existedFFC.RedeemAmount;
                        existedFFC.NewValueAfterRedeem = Math.Round(existedFFC.NewValueAfterRedeem, 2, MidpointRounding.AwayFromZero);
                        existedFFC.DisplayNewValueAfterRedeem = (existedFFC.NewValueAfterRedeem).ToString("C2", CultureInfo.CurrentCulture);
                    }

                    futureFlightCredits.RemoveAll(f => f.RedeemAmount <= 0);
                    UpdatePricesInReservation(mobFormofPaymentDetails.TravelFutureFlightCredit, prices);
                    AssignIsOtherFOPRequired(mobFormofPaymentDetails, prices);
                    AssignFormOfPaymentType(mobFormofPaymentDetails, prices, false);
                }
            }
        }

        private double GetAllowedFFCAmount(List<ProdDetail> products, bool isAncillaryFFCEnable = false)
        {
            string allowedFFCProducts = string.Empty;
            allowedFFCProducts = _configuration.GetValue<string>("FFCEligibleProductCodes");
            double allowedFFCAmount = products == null ? 0 : products.Where(p => (allowedFFCProducts.IndexOf(p.Code) > -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            allowedFFCAmount += GetAncillaryAmount(products, isAncillaryFFCEnable);
            allowedFFCAmount = Math.Round(allowedFFCAmount, 2, MidpointRounding.AwayFromZero);
            return allowedFFCAmount;
        }
        private double GetAncillaryAmount(List<ProdDetail> products, bool isAncillaryFFCEnable = false)
        {
            double allowedFFCAmount = 0;
            if (isAncillaryFFCEnable)
            {
                string allowedFFCProducts = _configuration.GetValue<string>("TravelCreditEligibleProducts");
                allowedFFCAmount += products == null ? 0 : products.Where(p => (allowedFFCProducts.IndexOf(p.Code) > -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
                allowedFFCAmount += GetBundlesAmount(products);
                allowedFFCAmount = Math.Round(allowedFFCAmount, 2, MidpointRounding.AwayFromZero);
            }
            return allowedFFCAmount;
        }
        public double GetBundlesAmount(List<ProdDetail> products)
        {
            string nonBundleProductCode = _configuration.GetValue<string>("NonBundleProductCode");
            double bundleAmount = products == null ? 0 : products.Where(p => (nonBundleProductCode.IndexOf(p.Code) == -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            return bundleAmount;
        }

        private bool IsInclueWithThisToggle(int appId, string appVersion, string configToggleKey, string androidVersion, string iosVersion)
        {
            return _configuration.GetValue<bool>(configToggleKey) &&
                   GeneralHelper.IsApplicationVersionGreater(appId, appVersion, androidVersion, iosVersion, "", "", true, _configuration);
        }
        private bool IncludeTravelBankFOP(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelBankFOP")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidTravelBankFOPVersion", "iPhoneTravelBankFOPVersion", "", "", true, _configuration);
        }

        private bool IncludeMoneyPlusMilesV2(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableMilesPlusMoney")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidMilesPlusMoneyVersion", "iPhoneMilesPlusMoneyVersion", "", "", true, _configuration);
        }
    }
}
