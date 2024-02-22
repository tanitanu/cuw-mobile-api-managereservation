using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// Traveler Response.
    /// </summary>
    public class TravelerResponse
    {
        /// <summary>
        /// Gets or sets the First Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the list of preferences for segments responses.
        /// </summary>
        public ICollection<PreferenceSegmentResponse> PreferenceSegmentsResponses { get; set; } = new Collection<PreferenceSegmentResponse>();
    }
}
