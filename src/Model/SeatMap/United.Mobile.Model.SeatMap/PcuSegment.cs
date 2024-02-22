using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class PcuSegment
    {
        private int segmentNumber;
        private string flightDescription;
        private string formattedPrice;
        private string upgradeDescription;
        private double price;
        private string segmentDescription;
        private string cabinDescription;
        private string noOfTravelersText;
        private double totalPriceForAllTravelers;
        private List<string> productIds;
        private bool isUpgradeFailed;
        private List<PcuUpgradeOption> upgradeOptions;
        private string origin;
        private string destination;
        private string flightNumber;

        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

        public string FlightDescription
        {
            get { return flightDescription; }
            set { flightDescription = value; }
        }

        public string FormattedPrice
        {
            get { return formattedPrice; }
            set { formattedPrice = value; }
        }

        public string UpgradeDescription
        {
            get { return upgradeDescription; }
            set { upgradeDescription = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string SegmentDescription
        {
            get { return segmentDescription; }
            set { segmentDescription = value; }
        }

        public string CabinDescription
        {
            get { return cabinDescription; }
            set { cabinDescription = value; }
        }

        public string NoOfTravelersText
        {
            get { return noOfTravelersText; }
            set { noOfTravelersText = value; }
        }

        public double TotalPriceForAllTravelers
        {
            get { return totalPriceForAllTravelers; }
            set { totalPriceForAllTravelers = value; }
        }

        public List<string> ProductIds
        {
            get { return productIds; }
            set { productIds = value; }
        }

        public bool IsUpgradeFailed
        {
            get { return isUpgradeFailed; }
            set { isUpgradeFailed = value; }
        }

        public List<PcuUpgradeOption> UpgradeOptions
        {
            get { return upgradeOptions; }
            set { upgradeOptions = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }
    }
}
