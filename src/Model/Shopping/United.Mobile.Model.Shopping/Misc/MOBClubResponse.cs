using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubResponse : MOBResponse
    {
        private string airportCode = string.Empty;
        private string clubType = string.Empty;
        private bool bfcOnly;
        private List<Club> clubs;

        public ClubResponse()
            : base()
        {
        }

        public string AirportCode
        {
            get
            {
                return this.airportCode;
            }
            set
            {
                this.airportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ClubType
        {
            get
            {
                return this.clubType;
            }
            set
            {
                this.clubType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool BFCOnly
        {
            get
            {
                return this.bfcOnly;
            }
            set
            {
                this.bfcOnly = value;
            }
        }

        public List<Club> Clubs
        {
            get
            {
                return this.clubs;
            }
            set
            {
                this.clubs = value;
            }
        }
    }
}
