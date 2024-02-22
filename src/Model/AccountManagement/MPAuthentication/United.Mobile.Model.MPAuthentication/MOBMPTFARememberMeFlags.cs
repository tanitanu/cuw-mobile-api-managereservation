using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBMPTFARememberMeFlags
    {
        public MOBMPTFARememberMeFlags()
        {
            showRememberMeDeviceButton = false; //TODO Convert.ToBoolean(ConfigurationManager.AppSettings["ShowRememberMeDeviceButton"].ToString());
            rememberMeDeviceSwitchON = false; //TODO Convert.ToBoolean(ConfigurationManager.AppSettings["RememberMeDeviceSwitchON"].ToString());

            string[] strMessages = null; //TODO ConfigurationManager.AppSettings["RememberMEButtonMessages"].ToString().Split('|');

            rememberMEButtonMessages = new List<MOBItem>();
            foreach (string msg in strMessages)
            {
                string id = msg.Split(',')[0];
                string currentValue = msg.Split(',')[1];
                rememberMEButtonMessages.Add(new MOBItem() { Id = id, CurrentValue = currentValue, SaveToPersist = true });
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
