using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class AirportAdvisoryResponse :MOBResponse
    {
        private AirportAdvisoryMessage airportAdvisoryMessage;


        public AirportAdvisoryResponse()
            : base()
        {
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
