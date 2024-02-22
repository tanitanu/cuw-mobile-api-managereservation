namespace United.Mobile.Model.SeatMap
{
    public class PreferencesResult<T>
    {
        /// <summary>Gets the errors.</summary>
        /// <value>The error.</value>
        public int ErrorNumber { get; set; }

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        public T Data { get; set; }
    }
}
