using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlType("MOBSHOPRewardProgram")]
    public class RewardProgram
    {
        private string programID;
        private string type = string.Empty;
        private string description = string.Empty;

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string ProgramID
        {
            get
            {
                return programID;
            }
            set
            {
                programID = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

    }
}
