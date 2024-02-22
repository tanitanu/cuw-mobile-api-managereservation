using System;
using System.Collections.Generic;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFOPTravelerCertificateRequest : ShoppingRequest
    {
        private MOBFOPCertificate certificate;
        private bool isRemove;

        private List<MOBFOPCertificate> combineCertificates;
        public List<MOBFOPCertificate> CombineCertificates
        {
            get { return combineCertificates; }
            set { combineCertificates = value; }
        }

        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; }
        }

        public MOBFOPCertificate Certificate
        {
            get { return certificate; }
            set { certificate = value; }
        }

    }
}
