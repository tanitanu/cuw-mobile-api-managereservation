using United.Mobile.Model.Common;

namespace United.Mobile.Model.CodeTable
{
    public class MOBCountryPhoneRequest : MOBRequest
    {
        public MOBCountryPhoneRequest()
               : base()
        {
        }
        public string mileagePlusNumber = string.Empty;

        public string MileagePlusNumber
        {
            get { return this.mileagePlusNumber; }
            set { this.mileagePlusNumber = value; }
        }
    }
}
