using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BagStatus
    {
        private string bagTagStatShortDesc = string.Empty;
        private string bagTagStatHistDesc = string.Empty;
        private string actnLclDtTm = string.Empty;
        private string actnGMTDtTm = string.Empty;
        private string actnStnCd = string.Empty;
        private string scanDateTime = string.Empty;

        public string BagTagStatShortDesc
        {
            get
            {
                return this.bagTagStatShortDesc;
            }
            set
            {
                this.bagTagStatShortDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BagTagStatHistDesc
        {
            get
            {
                return this.bagTagStatHistDesc;
            }
            set
            {
                this.bagTagStatHistDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActnLclDtTm
        {
            get
            {
                return this.actnLclDtTm;
            }
            set
            {
                this.actnLclDtTm = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActnGMTDtTm
        {
            get
            {
                return this.actnGMTDtTm;
            }
            set
            {
                this.actnGMTDtTm = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActnStnCd
        {
            get
            {
                return this.actnStnCd;
            }
            set
            {
                this.actnStnCd = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScanDateTime
        {
            get
            {
                return this.scanDateTime;
            }
            set
            {
                this.scanDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
