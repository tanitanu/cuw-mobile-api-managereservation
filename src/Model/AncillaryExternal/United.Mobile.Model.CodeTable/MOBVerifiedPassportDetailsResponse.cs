using System;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBVerifiedPassportDetailsResponse : MOBResponse
    {
        public bool IsPassportVerified { get; set; }

        public int InsertedRowID { get; set; }

        public MOBVerifiedPassportDetails VerifiedPassportDetails { get; set; }
    }
}
