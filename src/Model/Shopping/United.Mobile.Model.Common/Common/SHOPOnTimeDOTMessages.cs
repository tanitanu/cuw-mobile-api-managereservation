using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SHOPOnTimeDOTMessages
    {
        public string CancellationPercentageMessage { get; set; } = string.Empty;

        public string DelayPercentageMessage { get; set; } = string.Empty;

        public string DelayAndCancellationPercentageMessage { get; set; } = string.Empty;

        public string DOTMessagePopUpButtonCaption { get; set; } = string.Empty;

    }
}
