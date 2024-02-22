using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;


namespace United.Mobile.Model.Common
{
    [Serializable]
    public enum MOBPageId
    {
        [EnumMember(Value = "NONE")]
        NONE,

        [EnumMember(Value = "CANADIANTRAVELNUMBER")]
        CANADIANTRAVELNUMBER
    }
    public class MOBPageContent
    {
        public MOBPageId id { get; set; }
        public string displayHeader { get; set; }
        public string displayBody { get; set; }
        public string displayFooter { get; set; }
        public List<MOBDisplayItem> displayOptions { get; set; }
        public bool isHtmlBodyText { get; set; }
    }

}

