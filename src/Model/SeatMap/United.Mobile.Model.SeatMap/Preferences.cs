namespace United.Mobile.Model.SeatMap
{
    /// <summary>
    /// Preferred values for give type.
    /// </summary>
    public class Preferences
    {
        /// <summary>
        /// Gets or sets the Preference type.
        /// </summary>
        public PreferenceType PreferenceType { get; set; }

        /// <summary>
        /// Gets or sets the Preference value.
        /// </summary>
        public PreferenceOption PreferenceValue { get; set; }
    }
}
