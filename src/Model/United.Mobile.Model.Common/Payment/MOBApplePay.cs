using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBApplePay
    {
        private string applePayLoadJSON;
        private MOBAddress billingAddress;
        private MOBEmail emailAddress;
        private MOBName cardHolderName;
        private string cardNameWithLastFourDigits;

        public string CardNameWithLastFourDigits
        {
            get { return cardNameWithLastFourDigits; }
            set { cardNameWithLastFourDigits = value; }
        }

        public string ApplePayLoadJSON
        {
            get { return applePayLoadJSON; }
            set { applePayLoadJSON = value; }
        }
        
        public MOBAddress BillingAddress
        {
            get { return billingAddress; }
            set { billingAddress = value; }
        }

        public MOBEmail EmailAddress
        {
            get {return emailAddress;}
            set{emailAddress = value;}
        }

        public MOBName CardHolderName
        {
            get { return cardHolderName; }
            set { cardHolderName = value; }
        }

        public string LastFourDigits
        {
            get
            {
                string fourDigits = string.Empty;
                if (!String.IsNullOrEmpty(this.cardNameWithLastFourDigits) && this.CardNameWithLastFourDigits.Length >= 4)
                    fourDigits = this.cardNameWithLastFourDigits.Substring(this.cardNameWithLastFourDigits.Length - 4).Trim();

                return fourDigits;
            }
        }

        public string CardName
        {
            get
            {
                string cardName = string.Empty;
                if (!String.IsNullOrEmpty(this.cardNameWithLastFourDigits))
                    cardName = FilterAlphabetsFromString(this.cardNameWithLastFourDigits); //this.cardNameWithLastFourDigits.Substring(0, this.cardNameWithLastFourDigits.Length - 4).Trim();

                return cardName;
            }
        }

        public string CurrencyCode
        {
            get { return "USD"; }
        }


        public static string FilterAlphabetsFromString(string numericText)
        {
            string allNumerics = string.Empty;
            var regex = new Regex(@"[^A-Za-z]");
            if (!string.IsNullOrEmpty(numericText))
            {
                allNumerics = regex.Replace(numericText, "");
            }
            return allNumerics;

        }


    }

    
}
