using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightReservation
{
    [Serializable]
    public class PnrSummaryResponseCollection
    {
        private string id;
        private List<PnrSummaryResponse> pnrSummaryResponses;

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

        public List<PnrSummaryResponse> PnrSummaryResponses
        {
            get
            {
                return this.pnrSummaryResponses;
            }
            set
            {
                this.pnrSummaryResponses = value;
            }
        }
    }
}
