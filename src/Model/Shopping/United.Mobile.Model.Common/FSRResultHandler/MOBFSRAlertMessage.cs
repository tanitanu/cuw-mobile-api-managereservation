using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    public enum FSRAlertMessageType
    {
        None,
        SuggestNearbyAirports,
        NoResults,
        NonstopsSuggestion,
        RevenueLowestPriceForAwardSearch,
        CorporateLeisureOptOut,
        NoChangeFee,
        NoSeatsAvailable
    }

    public enum MOBFSRAlertMessageType
    {
        Warning,
        Success,
        Information,
        Caution
    }

    public enum MOBSHOPSegmentInfoAlertsOrder
    {
        NearByAirport,
        ArrivesNextDay,
        TerminalChange,
        RedEyeFlight,
        LongLayover,
        RiskyConnection,
        GovAuthority,
        AirportChange,
        TicketsLeft
    }

    public enum MOBSHOPSegmentInfoDisplay
    {
        FSRCollapsed, // Display only at flgiht block level
        FSRExpanded, // Display only at Flight Details level
        FSRAll // Display in both Flight block and details level
    }

    [Serializable()]
    [XmlType("MOBFSRAlertMessage")]
    public class MOBFSRAlertMessage
    {
        private string headerMsg;
        private string bodyMsg;
        private List<MOBFSRAlertMessageButton> buttons;
        private FSRAlertMessageType messageTypeDescription = FSRAlertMessageType.None; // default to none
        private int messageType = 0; //default to 0
        private string restHandlerType;
        private string alertType = MOBFSRAlertMessageType.Warning.ToString();

        public string AlertType
        {
            get { return alertType; }
            set { alertType = value; }
        }

        /// <summary>
        /// Header message
        /// </summary>

        [JsonProperty(PropertyName = "headerMsg")]
        [JsonPropertyName("headerMsg")]
        public string HeaderMessage
        {
            get { return headerMsg; }
            set { headerMsg = value; }
        }

        /// <summary>
        /// Body message
        /// </summary>

        [JsonProperty(PropertyName = "bodyMsg")]
        [JsonPropertyName("bodyMsg")]
        public string BodyMessage
        {
            get { return bodyMsg; }
            set { bodyMsg = value; }
        }

        /// <summary>
        /// List of buttons that need to be shown
        /// </summary>
        public List<MOBFSRAlertMessageButton> Buttons
        {
            get { return buttons; }
            set { buttons = value; }
        }

        /// <summary>
        /// The type on message (info/alert/etc)
        /// 0 for info per UI proposal
        /// </summary>
        public FSRAlertMessageType MessageTypeDescription
        {
            get { return messageTypeDescription; }
            set { messageTypeDescription = value; }
        }

        /// <summary>
        /// This is for REST debug purpose only
        /// </summary>
        public string RestHandlerType
        {
            get { return restHandlerType; }
            set { restHandlerType = value; }
        }

        public int MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

    }
}
