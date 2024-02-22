using System.Collections.Generic;
using System.ComponentModel;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    public class PersistSeatPreferenceResponse
    {
        public Dictionary<string, string> Captions { get; set; } = new Dictionary<string, string>();

        public List<OnboardingScreen> OnboardingScreenCaptions { get; set; } = new List<OnboardingScreen>();
        public List<SeatPreferenceOption> SeatPreferences { get; set; } = new List<SeatPreferenceOption>();
        public List<ReservationSegment> Segments { get; set; } = new List<ReservationSegment>();
        public List<MOBPNRPassenger> Passengers { get; set; } = new List<MOBPNRPassenger>();

        public string RecordLocator { get; set; }

        public string LastName { get; set; }
    }

    public class SeatPreferenceOption
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string OptionType { get; set; }

        public string PreferenceType { get; set; }

        public string PreferenceValue { get; set; }

        public List<PreferenceDetail> Details { get; set; } = new List<PreferenceDetail>();

        public bool enableSaveButton { get; set; } = false;
    }

    public enum ButtonType
    {
        [Description("Radio")]
        Radio,
        [Description("Checkbox")]
        Checkbox
    }

    public class PreferenceDetail
    {
        public string PreferenceValue { get; set; }
        public string DisplayText { get; set; }
        public string DisplaySubText { get; set; }
        public bool IsSelected { get; set; }
    }

    public class OnboardingScreen
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;

    }
}
