using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBKOnTimeDOTMessages
    {
        private string cancellationPercentageMessage = string.Empty;
        private string delayPercentageMessage = string.Empty;
        private string delayAndCancellationPercentageMessage = string.Empty;
        private string dotMessagePopUpButtonCaption = string.Empty;

        public string CancellationPercentageMessage
        {
            get
            {
                return this.cancellationPercentageMessage;
            }
            set
            {
                this.cancellationPercentageMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DelayPercentageMessage
        {
            get
            {
                return this.delayPercentageMessage;
            }
            set
            {
                this.delayPercentageMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DelayAndCancellationPercentageMessage
        {
            get
            {
                return this.delayAndCancellationPercentageMessage;
            }
            set
            {
                this.delayAndCancellationPercentageMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DOTMessagePopUpButtonCaption
        {
            get
            {
                return this.dotMessagePopUpButtonCaption;
            }
            set
            {
                this.dotMessagePopUpButtonCaption = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
