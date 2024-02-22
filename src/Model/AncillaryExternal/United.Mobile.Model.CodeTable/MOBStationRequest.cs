using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBStationRequest : MOBRequest
    {
        public string AvailableFlag { get; set; } = string.Empty;

        public int VersionToUpdate { get; set; }

    }
}
