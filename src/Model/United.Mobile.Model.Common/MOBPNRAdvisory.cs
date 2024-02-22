using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public enum AdvisoryType
    {
        [EnumMember(Value = "NONE")] //DEFAULT
        NONE,
        [EnumMember(Value = "WARNING")] //RED
        WARNING,
        [EnumMember(Value = "INFORMATION")] //BLUE
        INFORMATION,
        [EnumMember(Value = "CAUTION")] //YELLOW
        CAUTION,
        [EnumMember(Value = "ECO_ALERT")] //YELLOW
        ECO_ALERT
    }

    [Serializable]
    public enum ContentType
    {
        [EnumMember(Value = "SCHEDULECHANGE")]
        SCHEDULECHANGE,
        [EnumMember(Value = "POLICYEXCEPTION")]
        POLICYEXCEPTION,
        [EnumMember(Value = "INCABINPET")]
        INCABINPET,
        [EnumMember(Value = "MAX737WAIVER")]
        MAX737WAIVER,
        [EnumMember(Value = "TRAVELWAIVERALERT")]
        TRAVELWAIVERALERT,
        [EnumMember(Value = "FACECOVERING")]
        FACECOVERING,
        [EnumMember(Value = "MILESINSUFFICIENT")]
        MILESINSUFFICIENT,
        [EnumMember(Value = "MILESWELCOMEMSG")]
        MILESWELCOMEMSG,
        [EnumMember(Value = "PPOINTSINSUFFICIENT")]
        PPOINTSINSUFFICIENT,
        [EnumMember(Value = "PPOINTSWELCOMEMSG")]
        PPOINTSWELCOMEMSG,
        [EnumMember(Value = "PPOINTSPARTIALEXPIRY")]
        PPOINTSPARTIALEXPIRY,
        [EnumMember(Value = "PPOINTSFULLEXPIRY")]
        PPOINTSFULLEXPIRY,
        [EnumMember(Value = "PPOINTSUSERNOTE")]
        PPOINTSUSERNOTE,
        [EnumMember(Value = "MIXEDINSUFFICIENT")]
        MIXEDINSUFFICIENT,
        [EnumMember(Value = "SKIPWAITLIST")]
        SKIPWAITLIST,
        [EnumMember(Value = "CABINOPTIONNOTSELECTED")]
        CABINOPTIONNOTSELECTED,
        [EnumMember(Value = "CABINOPTIONNOTLOADED")]
        CABINOPTIONNOTLOADED,
        [EnumMember(Value = "RESHOPNEWTRIP")]
        RESHOPNEWTRIP,
        [EnumMember(Value = "FUTUREFLIGHTCREDIT")]
        FUTUREFLIGHTCREDIT,
        [EnumMember(Value = "FFCRRESIDUAL")]
        FFCRRESIDUAL,
        [EnumMember(Value = "TRAVELREADY")]
        TRAVELREADY,
        [EnumMember(Value = "OTFCONVERSION")]
        OTFCONVERSION,
        [EnumMember(Value = "IRROPS")]
        IRROPS,
        [EnumMember(Value = "MILESMONEY")]
        MILESMONEY,
        [EnumMember(Value = "JSENONCONVERTEABLEPNR")]
        JSENONCONVERTEABLEPNR,
        [EnumMember(Value = "TRH")]
        TRH, 
        [EnumMember(Value = "JSX")]
        JSX,
        [EnumMember(Value = "SAFTHANKYOUMESSAGE")]
        SAFTHANKYOUMESSAGE,
        [EnumMember(Value = "CHANGESEATCONFIRMATIONMESSAGE")]
        CHANGESEATCONFIRMATIONMESSAGE
    }

    [Serializable]
    public class MOBPNRAdvisory
    {
        private AdvisoryType advisoryType;
        private ContentType contentType;
        private MOBDisplayType displayType;
        private string header;
        private string subTitle;
        private string body;
        private string footer;
        private string buttontext;
        private string buttonlink;
        private bool isBodyAsHtml;
        private bool shouldExpand = true;
        private bool isDefaultOpen = true;
        private List<MOBItem> buttonItems;
        private List<MOBItem> subItems;


        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public AdvisoryType AdvisoryType { get { return this.advisoryType; } set { this.advisoryType = value; } }
        public MOBDisplayType DisplayType { get { return this.displayType; } set { this.displayType = value; } }

        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public ContentType ContentType { get { return this.contentType; } set { this.contentType = value; } }

        public string Header { get { return this.header; } set { this.header = value; } }
        public string SubTitle { get { return this.subTitle; } set { this.subTitle = value; } }
        public string Body { get { return this.body; } set { this.body = value; } }
        public string Footer { get { return this.footer; } set { this.footer = value; } }
        public string Buttontext { get { return this.buttontext; } set { this.buttontext = value; } }
        public string Buttonlink { get { return this.buttonlink; } set { this.buttonlink = value; } }
        public bool IsBodyAsHtml { get { return this.isBodyAsHtml; } set { this.isBodyAsHtml = value; } }
        public bool IsDefaultOpen { get { return this.isDefaultOpen; } set { this.isDefaultOpen = value; } }
        public Boolean ShouldExpand { get { return this.shouldExpand; } set { this.shouldExpand = value; } }
        public List<MOBItem> ButtonItems { get { return this.buttonItems; } set { this.buttonItems = value; } }
        public List<MOBItem> SubItems { get { return this.subItems; } set { this.subItems = value; } }

    }
}
