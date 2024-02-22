using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.TravelerMisConnect
{
    public class TravelerMisConnectResponse : EResBaseResponse
    {
        /// <summary>
        /// Gets or sets the MisConnect Msg
        /// </summary>
        public string MisConnectMsg { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the MisConnect Info
        /// </summary>
        public List<MisConnectInfo> MisConnectInfo { get; set; } = new List<MisConnectInfo>();
    }

    /// <summary>
    ///  Class for Traveler MisConnect Response
    /// </summary>
    public class MisConnectInfo
    {
        /// <summary>
        /// Gets or sets the Flight Info
        /// </summary>
        public string FlightInfo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Flight is delayed or cancelled
        /// </summary>
        public string FlightStatus { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Passenger MisConnect Count
        /// </summary>
        public string PassengerMisConnectCount { get; set; } = string.Empty;
    }
}
