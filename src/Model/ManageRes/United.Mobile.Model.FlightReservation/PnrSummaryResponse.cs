using System;

namespace United.Mobile.Model.FlightReservation
{
    [Serializable()]
    public class PnrSummaryResponse
    {
        public string id = string.Empty;
        public PnrSummary pnrSummary;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public PnrSummary PnrSummary
        {
            get
            {
                return this.pnrSummary;
            }
            set
            {
                this.pnrSummary = value;
            }
        }

    }
}
