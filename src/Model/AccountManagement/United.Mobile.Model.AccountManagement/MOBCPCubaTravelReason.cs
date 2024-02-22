using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable()]
    public class MOBCPCubaTravelReason
    {
        private string headLine;

        public string HeadLine
        {
            get { return headLine; }
            set { headLine = value; }
        }

        private string vanity;

        public string Vanity
        {
            get { return vanity; }
            set { vanity = value; }
        }

        private string contentFull;

        public string ContentFull
        {
            get { return contentFull; }
            set { contentFull = value; }
        }


        private bool isInputRequired;

        public bool IsInputRequired
        {
            get { return isInputRequired; }
            set { isInputRequired = value; }
        }


    }
}
