using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// Preferences for given a customer.
    /// </summary>
    public class PersistCustomerPreferences
    {
        /// <summary>
        /// Gets or sets the Record Locator.
        /// </summary>
        public string RecordLocator { get; set; }

        /// <summary>
        /// Gets or sets the list of travelers.
        /// </summary>
        public ICollection<Traveler> Travelers { get; set; }
    }
}
