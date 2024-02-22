using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.FlightSearchResult
{
    public class FlightPBTResponse
    {
       public  List<SegmentPBT> SegmentPBTs { get; set; }
       public FlightPBTRequest FlightPBTRequest { get; set; }
    }
}
