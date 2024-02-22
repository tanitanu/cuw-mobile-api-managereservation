using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{

    [Serializable]
    public enum UpgradeCabinAdvisoryType
    {
        [EnumMember(Value = "NONE")]
        NONE,
        [EnumMember(Value = "WARNING")]
        WARNING,
        [EnumMember(Value = "INFORMATION")]
        INFORMATION,
    }


    [Serializable]
    public enum UpgradeCabinContentType
    {
        [EnumMember(Value = "NONE")]
        NONE,
        [EnumMember(Value = "ELIGIBILITYSERVICEERROR")]
        ELIGIBILITYSERVICEERROR,        
        [EnumMember(Value = "PPOINTSPARTIALEXPIRY")]
        PPOINTSPARTIALEXPIRY,
        [EnumMember(Value = "PPOINTSFULLEXPIRY")]
        PPOINTSFULLEXPIRY,
        [EnumMember(Value = "PPOINTSEVERGREEN")]
        PPOINTSEVERGREEN,        
        [EnumMember(Value = "CABINOPTIONNOTSELECTED")]
        CABINOPTIONNOTSELECTED,
        [EnumMember(Value = "CABINOPTIONNOTLOADED")]
        CABINOPTIONNOTLOADED,        
        [EnumMember(Value = "MILESDOUBLEUPGRADEDETAIL")]
        MILESDOUBLEUPGRADEDETAIL,
    }


    [Serializable]
    public class MOBUpgradeCabinAdvisory
    {
        private UpgradeCabinAdvisoryType advisoryType;
        private UpgradeCabinContentType contentType;
        private string header;
        private string body;           
        private bool shouldExpand;        
        private List<MOBItem> bodyItems;
        public UpgradeCabinAdvisoryType AdvisoryType { get { return this.advisoryType; } set { this.advisoryType = value; } }
        public UpgradeCabinContentType ContentType { get { return this.contentType; } set { this.contentType = value; } }
        public string Header { get { return this.header; } set { this.header = value; } }
        public string Body { get { return this.body; } set { this.body = value; } }        
        public Boolean ShouldExpand { get { return this.shouldExpand; } set { this.shouldExpand = value; } }        
        public List<MOBItem> BodyItems { get { return this.bodyItems; } set { this.bodyItems = value; } }
    }
}
