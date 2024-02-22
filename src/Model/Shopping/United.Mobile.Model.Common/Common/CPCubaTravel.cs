using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CPCubaTravel
    {
        public List<MOBItem> CubaTravelTitles { get; set; }

        public List<MOBCPCubaTravelReason> TravelReasons { get; set; }
        public CPCubaTravel()
        {
            CubaTravelTitles = new List<MOBItem>();
            TravelReasons = new List<MOBCPCubaTravelReason>();
        }
    }
}
