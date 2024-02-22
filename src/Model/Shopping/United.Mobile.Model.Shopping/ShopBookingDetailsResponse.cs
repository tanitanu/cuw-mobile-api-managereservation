using System;
using United.Mobile.Model.Common;
//using United.Service.Presentation.ReservationModel;
//using United.Services.FlightShopping.Common.FlightReservation;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopBookingDetailsResponse : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.ShopBookingDetailsResponse";
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

        public string SessionId { get; set; }
        public string CartId { get; set; }
        public United.Service.Presentation.ReservationModel.Reservation Reservation { get; set; }
        public United.Service.Presentation.ProductResponseModel.ProductOffer FareLock { get; set; }
    }
}
