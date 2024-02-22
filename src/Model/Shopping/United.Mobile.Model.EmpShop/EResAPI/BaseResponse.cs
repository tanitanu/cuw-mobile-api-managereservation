using System;

namespace United.Mobile.Model.Common
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            ServerName = Environment.MachineName;
        }

        /// <summary>
        /// Gets or sets Error information
        /// </summary>
        public ErrorInfo Error { get; set; }

        /// <summary>
        /// Gets or sets Last call date time
        /// </summary>
        public string LastCallDateTime { get; set; } = DateTime.Now.ToString();

        /// <summary>
        /// Gets or sets the server name
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets status such as success or failure.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets TransactionID.
        /// </summary>
        public string TransactionID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the BaseAlerts
        /// </summary>
        public BaseAlert BaseAlert { get; set; }

        /// <summary>
        /// Gets or sets the TransferMessage
        /// </summary>
        public string TransferMessage { get; set; } = string.Empty;

        /// <summary>
        /// check whether the user is allowed to select a flight
        /// </summary>
        public bool IsAllowedToSelectFlight = true;
    }
}