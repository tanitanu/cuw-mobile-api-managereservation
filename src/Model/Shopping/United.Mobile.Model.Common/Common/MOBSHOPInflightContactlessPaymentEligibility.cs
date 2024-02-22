using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPInflightContactlessPaymentEligibility
    {
        [JsonIgnore]
        public string ObjectName { get; set; } = "United.Definition.Shopping.MOBSHOPInflightContactlessPaymentEligibility";
        private bool isEligibleInflightCLPayment;
        private string optTitle;
        private string optMessage;
        private bool isCCSelectedForContactless;

        public bool IsCCSelectedForContactless
        {
            get { return isCCSelectedForContactless; }
            set { isCCSelectedForContactless = value; }
        }

        public MOBSHOPInflightContactlessPaymentEligibility() { }
        public MOBSHOPInflightContactlessPaymentEligibility(bool isEligible, string title, string msg)
        {
            isEligibleInflightCLPayment = isEligible;
            optTitle = title;
            optMessage = msg;
        }

        public bool IsEligibleInflightCLPayment
        {
            get { return isEligibleInflightCLPayment; }
            set { isEligibleInflightCLPayment = value; }
        }

        public string OptTitle
        {
            get { return optTitle; }
            set { optTitle = value; }
        }

        public string OptMessage
        {
            get { return optMessage; }
            set { optMessage = value; }
        }
    }
}