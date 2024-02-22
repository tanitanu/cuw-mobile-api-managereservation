using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SelectSeats 
    {
        #region IPersist Members

        public string ObjectName { get; set; } = "United.Persist.Definition.Shopping.SelectSeats";


        #endregion

        public string SessionId { get; set; }
        public SerializableDictionary<string, SelectSeatsRequest> Requests { get; set; }
        public SerializableDictionary<string, SelectSeatsResponse> Responses { get; set; }
    }
}
