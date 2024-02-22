using System;
using System.Collections.Generic;


namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSeatB
    {
        public MOBSeatB()
        {

        }
        public MOBSeatB(string number, string seatvalue, string fee, bool exit, bool limitedRecline, bool isEPlus)
        {
            Number = number;
            this.seatvalue = seatvalue;
            Fee = fee;
            Exit = exit;
            LimitedRecline = limitedRecline;
            IsEPlus = isEPlus;
        }

        private string displaySeatFeature = string.Empty;
        public string DisplaySeatFeature
        {
            get
            {
                return this.displaySeatFeature.Trim().Replace("®", "").Replace("℠", "");
            }
            set
            {
                this.displaySeatFeature = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplaySeatFeatureWithSymbols { get; set; } = string.Empty;

        private bool isHighestEPlus;
        public bool IsHighestEPlus
        {
            get { return isHighestEPlus; }
            set { isHighestEPlus = value; }
        }

        private bool isLowestEPlus;
        public bool IsLowestEPlus
        {
            get { return isLowestEPlus; }
            set { isLowestEPlus = value; }
        }

        private List<ServicesAndFees> servicesAndFees;
        public List<ServicesAndFees> ServicesAndFees
        {
            get { return servicesAndFees; }
            set { servicesAndFees = value; }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        //private string seatvalue;

        public string seatvalue
        {
            get;
            set;
        }

        private string fee;
        public string Fee
        {
            get { return fee; }
            set { fee = value; }
        }

        private bool exit;
        public bool Exit
        {
            get { return exit; }
            set { exit = value; }
        }

        private bool limitedRecline;
        public bool LimitedRecline
        {
            get { return limitedRecline; }
            set { limitedRecline = value; }
        }

        private bool isEPlus;
        public bool IsEPlus
        {
            get { return isEPlus; }
            set { isEPlus = value; }
        }

        private string program;
        public string Program
        {
            get
            {
                return this.program;
            }
            set
            {
                this.program = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private List<MOBCharacteristic> characteristics;
        public List<MOBCharacteristic> Characteristics
        {
            get { return this.characteristics; }
            set { this.characteristics = value; }
        }


        private string seatFeatureInfo;
        public string SeatFeatureInfo
        {
            get { return seatFeatureInfo; }
            set { seatFeatureInfo = value; }
        }

        private List<string> seatFeatureList;
        public List<string> SeatFeatureList
        {
            get { return seatFeatureList; }
            set { seatFeatureList = value; }
        }

        private bool isPcuOfferEligible;
        public bool IsPcuOfferEligible
        {
            get { return isPcuOfferEligible; }
            set { isPcuOfferEligible = value; }
        }

        private string pcuOfferPrice;
        public string PcuOfferPrice
        {
            get { return pcuOfferPrice; }
            set { pcuOfferPrice = value; }
        }

        private string pcuOfferOptionId;
        public string PcuOfferOptionId
        {
            get { return pcuOfferOptionId; }
            set { pcuOfferOptionId = value; }
        }

        private Int32 miles;
        public Int32 Miles
        {
            get { return miles; }
            set { miles = value; }
        }

        private string displayMiles;
        public string DisplayMiles
        {
            get { return displayMiles; }
            set { displayMiles = value; }
        }

        private bool isNoUnderSeatStorage;
        public bool IsNoUnderSeatStorage
        {
            get { return isNoUnderSeatStorage; }
            set { isNoUnderSeatStorage = value; }
        }

        public bool DoorExit { get; set; }
        public string MonumentType { get; set; }
        public string HorizontalSpan { get; set; }
        public string Location { get; set; } = "";

        private List<TravelerPricing> travelerPricing;
        public List<TravelerPricing> TravelerPricing
        {
            get { return travelerPricing; }
            set { travelerPricing = value; }
        }

        private string familySeatingText;
        public string FamilySeatingText
        {
            get { return familySeatingText; }
            set { familySeatingText = value; }
        }
    }
}
