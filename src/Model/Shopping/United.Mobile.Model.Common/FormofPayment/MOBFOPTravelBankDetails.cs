using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFOPTravelBankDetails
    {
        //ADD list of newly travelbank object as is like trvelcertificate
        private List<MOBMobileCMSContentMessages> applyTBContentMessage;
        private List<MOBMobileCMSContentMessages> reviewTBContentMessage;
        private List<MOBMobileCMSContentMessages> learnmoreTermsandConditions;
        private double tbAppliedByCustomer;

        public double TBAppliedByCustomer
        {
            get { return tbAppliedByCustomer; }
            set { tbAppliedByCustomer = value; }
        }

        public List<MOBMobileCMSContentMessages> LearnmoreTermsandConditions
        {
            get
            {
                return this.learnmoreTermsandConditions;
            }
            set
            {
                this.learnmoreTermsandConditions = value;
            }
        }
        private string payorLastName;

        public string PayorLastName
        {
            get { return payorLastName; }
            set { payorLastName = value; }
        }

        private string payorFirstName;

        public string PayorFirstName
        {
            get { return payorFirstName; }
            set { payorFirstName = value; }
        }

        private string mpnumber;

        public string MPNumber
        {
            get { return mpnumber; }
            set { mpnumber = value; }
        }

        public List<MOBMobileCMSContentMessages> ApplyTBContentMessage
        {
            get { return applyTBContentMessage; }
            set { applyTBContentMessage = value; }
        }

        public List<MOBMobileCMSContentMessages> ReviewTBContentMessage
        {
            get { return reviewTBContentMessage; }
            set { reviewTBContentMessage = value; }
        }

        private double tbBalance;

        public double TBBalance
        {
            get { return tbBalance; }
            set { tbBalance = value; }
        }

        private string displayTBBalance;

        public string DisplayTBBalance
        {
            get { return displayTBBalance; }
            set { displayTBBalance = value; }
        }

        private double tbApplied;

        public double TBApplied
        {
            get { return tbApplied; }
            set { tbApplied = value; }
        }

        private string displaytbApplied;

        public string DisplaytbApplied
        {
            get { return displaytbApplied; }
            set { displaytbApplied = value; }
        }

        private double remainingBalance;

        public double RemainingBalance
        {
            get { return remainingBalance; }
            set { remainingBalance = value; }
        }

        private string displayRemainingBalance;

        public string DisplayRemainingBalance
        {
            get { return displayRemainingBalance; }
            set { displayRemainingBalance = value; }
        }

        private string displayAvailableBalanceAsOfDate;
        public string DisplayAvailableBalanceAsOfDate { get => displayAvailableBalanceAsOfDate; set => displayAvailableBalanceAsOfDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
    }
}
