using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{

    [Serializable]
    [XmlType("MOBSHOPSegmentInfoAlerts")]
    public class SegmentInfoAlerts
    {
        public string AlertMessage { get; set; } = string.Empty;
       
        public bool AlignLeft { get; set; }
        
        public bool AlignRight { get; set; }
       
        public string AlertType { get; set; } = string.Empty;
        
        public string SortOrder { get; set; } = string.Empty;
        public string Visibility { get; set; } 
        public string Key { get; set; }

    }

    public enum SegmentInfoAlertsOrder
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
}
