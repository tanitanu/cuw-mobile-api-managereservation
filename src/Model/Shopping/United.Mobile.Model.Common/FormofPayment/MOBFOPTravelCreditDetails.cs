using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFOPTravelCreditDetails
    {

        private List<MOBMobileCMSContentMessages> lookUpMessages;
        private List<MOBMobileCMSContentMessages> alertMessages;
        private List<MOBMobileCMSContentMessages> reviewMessages;
        private double totalRedeemAmount;
        private string nameWaiverMatchMessage;
        private string travelCreditSummary;
        // private MOBSection findETCConfirmationMessage;

        public string NameWaiverMatchMessage
        {
            get { return nameWaiverMatchMessage; }
            set { nameWaiverMatchMessage = value; }
        }

        public List<MOBMobileCMSContentMessages> LookUpMessages
        {
            get { return lookUpMessages; }
            set { lookUpMessages = value; }
        }

 
        public List<MOBMobileCMSContentMessages> AlertMessages
        {
            get { return alertMessages; }
            set { alertMessages = value; }
        }

        public List<MOBMobileCMSContentMessages> ReviewMessages
        {
            get { return reviewMessages; }
            set { reviewMessages = value; }
        }

        //public MOBSection FindETCConfirmationMessage
        //{
        //    get { return findETCConfirmationMessage; }
        //    set { findETCConfirmationMessage = value; }
        //}
        private List<MOBMobileCMSContentMessages> reviewTravelCreditMessages;
        public List<MOBMobileCMSContentMessages> ReviewTravelCreditMessages
        {
            get { return reviewTravelCreditMessages; }
            set { reviewTravelCreditMessages = value; }
        }
        public string TravelCreditSummary
        {
            get { return travelCreditSummary; }
            set { travelCreditSummary = value; }
        }
        private List<MOBFOPTravelCredit> travelCredits;
        public List<MOBFOPTravelCredit> TravelCredits
        {
            get { return travelCredits; }
            set { travelCredits = value; }
        }

        public double TotalRedeemAmount
        {
            get
            {
                totalRedeemAmount = 0;
                if (travelCredits != null && travelCredits.Count > 0)
                {
                    foreach (var travelCredit in travelCredits)
                    {
                        if (travelCredit.IsApplied)
                            totalRedeemAmount += travelCredit.RedeemAmount;
                    }
                }
                return totalRedeemAmount;
            }
        }
    }
}
