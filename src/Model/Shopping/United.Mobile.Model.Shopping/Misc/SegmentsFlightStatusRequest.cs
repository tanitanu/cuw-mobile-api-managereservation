using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class SegmentsFlightStatusRequest : MOBRequest
    {
        public string SegmentParameters { get; set; } = string.Empty;

    }
}
