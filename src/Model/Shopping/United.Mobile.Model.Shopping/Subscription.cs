using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    public class Subscription : PersistBase, IPersist
    {
        public Subscription() { }
        public Subscription(string json, string objectType)
        {
            Json = json;
            ObjectType = objectType;
        }

        #region IPersist Members
        private string objectName = "United.Persist.Definition.Subscription.Subscription";

        public string ObjectName 
        {
            
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }
   
        #endregion

        private string mpNumber = "";

        public string MPNumber
        {

            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = value;
            }
        }

        private string callDuration = "";

        public string CallDuration
        {

            get
            {
                return this.callDuration;
            }
            set
            {
                this.callDuration = value;
            }
        }

        private string requestXml = "";
        public string RequestXml
        {

            get
            {
                return this.requestXml;
            }
            set
            {
                this.requestXml = value;
            }
        }

        private string responseXml = "";
        public string ResponseXml
        {

            get
            {
                return this.responseXml;
            }
            set
            {
                this.responseXml = value;
            }
        }
    }
}
