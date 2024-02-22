using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopRegisterTravelersRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public List<ResTraveler> Travelers { get; set; }
        public List<MOBComTelephone> Phones { get; set; }
        public bool LastTraveler { get; set; }
        public ShopRegisterTravelersRequest()
        {
            //Travelers = new List<ResTraveler>();
            Phones = new List<MOBComTelephone>();
        }
    }
}
