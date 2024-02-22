using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopBoardingTotal
    {
        public int Authorized { get; set; }
        public int Booked { get; set; }
        public int Capacity { get; set; }
        public int GroupBookings { get; set; }
        public int Held { get; set; }
        public int JumpSeat { get; set; }
        public int PositiveSpace { get; set; }
        public int Reserved { get; set; }
        public int RevenueStandby { get; set; }
        public int SpaceAvailable { get; set; }
        public int WaitList { get; set; }
    }
}
