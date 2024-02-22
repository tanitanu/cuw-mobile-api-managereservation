using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPPlusPointsResponse : MOBResponse
    {
        private MOBPlusPoints pluspointsDetails;

        public MOBMPPlusPointsResponse()
            : base()
        {
        }


        public MOBPlusPoints PluspointsDetails
        {
            get
            {
                return this.pluspointsDetails;
            }
            set
            {
                this.pluspointsDetails = value;
            }
        }
    }
}
