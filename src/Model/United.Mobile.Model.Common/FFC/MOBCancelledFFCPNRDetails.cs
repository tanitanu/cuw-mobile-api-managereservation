using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCancelledFFCPNRDetails
    {
        private string recordLocator;
        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        private string pnrLastName;

        public string PNRLastName
        {
            get { return pnrLastName; }
            set { pnrLastName = value; }
        }

        private List<MOBName> passengers;
        public List<MOBName> Passengers
        {
            get { return passengers; }
            set { passengers = value; }
        }
    }
}
