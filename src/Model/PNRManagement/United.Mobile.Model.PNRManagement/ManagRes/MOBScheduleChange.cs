using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace United.Mobile.Model.ManagRes
{

    public class MOBIRROPSChange
    {
        public string displayHeader { get; set; }
        public string displayBody { get; set; }
        public string displayFooter { get; set; }                
        public List<MOBDisplayItem> displayOptions { get; set; }   
        public bool isHtmlBodyText { get; set; }    
    }

    public class MOBScheduleChange
    {
        public string displayHeader { get; set; }
        public string displayBody { get; set; }
        public string displayFooter { get; set; }
        public string displayBtnText { get; set; }
        public string displayContent { get; set; }
        public List<MOBDisplayItem> displayOptions { get; set; }
        public string displayCity { get; set; }
        public bool shouldExpand { get; set; }
        public List<MOBDisplayItem> displayItems { get; set; }
        public bool isHtmlBodyText { get; set; }
        public int segmentNumber { get; set; }
        public string tripNumber { get; set; }
    }

    
    public class MOBDisplayItem
    {
        public string id { get; set; }
        public string displayType { get; set; }
        public string displayValue { get; set; }
        public string labelText { get; set; }
        public string displayText { get; set; }
        public string displaySubText { get; set; }
    }

    [Serializable]   
    public enum MOBDisplayType
    {
        [EnumMember(Value = "NONE")] 
        NONE,
        [EnumMember(Value = "WEBURL")]
        WEBURL,
        [EnumMember(Value = "PHONE")] 
        PHONE,
        [EnumMember(Value = "MAPPURL")]
        MAPPURL,
    }

    [Serializable]
    public enum MOBScheduleChangeDisplayId
    {
        [EnumMember(Value = "NONE")]
        [Display(Name = "None")]
        NONE,

        [EnumMember(Value = "SCHEDULECHANGE")]
        [Display(Name = "Schedule Change")]
        SCHEDULECHANGE,

        [EnumMember(Value = "DEPARTURETIME")]
        [Display(Name = "Previous departure date/time:")]
        DEPARTURETIME,

        [EnumMember(Value = "DEPARTURECITY")]
        [Display(Name = "Previous departure airport:")]
        DEPARTURECITY,

        [EnumMember(Value = "ARRIVALTIME")]
        [Display(Name = "Previous arrival date/time:")]
        ARRIVALTIME,

        [EnumMember(Value = "ARRIVALCITY")]
        [Display(Name = "Previous arrival airport:")]
        ARRIVALCITY,

        [EnumMember(Value = "CONNECTION")]
        [Display(Name = "Previous connection airport: {0}, now non-stop")]
        CONNECTION,

        [EnumMember(Value = "CONNECTIONCHANGE")]
        [Display(Name = "Previous connection airport: {0}, now {1}")]
        CONNECTIONCHANGE,

        [EnumMember(Value = "NONSTOP")]
        [Display(Name = "Previous non-stop, now connection in {0}")]
        NONSTOP,

        [EnumMember(Value = "FLIGHTDURATION")]
        [Display(Name = "Previous flight duration:")]
        FLIGHTDURATION,

        [EnumMember(Value = "AIRCRAFT")]
        [Display(Name = "Previous aircraft:")]
        AIRCRAFT,

        [EnumMember(Value = "CARRIER")]
        [Display(Name = "Previous operating carrier:")]
        CARRIER,

        [EnumMember(Value = "FLIGHTNUMBER")]
        [Display(Name = "Previous flight number{0}:")]
        FLIGHTNUMBER,

        [EnumMember(Value = "CABIN")]
        [Display(Name = "Previous fare class:")]
        CABIN,

        [EnumMember(Value = "KEEPTRIP")]
        [Display(Name = "Keep trip")]
        KEEPTRIP,

        [EnumMember(Value = "CHANGEFLIGHTS")]
        [Display(Name = "Change flights")]
        CHANGEFLIGHTS,

        [EnumMember(Value = "CANCELTRIP")]
        [Display(Name = "Cancel trip")]
        CANCELTRIP
    }
}

