using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class CCERequest
    {
        public bool IsSignedIn { get; set; }
        public int IPDma { get; set; }
        public string IPCountry { get; set; }
        //public string IPStateCS { get; set; }
        public string IPCity { get; set; }
        public string Url { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserPlatform { get; set; }
        public string Browser { get; set; }
        public string OriginAirport { get; set; }
        public string ChannelType { get; set; }
        public string MileagePlusId { get; set; }
        //public string CustomerId { get; set; }
        public string LangCode { get; set; }
        public string CartId { get; set; }
        public string SessionId { get; set; }
        public bool IsRemembered { get; set; }
        public string Currency { get; set; }
        public string PageToLoad { get; set; }
        public List<string> ComponentsToLoad { get; set; }
        public List<string> DataToLoad { get; set; }
        public CCERequestor Requestor { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public string AirportCode { get; set; }
        public List<string> PnrsToLoad { get; set; }
        public string CaCardType { get; set; }
        public string CaCardPNR { get; set; }
        //MPNUMBER, HASH, TOKEN. 
        public Dictionary<string, string> ValidationParameters { get; set; }
    }

    public class CCERequestor
    {
        public List<CCECharacteristic> Characteristic { get; set; }
    }

    public class CCECharacteristic
    {
        public string Code { get; set; }

        public string Value { get; set; }
    }

    public class GeoLocation
    {
        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
