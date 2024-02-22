using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
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
    }

    [Serializable]
    public class Advisory
    {
        private AdvisoryType advisoryType;
        private ContentType contentType;
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
        public AdvisoryType AdvisoryType { get { return this.advisoryType; } set { this.advisoryType = value; } }
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
