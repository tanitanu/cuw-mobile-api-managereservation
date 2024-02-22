using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
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
        public bool isDefaultOpen { get; set; }
    }

    [Serializable]
    public enum MOBDisplayType
    {

        [EnumMember(Value = "MAPPURL")]
        MAPPURL = 0,
        [EnumMember(Value = "MAPPCANCEL")]
        MAPPCANCEL = 1,
        [EnumMember(Value = "WEBURL")]
        WEBURL = 2,
        [EnumMember(Value = "PHONE")]
        PHONE = 3,
        [EnumMember(Value = "NONE")]
        NONE = 4,
        [EnumMember(Value = "MAPPCHANGE")]
        MAPPCHANGE = 5,
        [EnumMember(Value = "AOD")]
        AOD = 6,
    }

    [Serializable]
    public enum MOBScheduleChangeDisplayId
    {
        [EnumMember(Value = "NONE")]
        [Display(Name = "None")]
        NONE,

        [EnumMember(Value = "SCHEDULECHANGE")]
        [Display(Name = "Review changes to your trip")]
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

