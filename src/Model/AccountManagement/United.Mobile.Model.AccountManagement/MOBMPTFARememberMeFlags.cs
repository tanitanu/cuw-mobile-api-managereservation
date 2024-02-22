using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPTFARememberMeFlags
    {
        private readonly IConfiguration _configuration;
        public MOBMPTFARememberMeFlags()
        {

        }
        public MOBMPTFARememberMeFlags(IConfiguration configuration)
        {
            _configuration = configuration;
            showRememberMeDeviceButton = _configuration.GetValue<bool>("ShowRememberMeDeviceButton");
            rememberMeDeviceSwitchON = _configuration.GetValue<bool>("RememberMeDeviceSwitchON");

            string[] strMessages = configuration.GetValue<string>("RememberMEButtonMessages").Split('|');

            rememberMEButtonMessages = new List<MOBItem>();
            if (strMessages != null)
            {
                foreach (string msg in strMessages)
                {
                    string id = msg.Split(',')[0];
                    string currentValue = msg.Split(',')[1];
                    rememberMEButtonMessages.Add(new MOBItem() { Id = id, CurrentValue = currentValue, SaveToPersist = true });
                }
            }
        }

        private List<MOBItem> rememberMEButtonMessages;
        public List<MOBItem> RememberMEButtonMessages
        {
            get
            {

                return rememberMEButtonMessages;
            }
            set
            {
                this.rememberMEButtonMessages = value;
            }
        }

        #region Sample values of RememberMEButtonMessages
        //{ 
        //    "MOBMPTFARememberMeFlags": [{
        //        "id": "ButtonTitle",
        //        "currentValue": "Remembe me on this device"
        //    }, {
        //        "id": "ButtonTagLine",
        //        "currentValue": "You won't have to answere security questions again"
        //    }]
        //}
        #endregion

        private bool showRememberMeDeviceButton;
        public bool ShowRememberMeDeviceButton
        {
            get
            {

                return showRememberMeDeviceButton;
            }
            set { showRememberMeDeviceButton = value; }
        }

        private bool rememberMeDeviceSwitchON;
        public bool RememberMeDeviceSwitchON
        {
            get
            {

                return rememberMeDeviceSwitchON;
            }
            set { rememberMeDeviceSwitchON = value; }
        }
    }
}
