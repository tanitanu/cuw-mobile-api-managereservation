using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class SecureTravelerResponseData
    {
        public SecureTraveler SecureTraveler
        {
            get;
            set;
        }

        public List<SupplementaryTravelDocsDataMembers> SupplementaryTravelInfos
        {
            get;
            set;
        }
    }
}
