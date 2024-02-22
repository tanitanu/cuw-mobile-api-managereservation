using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBFareComparison
    {
        private string fareFamily = string.Empty; // Economy or Economy FullyRefundable

        private MOBFareHeader fareHeader;

        private List<MOBAirlineTravelInfo> airlineTravelInfo;

        private MOBFareTypeComparison fareCompareOption;

        private string interimScreenCode;

        public string InterimScreenCode
        {
            get { return interimScreenCode; }
            set { interimScreenCode = value; }
        }

        public MOBFareTypeComparison FareCompareOption
        {
            get { return fareCompareOption; }
            set { fareCompareOption = value; }
        }


        public string FareFamily
        {
            get { return fareFamily; }
            set { fareFamily = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }


        public List<MOBAirlineTravelInfo> AirlineTravelInfo
        {
            get { return airlineTravelInfo; }
            set { airlineTravelInfo = value; }
        }


        public MOBFareHeader FareHeader
        {
            get { return fareHeader; }
            set { fareHeader = value; }
        }

    }
    [Serializable()]
    public class MOBFareHeader
    {
        private string title; // Confirm experience
        private string header; // This flight is operated by our partner airline, XXX.
        private List<SegmentInfoAlerts> infoDescriptions;
        [XmlArrayItem("MOBSHOPSegmentInfoAlerts")]
        public List<SegmentInfoAlerts> InfoDescriptions
        {
            get { return infoDescriptions; }
            set { infoDescriptions = value; }
        }


        public string Header
        {
            get { return header; }
            set { header = value; }
        }


        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }
    [Serializable()]
    public class MOBAirlineTravelInfo
    {
        private string OptionIcon = string.Empty;
        private string OptionTitle = string.Empty;
        private string OptionDescription = string.Empty;


        public string optionTitle
        {
            get { return OptionTitle; }
            set { OptionTitle = value; }
        }


        public string optionIcon
        {
            get { return OptionIcon; }
            set { OptionIcon = value; }
        }
        public string optionDescription
        {
            get { return OptionDescription; }
            set { OptionDescription = value; }
        }
    }
    [Serializable()]
    public class DisplayAirlineContent
    {

        private List<MOBAirlineTravelInfo> displayContent = null;
        public List<MOBAirlineTravelInfo> DisplayContent
        {
            get { return displayContent; }
            set { displayContent = value; }
        }
    }
    [Serializable()]
    public class MOBFareTypeComparison
    {
        private string title;

        private List<MOBSegmentCabin> segmentCabins;

        private List<MOBAirlineAmenities> airlineAmenities;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        public List<MOBAirlineAmenities> AirlineAmenities
        {
            get { return airlineAmenities; }
            set { airlineAmenities = value; }
        }
        public List<MOBSegmentCabin> SegmentCabins
        {
            get { return segmentCabins; }
            set { segmentCabins = value; }
        }


    }
    [Serializable()]
    public class MOBSegmentCabin
    {
        private string cabinName;
        private string cabinCode;

        public string CabinCode
        {
            get { return cabinCode; }
            set { cabinCode = value; }
        }


        public string CabinName
        {
            get { return cabinName; }
            set { cabinName = value; }
        }
    }
    [Serializable()]
    public class MOBAirlineAmenities
    {
        private string amenityIcon = string.Empty;
        private string amenityTitle = string.Empty;
        private List<MOBAmenitiesDetails> amenitiesDetails;

        public string AmenityTitle
        {
            get { return amenityTitle; }
            set { amenityTitle = value; }
        }


        public string AmenityIcon
        {
            get { return amenityIcon; }
            set { amenityIcon = value; }
        }

        public List<MOBAmenitiesDetails> AmenitiesDetails
        {
            get { return amenitiesDetails; }
            set { amenitiesDetails = value; }
        }


    }
    [Serializable()]
    public class MOBAmenitiesDetails
    {
        private string cabinCode;
        private bool showMessage;
        private string message;
        private bool isAvailable;

        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }


        public bool ShowMessage
        {
            get { return showMessage; }
            set { showMessage = value; }
        }


        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string CabinCode
        {
            get { return cabinCode; }
            set { cabinCode = value; }
        }

    }

}
