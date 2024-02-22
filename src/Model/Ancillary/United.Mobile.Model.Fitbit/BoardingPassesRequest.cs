using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class BoardingPassesRequest : MOBRequest
    {
        public string MileagePlusNumber { get; set; } = string.Empty;

        public string HashValue { get; set; } = string.Empty;

        public string RecordLocator { get; set; } = string.Empty;
    }
}
