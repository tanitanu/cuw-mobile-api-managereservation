using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class SpecialNeeds : EResBaseResponse
    {
        public List<SpecialService> SpecialService { get; set; }
        public EResAlert PetInCabinMessage { get; set; }
    }
}
