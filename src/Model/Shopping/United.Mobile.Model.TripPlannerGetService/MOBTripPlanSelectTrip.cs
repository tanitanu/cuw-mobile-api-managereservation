namespace United.Mobile.Model.TripPlannerGetService
{
    [System.Serializable()]
    public class MOBTripPlanSelectTrip
    {
        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private string productId = string.Empty;


        private string cartId;

        public string CartId
        {
            get
            {
                return cartId;
            }
            set
            {
                cartId = value;
            }
        }


        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightId
        {
            get
            {
                return this.flightId;
            }
            set
            {
                this.flightId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}