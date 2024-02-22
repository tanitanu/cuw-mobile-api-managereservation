using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class RewardProgramData
    {
        public int ProgramId
        {
            get;
            set;
        }


        public string ProgramMemberId
        {
            get;
            set;
        }
        public string VendorDescription { get; set; }
        public string VendorCode { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
    }
}
