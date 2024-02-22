using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopAmenitiesRequest : IPersist
    {
        #region IPersist Members
        public string ObjectName { get; set; } = "United.Persist.Definition.Shopping.ShopAmenitiesRequest";
        #endregion
        public string SessionId { get; set; }
        public string CartId { get; set; }
        public List<string> TripIndexKeys { get; set; }
        public SerializableDictionary<string, United.Services.FlightShopping.Common.UpdateAmenitiesIndicatorsRequest> AmenitiesIndicatorsRequest { get; set; }
    }
}