using System;
using System.Collections.Generic;
using System.Globalization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPTravelFutureFlightCredit
    {
        private string ffcButtonText;
        private double totalRedeemAmount;
        private List<MOBMobileCMSContentMessages> learnmoreTermsandConditions;
        private List<MOBMobileCMSContentMessages> reviewFFCMessages;
        private List<MOBMobileCMSContentMessages> lookUpFFCMessages;
        private List<MOBMobileCMSContentMessages> findFFCMessages;
        private MOBSection findFFCConfirmationMessage;
        private List<MOBFOPFutureFlightCredit> futureFlightCredits;
        private MOBSection goToTripDetailDialog;

        public MOBSection GoToTripDetailDialog
        {
            get { return goToTripDetailDialog; }
            set { goToTripDetailDialog = value; }
        }
        public List<MOBMobileCMSContentMessages> EmailConfirmationFFCMessages { get; set; }
        public List<MOBFOPFutureFlightCredit> FutureFlightCredits
        {
            get { return futureFlightCredits; }
            set { futureFlightCredits = value; }
        }


        public MOBSection FindFFCConfirmationMessage
        {
            get { return findFFCConfirmationMessage; }
            set { findFFCConfirmationMessage = value; }
        }


        public List<MOBMobileCMSContentMessages> FindFFCMessages
        {
            get { return findFFCMessages; }
            set { findFFCMessages = value; }
        }

        //private MOBSection removeAllCertificateAlertMessage;

        public List<MOBMobileCMSContentMessages> LookUpFFCMessages
        {
            get { return lookUpFFCMessages; }
            set { lookUpFFCMessages = value; }
        }

        public List<MOBMobileCMSContentMessages> ReviewFFCMessages
        {
            get { return reviewFFCMessages; }
            set { reviewFFCMessages = value; }
        }

        public List<MOBMobileCMSContentMessages> LearnmoreTermsandConditions
        {
            get
            {
                return learnmoreTermsandConditions;
            }
            set
            {
                learnmoreTermsandConditions = value;
            }
        }


        public string DisplayTotalRedeemAmountText { get; set; }

        public string FFCButtonText
        {
            get { return ffcButtonText; }
            set { ffcButtonText = value; }
        }

        public double TotalRedeemAmount
        {
            get
            {
                totalRedeemAmount = 0;
                if (futureFlightCredits != null && futureFlightCredits.Count > 0)
                {
                    foreach (var certificate in futureFlightCredits)
                    {
                        totalRedeemAmount += certificate.RedeemAmount;
                    }
                }
                return totalRedeemAmount;
            }
        }
        //public MOBSection RemoveAllCertificateAlertMessage
        //{
        //    get { return removeAllCertificateAlertMessage; }
        //    set { removeAllCertificateAlertMessage = value; }
        //}
        private double allowedFFCAmount;
        public double AllowedFFCAmount
        {
            get { return allowedFFCAmount; }
            set { allowedFFCAmount = value; }
        }
        private double AllowedAncillaryAmount;

        public double totalAllowedAncillaryAmount
        {
            get { return AllowedAncillaryAmount; }
            set { AllowedAncillaryAmount = value; }
        }

    }
}
