using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBTNCItem : MOBSection
    {
        private List<MOBMobileCMSContentMessages> tnCList;

        public List<MOBMobileCMSContentMessages> TnCList
        {
            get { return tnCList; }
            set { tnCList = value; }
        }
    }
}
