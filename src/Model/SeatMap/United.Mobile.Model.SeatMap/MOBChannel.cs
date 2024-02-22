using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBChannel
    {
        private string channelCode = string.Empty;
        private string channelDescription = string.Empty;
        private string channelTypeCode = string.Empty;
        private string channelTypeDescription = string.Empty;

        public string ChannelCode
        {
            get
            {
                return this.channelCode;
            }
            set
            {
                this.channelCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ChannelDescription
        {
            get
            {
                return this.channelDescription;
            }
            set
            {
                this.channelDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelTypeCode
        {
            get
            {
                return this.channelTypeCode;
            }
            set
            {
                this.channelTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ChannelTypeDescription
        {
            get
            {
                return this.channelTypeDescription;
            }
            set
            {
                this.channelTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}

