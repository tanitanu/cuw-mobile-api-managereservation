using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBAlertMessages
    {
        public string HeaderMessage { get; set; }

        public List<MOBSection> AlertMessages { get; set; }

        public bool IsDefaultOption { get; set; }

        public string MessageType { get; set; }
        public MOBAlertMessages()
        {
            AlertMessages = new List<MOBSection>();
        }
    }
}
