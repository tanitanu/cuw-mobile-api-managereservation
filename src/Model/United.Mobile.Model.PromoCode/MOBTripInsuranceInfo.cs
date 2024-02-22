using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PromoCode
{
    [Serializable]
    public class MOBTripInsuranceInfo
    {
        private double amount;
        private string quoteHeader = string.Empty; //Ex: Insure your trip with
        private string quoteCompanyName = string.Empty;// Ex: AIG Travel Guard 
        private string quoteDisplayAmount = string.Empty; // Ex: for $28.50 
        private string currencyCode = string.Empty;
        private List<string> listOfBenifits;// EX: 100% trip cancellation 
        private string tnc = string.Empty;
        private string tncLink = string.Empty;
        private string code = string.Empty;
        private string productId = string.Empty;
        private List<MOBSHOPTravelOptionSubItem> listOfPriceDetails;// Ex: per person price and total price         

        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }
        public string QuoteHeader
        {
            get
            {
                return this.quoteHeader;
            }
            set
            {
                this.quoteHeader = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string QuoteCompanyName
        {
            get
            {
                return this.quoteCompanyName;
            }
            set
            {
                this.quoteCompanyName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string QuoteDisplayAmount
        {
            get
            {
                return this.quoteDisplayAmount;
            }
            set
            {
                this.quoteDisplayAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CurrencyCode
        {
            get
            {
                return this.currencyCode;
            }
            set
            {
                this.currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


        public List<string> ListOfBenifits
        {
            get { return listOfBenifits; }
            set { listOfBenifits = value; }
        }


        public string Tnc
        {
            get
            {
                return this.tnc;
            }
            set
            {
                this.tnc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TncLink
        {
            get
            {
                return this.tncLink;
            }
            set
            {
                this.tncLink = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBSHOPTravelOptionSubItem> ListOfPriceDetails
        {
            get { return listOfPriceDetails; }
            set { listOfPriceDetails = value; }
        }

    }
    [Serializable]
    public class MOBTripInsuranceFile
    {
        private MOBTPIInfo tripInsuranceInfo;
        public MOBTPIInfo TripInsuranceInfo
        {
            get { return this.tripInsuranceInfo; }
            set { this.tripInsuranceInfo = value; }
        }

        private MOBSHOPMakeReservationRequest registerFOPForTPIRequest;
        public MOBSHOPMakeReservationRequest RegisterFOPForTPIRequest
        {
            get { return this.registerFOPForTPIRequest; }
            set { this.registerFOPForTPIRequest = value; }
        }
        private MOBSHOPMakeReservationRequest registerFormOfPaymentForSecondaryPaymentRequest;
        public MOBSHOPMakeReservationRequest RegisterFormOfPaymentForSecondaryPaymentRequest
        {
            get { return this.registerFormOfPaymentForSecondaryPaymentRequest; }
            set { this.registerFormOfPaymentForSecondaryPaymentRequest = value; }
        }
        private MOBSHOPMakeReservationResponse registerFormOfPaymentForSecondaryPaymentResponse;
        public MOBSHOPMakeReservationResponse RegisterFormOfPaymentForSecondaryPaymentResponse
        {
            get { return this.registerFormOfPaymentForSecondaryPaymentResponse; }
            set { this.registerFormOfPaymentForSecondaryPaymentResponse = value; }
        }
        private MOBRegisterFOPForTPIResponse registerFOPForTPIResponse;
        public MOBRegisterFOPForTPIResponse RegisterFOPForTPIResponse
        {
            get { return this.registerFOPForTPIResponse; }
            set { this.registerFOPForTPIResponse = value; }
        }

        private string accountNumberToken = string.Empty;
        public string AccountNumberToken
        {
            get { return accountNumberToken; }
            set { accountNumberToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string confirmationResponseDetailMessage1 = string.Empty;
        public string ConfirmationResponseDetailMessage1
        {
            get { return confirmationResponseDetailMessage1; }
            set { confirmationResponseDetailMessage1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string confirmationResponseDetailMessage2 = string.Empty;
        public string ConfirmationResponseDetailMessage2
        {
            get { return confirmationResponseDetailMessage2; }
            set { confirmationResponseDetailMessage2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private MOBTPIInfoInBookingPath tripInsuranceBookingInfo;
        public MOBTPIInfoInBookingPath TripInsuranceBookingInfo
        {
            get { return this.tripInsuranceBookingInfo; }
            set { this.tripInsuranceBookingInfo = value; }
        }
    }
}
