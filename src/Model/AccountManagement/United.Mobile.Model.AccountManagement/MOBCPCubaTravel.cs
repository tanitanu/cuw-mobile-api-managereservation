using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable()]
    public class MOBCPCubaTravel
    {
        private List<MOBItem> cubaTravelTitles;

        public List<MOBItem> CubaTravelTitles
        {
            get { return cubaTravelTitles; }
            set { cubaTravelTitles = value; }
        }


        private List<MOBCPCubaTravelReason> travelReasons;

        public List<MOBCPCubaTravelReason> TravelReasons
        {
            get { return travelReasons; }
            set { travelReasons = value; }
        }



    }
}
