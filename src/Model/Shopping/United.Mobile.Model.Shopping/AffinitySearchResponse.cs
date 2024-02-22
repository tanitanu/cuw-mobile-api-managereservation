using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AffinitySearchResponse //:MOBResponse
    {
        public AffinitySearchRequest Request { get; set; }
       
        public AffinitySearch Results { get; set; }
       
    }

}
