using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// Traveler.
    /// </summary>
    public class Traveler
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
        /// Gets or sets the list of preferences for segments.
        /// </summary>
        public ICollection<PreferenceSegment> PreferenceSegments { get; set; }
    }
}
