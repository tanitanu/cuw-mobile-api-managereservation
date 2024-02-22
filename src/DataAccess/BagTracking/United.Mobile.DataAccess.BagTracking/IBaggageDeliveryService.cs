using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.BagTracking;

namespace United.Mobile.DataAccess.BagTracking
{
    public interface IBaggageDeliveryService
    {
        Task<BaggageDeliveryDetails> SubmitBaggageDeliveryClaim(string token, BaggageDeliveryRequest request, string sessionId);
    }
}
