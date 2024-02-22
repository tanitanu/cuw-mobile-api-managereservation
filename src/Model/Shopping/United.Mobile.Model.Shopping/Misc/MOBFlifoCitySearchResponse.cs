using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoCitySearchResponse : MOBResponse
    {
        public FlifoCitySearchResponse()
            : base()
        {
        }

        private FlifoCitySearchSchedule schedule;

        public FlifoCitySearchSchedule Schedule
        {
            get { return this.schedule; }
            set
            {
                this.schedule = value;
            }
        }
        //public FlifoScheduleTrip[] Schedule;
    }
}
