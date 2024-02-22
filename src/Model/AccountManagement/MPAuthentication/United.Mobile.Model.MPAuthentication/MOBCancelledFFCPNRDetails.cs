using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
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
