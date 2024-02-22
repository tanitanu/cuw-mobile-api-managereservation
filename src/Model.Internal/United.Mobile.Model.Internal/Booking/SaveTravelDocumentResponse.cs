using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Booking
{
    public class SaveTravelDocumentResponse:EResBaseResponse
    {
        public List<SSRInfo> SSRInfos { get; set; }
    }
}
