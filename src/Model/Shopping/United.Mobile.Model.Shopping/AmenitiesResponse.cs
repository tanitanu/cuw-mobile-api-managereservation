using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AmenitiesResponse: MOBResponse
    {
        public AmenitiesRequest ShopAmenitiesRequest { get; set; }

        public string CartId { get; set; } = string.Empty;
       
        public List<AmenitiesFlight> AmenitiesFlightList { get; set; }

    }
}
