using United.Mobile.Model.Common;
using United.Services.FlightShopping.Common.FlightReservation;

namespace United.Mobile.Model.ShopTrips
{
    public class CCEFlightReservationResponseByCartId : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.CCE.CCEFlightReservationResponse";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion
        private FlightReservationResponse cslFlightReservationResponse;

        public FlightReservationResponse CslFlightReservationResponse
        {
            get { return cslFlightReservationResponse; }
            set { cslFlightReservationResponse = value; }
        }
        private string cartId;

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

    }
}
