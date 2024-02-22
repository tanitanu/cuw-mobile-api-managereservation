using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Pcu;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class GetOffersResponse :MOBResponse
    {
        public PriorityBoarding PriorityBoarding { get; set; }
        public GetOffersRequest Request { get; set; }
        public MOBPremiumCabinUpgrade PremiumCabinUpgrade { get; set; }
        public string SessionId { get; set; } = string.Empty;

    }
}
