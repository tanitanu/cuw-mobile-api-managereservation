using System.Collections.ObjectModel;
using System.Linq;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// PreferenceSegmentResponse.
    /// </summary>
    public class PreferenceSegmentResponse
    {
        /// <summary>Gets a value indicating whether this <see cref="PreferenceSegmentResponse"/> is success.</summary>
        /// <value>
        /// <c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success => !this.Errors.Any();

        /// <summary>Gets or sets the error responses.</summary>
        /// <value>The error responses.</value>
        public Collection<ErrorResponse> Errors { get; set; }

        /// <summary>
        /// Gets or sets the preferences for segment.
        /// </summary>
        public PreferenceSegment PreferenceSegment { get; set; }
    }
}
