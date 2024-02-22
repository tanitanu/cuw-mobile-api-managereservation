using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubLocationUpdateResponse :MOBResponse
    {
        private List<AirportClubLocation> airportClubLocations;

        public ClubLocationUpdateResponse()
            : base()
        {
        }

        public List<AirportClubLocation> AirportClubLocations
        {
            get
            {
                return this.airportClubLocations;
            }
            set
            {
                this.airportClubLocations = value;
            }
        }
    }
}
