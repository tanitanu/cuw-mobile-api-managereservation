using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    public class PNRPassRider
    {
        public bool CanSelectSeat { get; set; } = false;
        public string SelectSeatURL { get; set; } = string.Empty;
        public string PassRiderName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<EResCancelReservationSeatInfo> Seats { get; set; } = null;
        public string PassClass { get; set; } = string.Empty;
        public RelationshipObject RelationShip { get; set; } = new RelationshipObject();
        public string Position { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
    }
}