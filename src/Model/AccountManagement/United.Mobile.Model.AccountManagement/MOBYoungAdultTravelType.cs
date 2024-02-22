using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBYoungAdultTravelType
    {
        private List<MOBTravelType> youngAdultTravelTypes;
        private bool isYoungAdultTravel;

        public List<MOBTravelType> YoungAdultTravelTypes
        {
            get { return youngAdultTravelTypes; }
            set { youngAdultTravelTypes = value; }
        }

        public bool IsYoungAdultTravel
        {
            get { return isYoungAdultTravel; }
            set { isYoungAdultTravel = value; }
        }
    }
}
