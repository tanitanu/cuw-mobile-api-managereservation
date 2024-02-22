using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    public class Enum
    {
        public enum MOBStyledColor
        {
            [Description("#FF 000000")]
            Black,
            [Description("#FF FFC558")]
            Yellow,
            [Description("#FF 1D7642")]
            Green,
            [Description("#00 000000")]
            Clear
        }

        public enum MOBFlightBadgeSortOrder
        {
            CovidTestRequired
        }
        public enum MOBFlightProductBadgeSortOrder
        {
            Specialoffer,
            MixedCabin,
            YADiscounted,
            CorporateDiscounted,
            MyUADiscounted,
            BreakFromBusiness
        }
        public enum MOBSHOPSegmentInfoAlertsOrder
        {
            ArrivesNextDay,
            TerminalChange,
            RedEyeFlight,
            LongLayover,
            RiskyConnection,
            GovAuthority,
            AirportChange,
            TicketsLeft,
            NearByAirport,
            AirlineFareCompare
        }

        public enum MOBSHOPSegmentInfoDisplay
        {
            FSRCollapsed, // Display only at flgiht block level
            FSRExpanded, // Display only at Flight Details level
            FSRAll // Display in both Flight block and details level
        }
    }
}
