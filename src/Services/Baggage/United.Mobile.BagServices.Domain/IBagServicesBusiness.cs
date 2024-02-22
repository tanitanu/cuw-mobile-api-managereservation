using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.BagServices.Domain
{
    public interface IBagServicesBusiness
    {
        void PostBaggageEventMessage(dynamic request);
    }
}
