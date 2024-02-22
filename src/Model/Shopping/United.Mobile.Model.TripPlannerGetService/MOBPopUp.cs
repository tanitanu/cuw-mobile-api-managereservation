using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBPopUp
    {
        private string headerText;   //HeaderText
        public string HeaderText
        {
            set { headerText = value;  }
            get { return headerText;  }
        }

        private string bodyContent;     //BodyContent
        public string BodyContent
        {
            set { bodyContent = value; }
            get { return bodyContent; }
        }

        private List<MOBActionButton> buttons;
        public List<MOBActionButton> Buttons
        {
            set { buttons = value; }
            get { return buttons; }
        }
    }
}
