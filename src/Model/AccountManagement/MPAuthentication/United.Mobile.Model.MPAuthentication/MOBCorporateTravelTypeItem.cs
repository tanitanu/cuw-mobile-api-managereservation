using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBCorporateTravelTypeItem
    {
        private string travelType;
        private string travelTypeDescription;

        public string TravelType
        {
            get
            {
                return travelType;
            }
            set
            {
                travelType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelTypeDescription
        {
            get
            {
                return travelTypeDescription;
            }
            set
            {
                travelTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
