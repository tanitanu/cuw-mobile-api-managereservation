using System;

namespace United.Mobile.Model.UpgradeList
{
    [Serializable()]
    public class DoDOfferEligibilityInfo
    {
        private string recordLocator;
        private string lastName;
        private string data;
        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
