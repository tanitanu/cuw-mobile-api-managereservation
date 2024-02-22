using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKProfileResponse : MOBResponse
    {
        private List<MOBBKProfile> profiles;
        private ProfileRequest request;

        public ProfileRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        public List<MOBBKProfile> Profiles
        {
            get
            {
                return profiles;
            }
            set
            {
                this.profiles = value;
            }
        }
    }
}
