using System;

namespace United.Mobile.Model.ReShop
{
    [Serializable()]
    public class MOBSHOPRewardProgram
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
