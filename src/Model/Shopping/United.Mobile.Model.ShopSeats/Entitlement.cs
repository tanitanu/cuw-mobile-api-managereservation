using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class Entitlement
    {
        public virtual string EPlusSubscribeEligible { get; set; }
        public virtual string EplusSubscribeRegion { get; set; }
        public virtual string EplusSubscribeType { get; set; }
        public virtual string EPlusSubscribeCompanionCount { get; set; }
        public virtual string EPlusSubscribeOrigin { get; set; }
        public virtual string EPlusSubscribeDestination { get; set; }
        public virtual string EPlusSubscribeFlightNumber { get; set; }
        public virtual string EPlusSubscribeFlightDate { get; set; }
    }
}
