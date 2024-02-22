using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusSegmentResponse : MOBResponse
    {
        private FlightStatusSegment flightStatusSegment;



        public FlightStatusSegmentResponse()
            : base()
        {
        }


        public FlightStatusSegment FlightStatusSegment
        {
            get
            {
                return this.flightStatusSegment;
            }
            set
            {
                this.flightStatusSegment = value;
            }
        }



    }
}
