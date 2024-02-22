using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ClubDayPassResponse :MOBResponse
    {
        private ClubDayPassRequest request;
        private List<ClubDayPass> passes;

        public ClubDayPassRequest Request
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

        public List<ClubDayPass> Passes
        {
            get
            {
                return this.passes;
            }
            set
            {
                this.passes = value;
            }
        }
    }
}
