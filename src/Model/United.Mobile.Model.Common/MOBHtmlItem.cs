using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public enum DisplayType
    {
        [EnumMember(Value = "NONE")] //DEFAULT
        NONE,
        [EnumMember(Value = "LINK")] //LINK
        LINK,
        [EnumMember(Value = "DATA")] //DATA
        DATA
    }

    [Serializable]
    public class MOBHtmlItem
    {
        private DisplayType displayKey;       
        private string displayText = string.Empty;
        private string displayLink = string.Empty;
        private string displayData = string.Empty;

        public DisplayType DisplayKey { get { return this.displayKey; } set { this.displayKey = value; } }
        public string DisplayText { get { return this.displayText; } set { this.displayText = value; } }
        public string DisplayLink { get { return this.displayLink; } set { this.displayLink = value; } }
        public string DisplayData { get { return this.displayData; } set { this.displayData = value; } }
    }
}
