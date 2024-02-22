using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoCitySearchSchedule
    {
        private string date = string.Empty;
        private List<FlifoCitySearchTrip> trips = new List<FlifoCitySearchTrip>();
        private AirportAdvisoryMessage airportAdvisoryMessage;

        public string Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<FlifoCitySearchTrip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
        }

        public AirportAdvisoryMessage AirportAdvisoryMessage
        {
            get
            {
                return this.airportAdvisoryMessage;
            }
            set
            {
                this.airportAdvisoryMessage = value;
            }
        }    
    }
}
