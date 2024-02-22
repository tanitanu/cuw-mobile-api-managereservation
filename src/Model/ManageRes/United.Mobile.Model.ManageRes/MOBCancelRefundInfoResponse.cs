using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using United.Mobile.Model.Common;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBCancelRefundInfoResponse : MOBModifyReservationResponse
    {
        [JsonIgnore]
        public string ObjectName { get; set; } = "United.Definition.CancelReservation.MOBCancelRefundInfoResponse";
        private string policyMessage = string.Empty;
        private MOBModifyFlowPricingInfo pricing;            
        private bool cancelPathEligible;
        private bool refundPathEligible;
        private MOBPayment payment;
        private string redirectURL = string.Empty;
        private MOBBasePrice refundAmount;
        private List<MOBRefundOption> refundOptions;
        private string reservationValueHeader;
        private string failedRule;
        private bool revenueReshopPathEligible;
        private string customerServicePhoneNumber;
        private string formattedQuoteType = string.Empty;
        private string paymentOption = string.Empty;
        private string token = string.Empty;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;
        private string screenTitle = string.Empty;
        private string confirmButtonText = string.Empty;
        private bool awardTravel = false;
        private string sponsorMileagePlus = string.Empty;
        private string refundMessage = string.Empty;
        private string quoteTypeMessage = string.Empty;
        private bool partiallyFlown = false;
        private bool requireMailingAddress = false;
        private bool isManualRefund = false;
        private bool isExecutiveBulkTicket = false;        
        private List<MOBModifyFlowPricingInfo> quotes;
        private Collection<Characteristic> characteristics;
        private bool isBasicEconomyNonRefundable;
        private string pointOfSale = string.Empty;
        private bool showRefundEmail = false;
        private bool requireDateOfBirth = false;
        private bool isMultipleRefundFOP = false;
        private bool isCancellationFee = false;
        private bool isJapanStandardEconomy;
        private United.Service.Presentation.ReservationResponseModel.ReservationDetail reservationDetail;
        private bool isAgencyBooking = false;
        private MOBPnrInfo pnrInfo;
        private List<MOBPNRAdvisory> advisoryInfo;
        public List<MOBModifyFlowPricingInfo> Quotes
        {
            get { return quotes; }
            set { quotes = value; }
        }
        public Collection<Characteristic> Characteristics { get { return this.characteristics; } set { this.characteristics = value; } }
        public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        public bool IsAgencyBooking { get { return isAgencyBooking; } set { isAgencyBooking = value; } }

        public United.Service.Presentation.ReservationResponseModel.ReservationDetail ReservationDetail { get { return reservationDetail; } set { reservationDetail = value; } }
        public MOBPnrInfo PnrInfo { get { return pnrInfo; } set { pnrInfo = value; } }
        public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }
        public List<MOBRefundOption> RefundOptions { get { return this.refundOptions; } set { this.refundOptions = value; } }
        public string ReservationValueHeader { get { return this.reservationValueHeader; } set { this.reservationValueHeader = value; } }
        public string FailedRule { get { return this.failedRule; } set { this.failedRule = value; } }
        public string ScreenTitle { get { return this.screenTitle; } set { this.screenTitle = value; } }
        public string ConfirmButtonText { get { return this.confirmButtonText; } set { this.confirmButtonText = value; } }
        public string PaymentOption { get { return paymentOption; } set { paymentOption = value; } }
        public string Token { get { return token; } set { token = value; } }
        public string PolicyMessage { get { return policyMessage; } set { policyMessage = value; } }
        public MOBModifyFlowPricingInfo Pricing { get { return pricing; } set { pricing = value; } }                       
        public bool CancelPathEligible { get { return cancelPathEligible; } set { cancelPathEligible = value; } }
        public bool RefundPathEligible { get { return refundPathEligible; } set { refundPathEligible = value; } }
        public bool RevenueReshopPathEligible { get { return revenueReshopPathEligible; } set { revenueReshopPathEligible = value; } }
        public MOBPayment Payment { get { return payment; } set { payment = value; } }
        public string RedirectURL { get { return redirectURL; } set { redirectURL = value; } }
        public MOBBasePrice RefundAmount { get { return refundAmount; } set { refundAmount = value; } }
        public string CustomerServicePhoneNumber { get { return customerServicePhoneNumber; } set { customerServicePhoneNumber = value; } }
        public bool AwardTravel { get { return awardTravel; } set { awardTravel = value; } }
        public bool PartiallyFlown { get { return partiallyFlown; } set { partiallyFlown = value; } }
        public bool IsMultipleRefundFOP { get { return isMultipleRefundFOP; } set { isMultipleRefundFOP = value; } }
        public bool IsManualRefund { get { return isManualRefund; } set { isManualRefund = value; } }
        public bool IsExecutiveBulkTicket { get { return isExecutiveBulkTicket; } set { isExecutiveBulkTicket = value; } }
        public bool RequireMailingAddress { get { return requireMailingAddress; } set { requireMailingAddress = value; } }
        public string SponsorMileagePlus { get { return sponsorMileagePlus; } set { sponsorMileagePlus = value; } }
        public string RefundMessage { get { return refundMessage; } set { refundMessage = value; } }
        public string QuoteTypeMessage { get { return quoteTypeMessage; } set { quoteTypeMessage = value; } }
        public string FormattedQuoteType { get { return formattedQuoteType; } set { formattedQuoteType = value; } }
        public bool IsBasicEconomyNonRefundable { get { return isBasicEconomyNonRefundable;  } set { isBasicEconomyNonRefundable = value; } }
        public string PointOfSale { get { return pointOfSale; } set { pointOfSale = value; } }
        public bool ShowRefundEmail { get { return showRefundEmail; } set { showRefundEmail = value; } }
        public bool IsCancellationFee { get { return isCancellationFee; } set { isCancellationFee = value; } }
        public bool RequireDateOfBirth { get { return requireDateOfBirth; } set { requireDateOfBirth = value; } }
        public bool IsJapanStandardEconomy { get { return isJapanStandardEconomy; } set { isJapanStandardEconomy = value; } }
        public List<MOBPNRAdvisory> AdvisoryInfo { get { return this.advisoryInfo; } set { this.advisoryInfo = value; } }
    }
    [Serializable]
    public class MOBPnrInfo
    {
        private List<MOBPNRPassenger> pnrTravelers = null;
        private List<MOBPNRSegment> pnrSegment = null;
        private List<MOBTrip> pnrTrips = null;
        private string emailAddress = string.Empty;
        private bool awardTravel = false;

        public List<MOBPNRPassenger> PnrTravelers { get { return pnrTravelers; } set { pnrTravelers = value; } }
        [System.Text.Json.Serialization.JsonPropertyName("pnrSegment")]
        [JsonProperty("pnrSegment")]
        public List<MOBPNRSegment> PnrSegments { get { return pnrSegment; } set { pnrSegment = value; } }
        public List<MOBTrip> PnrTrips { get { return pnrTrips; } set { pnrTrips = value; } }
        public string EmailAddress { get { return emailAddress; } set { emailAddress = value; } }
        public bool AwardTravel { get { return awardTravel; } set { awardTravel = value; } }
    }
}
