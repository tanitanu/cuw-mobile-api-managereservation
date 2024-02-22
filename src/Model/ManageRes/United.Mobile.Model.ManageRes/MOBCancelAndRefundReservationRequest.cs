using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Service.Presentation.PersonModel;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBCancelAndRefundReservationRequest : MOBModifyReservationRequest
    {
        private string recordLocator = string.Empty;
        private string emailAddress = string.Empty;
        private string lastName = string.Empty;
        private string pointOfSale = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string quoteType = string.Empty;
        private string refundAmount = string.Empty;
        private string currencyCode = string.Empty;
        private MOBSHOPFormOfPayment formOfPayment;
        private MOBAddress billingAddress;
        private MOBCPPhone billingPhone;
        private List<MOBRefundOption> selectedRefundOptions;
        private string dateOfBirth = string.Empty;

        public string Token { get; set; }
        public string RefundMiles { get; set; }
        public bool IsAward { get; set; }
        public decimal AwardRedepositFeeTotal { get; set; }
        public decimal AwardRedepositFee { get; set; }
        public LoyaltyPerson sponsor { get; set; }
        public List<MOBRefundOption> SelectedRefundOptions { get { return selectedRefundOptions; } set { selectedRefundOptions = value; } }
        public string RecordLocator
        { get { return recordLocator; } set { recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string EmailAddress
        { get { return emailAddress; }  set { emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string LastName
        { get { return lastName; } set { lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string PointOfSale
        { get { return pointOfSale; } set { pointOfSale = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string MileagePlusNumber
        { get { return mileagePlusNumber; } set { mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string QuoteType
        { get { return quoteType; } set { quoteType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string RefundAmount
        { get { return refundAmount; } set { refundAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public string CurrencyCode
        { get { return currencyCode; } set { currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }}
        public MOBSHOPFormOfPayment FormOfPayment
        { get { return this.formOfPayment; } set {this.formOfPayment = value; }}
        public MOBAddress BillingAddress
        { get { return this.billingAddress; } set { this.billingAddress = value;} }
        public MOBCPPhone BillingPhone
        { get { return this.billingPhone; } set { this.billingPhone = value; }}        
        public string DateOfBirth { get { return dateOfBirth; } set { dateOfBirth = value; } }
    }
}
