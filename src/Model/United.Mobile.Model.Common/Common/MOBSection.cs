using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBSection
    {
        public string Text1 { get; set; } = string.Empty;
        public string Text2 { get; set; } = string.Empty;
        public string Text3 { get; set; }
        public string Order { get; set; } = string.Empty;
        public string MessageType { get; set; }
        private bool isDefaultOpen = true;


        public bool IsDefaultOpen
        {
            get
            {
                return this.isDefaultOpen;
            }
            set
            {
                this.isDefaultOpen = value;
            }
        }
    }
}
