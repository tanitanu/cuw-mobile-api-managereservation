using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ShopSeats
{
    public class Seatmap
    {
        public virtual FlightProfile SegmentInfo { get; set; }
        public virtual Aircraft Aircraft { get; set; }
    }
}
