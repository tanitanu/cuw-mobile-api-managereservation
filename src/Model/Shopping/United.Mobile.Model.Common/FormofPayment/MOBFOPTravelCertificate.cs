using System;
using System.Collections.Generic;
using System.Globalization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFOPTravelCertificate
    {
        private List<MOBFOPCertificate> certificates;
        private string certificateButtonText;
        private double totalRedeemAmount;
        private string displayTotalRedeemAmountText;
        private List<MOBMobileCMSContentMessages> learnmoreTermsandConditions;
        private double allowedETCAmount;
        private double notAllowedETCAmount;
        private int maxNumberOfETCsAllowed;
        private double maxAmountOfETCsAllowed;
        private List<MOBMobileCMSContentMessages> reviewETCMessages;
        private List<MOBMobileCMSContentMessages> savedETCMessages;
        private Section removeAllCertificateAlertMessage;

        public List<MOBMobileCMSContentMessages> SavedETCMessages
        {
            get { return savedETCMessages; }
            set { savedETCMessages = value; }
        }

        public List<MOBMobileCMSContentMessages> ReviewETCMessages
        {
            get { return reviewETCMessages; }
            set { reviewETCMessages = value; }
        }

        public double MaxAmountOfETCsAllowed
        {
            get { return maxAmountOfETCsAllowed; }
            set { maxAmountOfETCsAllowed = value; }
        }

        public int MaxNumberOfETCsAllowed
        {
            get { return maxNumberOfETCsAllowed; }
            set { maxNumberOfETCsAllowed = value; }
        }
        public double NotAllowedETCAmount
        {
            get { return notAllowedETCAmount; }
            set { notAllowedETCAmount = value; }
        }


        public double AllowedETCAmount
        {
            get { return allowedETCAmount; }
            set { allowedETCAmount = value; }
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


        public string DisplayTotalRedeemAmountText 
        {
            get
            {
                displayTotalRedeemAmountText = (TotalRedeemAmount).ToString("N2", CultureInfo.CurrentCulture);
                return displayTotalRedeemAmountText;
            }
        }

        public List<MOBFOPCertificate> Certificates
        {
            get { return certificates; }
            set { certificates = value; }
        }

        public string CertificateButtonText
        {
            get { return certificateButtonText; }
            set { certificateButtonText = value; }
        }

        public double TotalRedeemAmount
        {
            get
            {
                totalRedeemAmount = 0;
                if (Certificates != null && Certificates.Count > 0)
                {
                    foreach (var certificate in Certificates)
                    {
                        totalRedeemAmount += certificate.RedeemAmount;
                    }
                }
                return totalRedeemAmount;
            }
        }
        public Section RemoveAllCertificateAlertMessage
        {
            get { return removeAllCertificateAlertMessage; }
            set { removeAllCertificateAlertMessage = value; }
        }
    }
}