using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// update Preferences for given a customer response.
    /// </summary>
    public class PersistCustomerPreferencesResponse
    {
        /// <summary>
        /// Gets or sets the Record Locator.
        /// </summary>
        public string RecordLocator { get; set; }

        /// <summary>
        /// Gets or sets the list of traveler responses.
        /// </summary>
        public ICollection<TravelerResponse> TravelerResponses { get; set; } = new Collection<TravelerResponse>();
    }
}
