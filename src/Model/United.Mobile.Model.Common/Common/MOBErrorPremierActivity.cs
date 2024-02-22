using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class ErrorPremierActivity
    {
        private string errorPremierActivityTitle;
        private string errorPremierActivityText;
        private bool showErrorIcon;
        public string ErrorPremierActivityTitle
        {
            get { return this.errorPremierActivityTitle; }
            set { this.errorPremierActivityTitle = value; }
        }
        public string ErrorPremierActivityText
        {
            get { return this.errorPremierActivityText; }
            set { this.errorPremierActivityText = value; }
        }
        public bool ShowErrorIcon
        {
            get { return this.showErrorIcon; }
            set { this.showErrorIcon = value; }
        }
    }
}
