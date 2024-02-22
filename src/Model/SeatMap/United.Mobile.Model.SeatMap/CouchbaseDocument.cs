using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class CouchbaseDocument : CouchbaseDocumentEnvelop
    {
        private string defaultExpDate = string.Empty;

        public string _exp
        {
            get
            {
                if (string.IsNullOrEmpty(defaultExpDate))
                    return (new DateTime(2025, 1, 1)).ToString("yyyy-MM-dd~HH:mm:sszzz").Replace('~', 'T');
                else
                    return defaultExpDate;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    defaultExpDate = (new DateTime(2025, 1, 1)).ToString("yyyy-MM-dd~HH:mm:sszzz").Replace('~', 'T');
                else
                    defaultExpDate = value;
            }
        }

        public string _rev { get; set; }
        public string _id { get; set; }      
        public string Type { get; set; }
        public List<string> Channels { get; set; }
        public int RefreshIntervalInSeconds { get; set; }
        public long EventTimestamp { get; set; }
        public string Bucket { get; set; }
        public string DocumentSavedTimeStamp { get; set; }
        public string ClientTTL { get; set; }
        public ulong Cas { get; set; }
    }
}
