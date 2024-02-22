using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class PostSeatPreferenceRequest
    {
        public List<PostSeatPreferences> SeatPreferences { get; set; } = new List<PostSeatPreferences>();
        public List<ReservationSegment> Segments { get; set; }
        public string RecordLocator { get; set; }
        //public string LastName { get; set; }
        //public bool IsRemembered { get; set; }
        //public bool IsSemiSignedIn { get; set; }
        //public string MileagePlusId { get; set; }
        //public string LangCode { get; set; }
        public string SessionId { get; set; }
        public string EntryPoint { get; set; }
        public bool IsStandby { get; set; } = false;
        public bool IsMultiPax { get; set; } = false;
    }

    public class PostSeatPreferences
    {
        /// <summary>
        /// Gets or sets the Preference type.
        /// </summary>
        public string PreferenceType { get; set; }

        /// <summary>
        /// Gets or sets the Preference value.
        /// </summary>
        public string PreferenceValue { get; set; }
    }
}
