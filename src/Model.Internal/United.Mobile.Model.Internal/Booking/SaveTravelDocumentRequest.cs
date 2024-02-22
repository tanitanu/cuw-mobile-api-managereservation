using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Booking
{
    public class SaveTravelDocumentRequest
    {
        public bool IsPassriderCabin { get; set; }
        public string TransactionId { get; set; }
        public List<SSRInfo> SSRInfos { get; set; }
    }
}
