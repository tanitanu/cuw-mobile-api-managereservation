using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using United.Mobile.Model.Fitbit;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.ManageRes
{
    public enum FopType
    {
        Unknown = 0,
        CreditCard = 1,
        PayPal = 2,
        AliPay = 3,
        Certificate = 4,
        TravelBank = 5,
        Cash = 6,
        Check = 7,
        CompanyDirectPayment = 8
    }

    [Serializable]
    public class QuoteRefundResponse
    {
        public string QuoteType { get; set; }
        public virtual Collection<Characteristic> Characteristics { get; set; }
        public string Pnr { get; set; }
        public MOBPolicy Policy { get; set; }
        public virtual List<MOBPriceBreakDown> PriceBreakDown { get; set; }
        public MOBPayment FopDetails { get; set; }
        public MOBBasePrice RefundAmount { get; set; }
        public MOBBasePrice RefundMiles { get; set; }
        public MOBBasePrice RefundFee { get; set; }
        public virtual Collection<Error> Error { get; set; }
        public virtual Collection<Status> StatusDetails { get; set; }
        public MOBBasePrice AwardRedepositFee { get; set; }
        public MOBBasePrice AwardRedepositFeeTotal { get; set; }
        public List<AncillaryCharge> AncillaryProducts { get; set; }
        public List<AncillaryCharge> RefundAncillaryProducts { get; set; }
        public Charge RefundAmountTicket { get; set; }
        public List<RefundUpgradeInstrument> RefundUpgradeInstruments { get; set; }
        public double RefundUpgradeInstrumentsTotal { get; set; }
        public List<RefundUpgradePoint> RefundUpgradePoints { get; set; }
        public MOBBasePrice RefundUpgradePointsTotal { get; set; }
        public string PointOfSale { get; set; }
        public RefundableCharge RefundAmountOtherCurrency { get; set; }
        public List<Payment> PaymentMethods { get; set; }
    }

    public class AncillaryCharge : RefundableCharge
    {
        public string ConfirmationNumber { get; set; }
        public string PurchaseDate { get; set; }
        public string Status { get; set; }
        public Payment PaymentMethod { get; set; }
        public List<Payment> PaymentMethods { get; set; }
        public List<Charge> SubProducts { get; set; }
    }

    public class RefundableCharge : Charge
    {
        public bool Refundable { get; set; }
        public bool Voidable { get; set; }
    }

    public class Charge
    {
        public decimal Amount { get; set; }
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlace { get; set; }
        public string Description { get; set; }
    }

    public class Payment
    {
        public FopType FopType { get; set; }
        public Charge PaidAmountTicket { get; set; }
        public Charge RefundAmountTicket { get; set; }
        public Charge RefundAmountTicketEquivalent { get; set; }
        public Charge RefundFee { get; set; }
        public int PaymentIndex { get; set; }
        public string PaymentType { get; set; }
        public string CreditCardType { get; set; }
        public string PaymentName { get; set; }
        public string PaymentDescription { get; set; }
        public string AccountNumber { get; set; }
        public Charge RefundAmount { get; set; }
        public string AccountNumberLastFourDigits => AccountNumber?.Substring(Math.Max(0, AccountNumber.Length - 4));
        public string AccountNumberToken { get; set; }
        public string ExpirationDate { get; set; }
        public bool IsTicketPaidFOP { get; set; }
        public bool IsTicketRefundFOP { get; set; }
        public bool IsAncillaryRefundFOP { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlace { get; set; }
        public string MileagePlusNumber { get; set; }
        public MOBBasePrice RefundMiles { get; set; }

        public Payment Clone()
        {
            return new Payment
            {
                FopType = this.FopType,
                PaymentIndex = this.PaymentIndex,
                AccountNumber = this.AccountNumber,
                AccountNumberToken = AccountNumberToken,
                Amount = this.Amount,
                PaidAmountTicket = this.PaidAmountTicket,
                RefundAmountTicket = this.RefundAmountTicket,
                RefundAmountTicketEquivalent = this.RefundAmountTicketEquivalent,
                CreditCardType = this.CreditCardType,
                CurrencyCode = this.CurrencyCode,
                DecimalPlace = this.DecimalPlace,
                IsTicketPaidFOP = this.IsTicketPaidFOP,
                IsTicketRefundFOP = this.IsTicketRefundFOP,
                IsAncillaryRefundFOP = this.IsAncillaryRefundFOP,
                PaymentDescription = this.PaymentDescription,
                PaymentName = this.PaymentName,
                PaymentType = this.PaymentType,
                MileagePlusNumber = this.MileagePlusNumber,
                RefundMiles = this.RefundMiles,
            };
        }
    }

    public class RefundUpgradeInstrument
    {
        public string RedeemingMileagePlusNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FormNumber { get; set; }
        public bool Waitlisted { get; set; }
        public bool Refundable { get; set; }
        public bool Voidable { get; set; }
        public float Amount { get; set; }
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlace { get; set; }
    }

    public class RefundUpgradePoint
    {
        public string RedeemingMileagePlusNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string HardAuthId { get; set; }
        public string FormNumber { get; set; }
        public CabinDetail[] CabinDetails { get; set; }
        public bool Waitlisted { get; set; }
        public bool Refundable { get; set; }
        public bool Voidable { get; set; }
        public float Amount { get; set; }
        public string Code { get; set; }
        public string CurrencyCode { get; set; }
        public int DecimalPlace { get; set; }
        public string Description { get; set; }
    }
    public class CabinDetail
    {
        public string CabinName { get; set; }
        public string CabinDescription { get; set; }
        public string SegmentId { get; set; }
    }
}
