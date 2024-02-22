using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class TravelersProfileResponse : Base
    {
        public List<TravelerProfileResponse> Travelers
        {
            get;
            set;
        }
    }
}
