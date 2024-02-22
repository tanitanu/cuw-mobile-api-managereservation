using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
namespace United.Mobile.Model.Internal.Booking
{
    public class PNRPassRider
    {
        public bool CanSelectSeat { get; set; } = false;
        public string SelectSeatURL { get; set; } = string.Empty;
        public string PassRiderName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Seat> Seats { get; set; } = null;
        public string PassClass { get; set; } = string.Empty;
        public Common.RelationShip RelationShip { get; set; } = new Common.RelationShip();
        public string Position { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
    }
}
