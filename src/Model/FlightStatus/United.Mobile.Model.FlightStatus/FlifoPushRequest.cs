using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class FlifoPushRequest : MOBRequest
    {
        public string PushToken { get; set; }
    }
}
