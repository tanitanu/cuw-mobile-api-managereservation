using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBQuoteRefundResponse : MOBResponse
    {
        [JsonIgnore]
        public string ObjectName { get; set; } = "United.Definition.CancelReservation.MOBQuoteRefundResponse";
        private string quoteType;
        public string QuoteType
        {
            get { return quoteType; }
            set { quoteType = value; }
        }

        private MOBPolicy policy;
        public MOBPolicy Policy {
            get { return policy; }
            set { policy = value; }
        }

        private List<MOBPriceBreakDown> priceBreakDown; 
        public List<MOBPriceBreakDown> PriceBreakDown
        {
            get { return priceBreakDown; }
            set { priceBreakDown = value; }
        }

        private MOBPayment fopDetails;
        public MOBPayment FopDetails
        {
            get { return fopDetails; }
            set { fopDetails = value; }
        }

        private MOBBasePrice refundAmount;
        public MOBBasePrice RefundAmount
        {
            get { return refundAmount; }
            set { refundAmount = value; }
        }

        private MOBBasePrice refundMiles;
        public MOBBasePrice RefundMiles
        {
            get { return refundMiles; }
            set { refundMiles = value; }
        }

        private MOBBasePrice refundFee;
        public MOBBasePrice RefundFee
        {
            get { return refundFee; }
            set { refundFee = value; }
        }

        private MOBBasePrice awardRedepositFee;

        public MOBBasePrice AwardRedepositFee
        {
            get { return awardRedepositFee; }
            set { awardRedepositFee = value; }
        }

        private MOBBasePrice awardRedepositFeeTotal;

        public MOBBasePrice AwardRedepositFeeTotal
        {
            get { return awardRedepositFeeTotal; }
            set { awardRedepositFeeTotal = value; }
        }
        public Boolean IsRevenueRefundable { get; set; }
        public Boolean IsRevenueNonRefundable { get; set; }
        public Boolean IsFFCREligible { get; set; }
        public Boolean IsMultipleRefundFOP { get; set; }
        public Boolean IsMilesMoneyPaidFOP { get; set; }
        public Boolean IsMilesMoneyRefundFOP { get; set; }
        public Boolean IsETCEligible { get; set; }
        public Boolean IsCancelOnlyEligible { get; set; }
        public Boolean IsRefundfeeAvailable { get; set; }
        public Boolean IsMilesMoneyFFCTRefundFOP { get; set; }
        public bool IsAwardTravel { get; set; }
        public Boolean IsFarelock { get; set; }
        public Boolean IsFutureflightCredit { get; set; }
        public Boolean ShowETCConvertionInfo { get; set; }
        public Boolean IsBECancelFeeEligible { get; set; }
        public List<AncillaryCharge> AncillaryProducts { get; set; }
        public List<AncillaryCharge> RefundAncillaryProducts { get; set; }
        public Charge RefundAmountTicket { get; set; }
        public List<RefundUpgradeInstrument> RefundUpgradeInstruments { get; set; }
        public double RefundUpgradeInstrumentsTotal { get; set; }
        public List<RefundUpgradePoint> RefundUpgradePoints { get; set; }
        public MOBBasePrice RefundUpgradePointsTotal { get; set; }
        public string PointOfSale { get; set; }
        public RefundableCharge RefundAmountOtherCurrency { get; set; }
        public bool IsRefundableTicket { get; set; }
        public List<Payment> PaymentMethods { get; set; }
        public bool IsJapanStandardEconomy { get; set; }

    }
}
