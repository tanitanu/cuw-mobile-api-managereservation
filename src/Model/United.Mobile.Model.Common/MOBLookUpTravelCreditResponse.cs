using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBLookUpTravelCreditResponse : MOBResponse
    {
        private MOBFutureFlightCredit futureflightcredit;

        public MOBFutureFlightCredit FutureFlightCredit { get { return this.futureflightcredit; } set { this.futureflightcredit = value; } }
    }
}
