namespace United.Mobile.Model.SeatMap
{
    /// <summary>Contains error information.</summary>
    public class ErrorResponse
    {
        /// <summary>Gets or sets the error number.</summary>
        /// <value>The number.</value>
        public int ErrorNumber { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; } = string.Empty;
    }
}
