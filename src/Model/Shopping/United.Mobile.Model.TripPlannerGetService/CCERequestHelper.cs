using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.TripPlannerGetService
{
    public class CCERequestHelper
    {
        private string mileagePlusNumber;
        private List<string> componentToLoad;
        private string pageToLoad;
        private MOBRequest mOBRequest;
        private List<Characteristic> characteristics;
        private string sessionId;
        private string logAction;

        public string LogAction
        {
            get { return logAction; }
            set { logAction = value; }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public List<Characteristic> Characteristics
        {
            get { return characteristics; }
            set { characteristics = value; }
        }

        public MOBRequest MOBRequest
        {
            get { return mOBRequest; }
            set { mOBRequest = value; }
        }


        public string PageToLoad
        {
            get { return pageToLoad; }
            set { pageToLoad = value; }
        }

        public List<string> ComponentToLoad
        {
            get { return componentToLoad; }
            set { componentToLoad = value; }
        }

        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }
    }
}
