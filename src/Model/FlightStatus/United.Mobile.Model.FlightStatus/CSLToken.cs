using System;

namespace United.Persist.Definition.FlightStatus
{
    public class CSLToken : PersistBase, IPersist
    {
        public CSLToken() { }

        public CSLToken(string json, string objectType)
        {
            Json = json;
            ObjectType = objectType;
        }

        public string ObjectName { get; set; } = "United.Persist.Definition.FlightStatus.CSLToken";

        public int Duration { get; set; }

        public string ApplicationType { get; set; }

        public string Token { get; set; }

        public DateTime ExpirationTime { get; set; }

        public DateTime InsertedDateTime { get; set; }
    }
}
