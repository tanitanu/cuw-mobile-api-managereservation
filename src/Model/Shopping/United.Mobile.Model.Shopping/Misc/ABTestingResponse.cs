using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class ABTestingResponse :MOBResponse
    {
        private List<ABSwitchOption> items;

        public List<ABSwitchOption> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }
    }
}
