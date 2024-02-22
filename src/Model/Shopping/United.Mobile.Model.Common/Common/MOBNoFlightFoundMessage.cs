using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBNoFlightFoundMessage
    {
        private string noFlightFoundHelpMessageText;
        public string NoFlightFoundHelpMessageText
        {
            get { return noFlightFoundHelpMessageText; }
            set { noFlightFoundHelpMessageText = value ?? string.Empty; }
        }
        private string sectionMessage;
        public string SectionMessage
        {
            get { return sectionMessage; }
            set { sectionMessage = value ?? string.Empty; }
        }
        private string sectionDescription;
        public string SectionDescription
        {
            get { return sectionDescription; }
            set { sectionDescription = value ?? string.Empty; }
        }
        private List<MOBNoFlightOtherSearchOption> noFlightOtherSearchOptions;
        public List<MOBNoFlightOtherSearchOption> NoFlightOtherSearchOptions
        {
            get { return noFlightOtherSearchOptions; }
            set { noFlightOtherSearchOptions = value; }
        }

    }

    [Serializable()]
    public class MOBNoFlightOtherSearchOption
    {
        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value ?? string.Empty; }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value ?? string.Empty; }
        }
        private string bodyText;
        public string BodyText
        {
            get { return bodyText; }
            set { bodyText = value ?? string.Empty; }
        }
        private List<MOBNoFlightButton> buttons;
        public List<MOBNoFlightButton> Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }
        private string imageAccessibilityText;
        public string ImageAccessibilityText
        {
            get { return imageAccessibilityText; }
            set { imageAccessibilityText = value ?? string.Empty; }
        }
    }

    [Serializable()]
    public class MOBNoFlightButton
    {
        private string buttonText;
        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value ?? string.Empty; }
        }
        private string otherRequestData;
        public string OtherRequestData
        {
            get { return otherRequestData; }
            set { otherRequestData = value ?? string.Empty; }
        }
        private string actionType;
        public string ActionType
        {
            get { return actionType; }
            set { actionType = value ?? string.Empty; }
        }

    }
    public enum MOBAvailabiltyResponseType
    {
        [Description("Default")]
        Default = 0,
        [Description("NoFlightFound")]
        NoFlightFound
    }
    public enum MOBSearchButtonActionType
    {
        [Description("OpenUrl")]
        OpenUrl = 0,
        [Description("SearchByRequest")]
        SearchByRequest,
        [Description("SearchByMap")]
        SearchByMap
    }

    public class AirportLookup
    {
        public AirportDataAirport Airport { get; set; }
        public string IsAllAirport { get; set; }
        public string PortOrder { get; set; }
        public string SortOrder { get; set; }
    }

    public class AirportDataAirport
    {
        public AirportsIATACityCode IATACityCode { get; set; }
        public string IATACode { get; set; }
        public AirportsIATACountryCode IATACountryCode { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public AirportsStateProvince StateProvince { get; set; }
    }
    public class AirportsIATACityCode
    {
        public string CityCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class AirportsIATACountryCode
    {
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
    public class AirportsStateProvince
    {
        public string StateProvinceCode { get; set; }
        public string Name { get; set; }
    }

    //public class ShopByMapRequest
    //{
    //    public string Origin { get; set; }
    //    public string Destination { get; set; }
    //    public GeoCoordinate DeviceLocation { get; set; }
    //    public GeoCoordinate NorthEastBound { get; set; }
    //    public GeoCoordinate SouthWestBound { get; set; }
    //    public string SearchType { get; set; }
    //    public string DepartureDate { get; set; }
    //    public string ArrivalDate { get; set; }
    //    public string BudgetPrice { get; set; }
    //    public bool IsNonStop { get; set; }
    //    public bool ShowAllAirports { get; set; }
    //    public bool ShowWhereIBeen { get; set; }
    //    public Collection<string> DecisionMarkingTags { get; set; }
    //    public string SessionId { get; set; }
    //    public string MapLaunch { get; set; }
    //}
}
