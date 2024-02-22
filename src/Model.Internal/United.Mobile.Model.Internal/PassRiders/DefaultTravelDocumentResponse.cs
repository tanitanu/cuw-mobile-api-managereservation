using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class DefaultTravelDocumentResponse: EResBaseResponse
    {
        public List<TravelDocument> TravelDocuments { get; set; }
    }
}
