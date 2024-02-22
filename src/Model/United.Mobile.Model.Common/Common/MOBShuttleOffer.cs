using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBShuttleOffer : MOBOfferTile
    {

        private string offerCode;
        private string offerText1;
        private string offerText2;
        private double offerPrice;
        private string formattedPrice;
        private string offerText3;
        private string offerTileImageName;
        private string eligibleAirport;
        private string shuttleStation;
        private string shuttleStationDescription;
        private Int32 offerMiles;
        private string displayOfferMiles;
        private string childWarningMessage;
        private string childCutoffAge;
        private bool isSelected;

        public string ChildWarningMessage { get { return this.childWarningMessage; } set { this.childWarningMessage = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string ChildCutoffAge { get { return this.childCutoffAge; } set { this.childCutoffAge = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string OfferCode { get { return this.offerCode; } set { this.offerCode = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string OfferText1 { get { return this.offerText1; } set { this.offerText1 = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string OfferText2 { get { return this.offerText2; } set { this.offerText2 = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public double OfferPrice { get { return this.offerPrice; } set { this.offerPrice = value; } }
        public bool IsSelected { get { return this.isSelected; } set { this.isSelected = value; } }
        public string FormattedPrice { get { return this.formattedPrice; } set { this.formattedPrice = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string OfferText3 { get { return this.offerText3; } set { this.offerText3 = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string OfferTileImageName { get { return this.offerTileImageName; } set { this.offerTileImageName = value; } }
        public string EligibleAirport { get { return this.eligibleAirport; } set { this.eligibleAirport = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string ShuttleStation { get { return this.shuttleStation; } set { this.shuttleStation = string.IsNullOrEmpty(value) ? string.Empty : value; } }
        public string ShuttleStationDescription { get { return this.shuttleStationDescription; } set { this.shuttleStationDescription = string.IsNullOrEmpty(value) ? string.Empty : value; } }       
        public Int32 OfferMiles { get { return this.offerMiles; } set { this.offerMiles = value; } }
        public string DisplayOfferMiles { get { return this.displayOfferMiles; } set { this.displayOfferMiles = value; } }
    }
}
