using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubMembershipResponse :MOBResponse
    {
        private ClubMembership membership;

        public ClubMembership Membership
        {
            get
            {
                return this.membership;
            }
            set
            {
                this.membership = value;
            }
        }
    }
}
