using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    public class FireCSLStatistics
    {
       public int ApplicationId { get; set; }
        public string ApplicationVersion { get; set; }
        public string DeviceId { get; set; }
        public string SessionID { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Flight_Date { get; set; }
        public string Trip_Type { get; set; }
        public string ClassOfService { get; set; }
        public string FareOption { get; set; }
        public bool IsAward { get; set; }
        public int NumberOfTravelers { get; set; }
        public string MileagePlusNumber { get; set; }
        public string RecordLocator { get; set; }
        public string Category { get; set; } 
        public string CartID { get; set; }
        public string CallStatistics { get; set; } 
        public string RESTAction { get; set; }
    }
}
