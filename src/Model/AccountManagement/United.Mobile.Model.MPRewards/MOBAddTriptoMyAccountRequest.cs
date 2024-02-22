using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPRewards
{
    public class MOBAddTriptoMyAccountRequest
    {
        private string arrivalAirportName = string.Empty;
        private string arrivalIATAcode = string.Empty;
        private string createDate = string.Empty;
        private string departureAirportName = string.Empty;
        private string departureIAtACode = string.Empty;
        private string loyaltyMemberID = string.Empty;
        private string recordLocator = string.Empty;

        public string ArrivalAirportName
        {
            get { return this.arrivalAirportName; }
            set { this.arrivalAirportName = value; }
        }

        public string ArrivalIATAcode
        {
            get { return this.arrivalIATAcode; }
            set { this.arrivalIATAcode = value; }
        }

        public string CreateDate
        {
            get { return this.createDate; }
            set { this.createDate = value; }
        }
        public string DepartureAirportName
        {
            get { return this.departureAirportName; }
            set { this.departureAirportName = value; }
        }
        public string DepartureIAtACode
        {
            get { return this.departureIAtACode; }
            set { this.departureIAtACode = value; }
        }
        public string LoyaltyMemberID
        {
            get { return this.loyaltyMemberID; }
            set { this.loyaltyMemberID = value; }
        }
        public string RecordLocator
        {
            get { return this.recordLocator; }
            set { this.recordLocator = value; }
        }
    }
}
