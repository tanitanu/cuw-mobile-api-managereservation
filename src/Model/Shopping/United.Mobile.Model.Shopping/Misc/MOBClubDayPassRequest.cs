using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ClubDayPassRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;
        private string email = string.Empty;
        private string clubPassCodes = string.Empty;

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ClubPassCodes
        {
            get
            {
                return this.clubPassCodes;
            }
            set
            {
                this.clubPassCodes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
