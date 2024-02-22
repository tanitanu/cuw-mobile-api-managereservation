using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.TravelerMisConnect
{
    public class TravelerMisConnectRequest : EResBaseRequest
    {
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Get or Set for Destination
        /// </summary>
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Get or Set for FlightDate
        /// </summary>
        public string FlightDate { get; set; } = string.Empty;

        /// <summary>
        /// Get or Set for FlightTime
        /// </summary>
        public string FlightTime { get; set; } = string.Empty;

        /// <summary>
        /// Get or Set for FlightNumber
        /// </summary>
        public string FlightNumber { get; set; } = string.Empty;

        /// <summary>
        /// Get or Set for CarrierCode
        /// </summary>
        public string CarrierCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Equipment
        /// </summary>
        public string EquipmentCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the EquipmentDesc
        /// </summary>
        public string EquipmentDesc { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the TailNumber
        /// </summary>
        public string TailNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether it is ShowPosition 
        /// </summary>
        public bool ShowPosition { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether request is fr mobile view.
        /// </summary>
        public bool IsMobile { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether Show Aircraft Coming From detail is needed.
        /// </summary>
        public bool ShowFlightStatus { get; set; } = false;

        /// <summary>
        /// Gets or sets a value TravelerListIndicator.
        /// </summary>
        public string TravelerListIndicator { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value TravelerListOption.
        /// </summary>
        public string TravelerListOption { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether flight is connecting or not
        /// </summary>
        public bool IsConnectingFlight { get; set; } = false;

        /// <summary>
        /// Gets or sets the operating carrier
        /// </summary>
        public string OperatingCarrier { get; set; } = string.Empty;
    }
}
