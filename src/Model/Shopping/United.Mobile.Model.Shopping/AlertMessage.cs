using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBAlertMessages
    {
        public string HeaderMessage { get; set; }
        
        public List<Section> AlertMessages { get; set; }

        public bool IsDefaultOption { get; set; }
        
        public string MessageType { get; set; }
        public MOBAlertMessages()
        {
            AlertMessages = new List<Section>();
        }
    }
}
