using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// Preferences for given a customer.
    /// </summary>
    public class PreferenceSegment
    {
        private const string Format = "ddMMMyyyy";

        /// <summary>
        /// Gets or sets the Carrier Code.
        /// </summary>
        public string CarrierCode { get; set; }

        /// <summary>
        /// Gets or sets the Flight Number.
        /// </summary>
        public int FlightNumber { get; set; }

        /// <summary>
        /// Gets or sets the Departure Date.
        /// </summary>
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Gets or sets the Origin.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets or sets the Destination.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Gets or sets the list of preferences.
        /// </summary>
        public ICollection<Preferences> Preferences { get; set; } = new Collection<Preferences>();

        /// <summary>
        /// Segment flight key.
        /// </summary>
        /// <returns>Retunrs flight key.</returns>
        public string FlightKey() =>
         $"{this.CarrierCode}{this.FlightNumber:D4}{this.DepartureDate.ToString(Format, CultureInfo.InvariantCulture)}"
                .ToUpperInvariant();
    }
}
