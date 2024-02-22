using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBSeatFocus
    {
        private string sharesIndex;
      
        private string origin;
        private string destination;
        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }


        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }


        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }


        public string SharesIndex
        {
            get { return sharesIndex; }
            set { sharesIndex = value; }
        }

       

    }
}
