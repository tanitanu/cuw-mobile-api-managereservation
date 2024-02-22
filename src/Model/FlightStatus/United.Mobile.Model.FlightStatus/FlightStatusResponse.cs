namespace United.Mobile.Model.FlightStatus
{
    public class FlightStatusResponse : MOBResponse
    {
        private FlightStatusInfo flightStatusInfo;

        public FlightStatusResponse()
            : base()
        {
        }

        public FlightStatusInfo FlightStatusInfo
        {
            get
            {
                return this.flightStatusInfo;
            }
            set
            {
                this.flightStatusInfo = value;
            }
        }
    }
}