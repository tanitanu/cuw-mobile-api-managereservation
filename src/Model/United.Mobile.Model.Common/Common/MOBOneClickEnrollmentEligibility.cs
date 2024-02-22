using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBOneClickEnrollmentEligibility
    {
        private string joinMileagePlusHeader = string.Empty;

        private string joinMileagePlusText = string.Empty;

        private string joinMileagePlus = string.Empty;

        public string JoinMileagePlusHeader { get { return joinMileagePlusHeader; } set { joinMileagePlusHeader = value; } }
        public string JoinMileagePlusText { get { return joinMileagePlusText; } set { joinMileagePlusText = value; } }
        public string JoinMileagePlus { get { return joinMileagePlus; } set { joinMileagePlus = value; } }
    }
}
