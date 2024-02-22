using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AccountProfileInfoResponse
    {
        [DataMember]
        public AccountProfileInfo AccountProfileInfo { get; set; }
    }
}
