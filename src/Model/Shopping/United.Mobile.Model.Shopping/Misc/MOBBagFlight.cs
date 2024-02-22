using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BagFlight
    {
        private string airlineCode = string.Empty;
        private MOBAirport arrAirport;
        private MOBAirport depAirport;
        private string scanOnInd = string.Empty;
        private string fltLegLclDepDt = string.Empty;
        private string fltLegSqnr = string.Empty;

        public MOBAirport ArrAirport
        {
            get
            {
                return this.arrAirport;
            }
            set
            {
                this.arrAirport = value;
            }
        }

        public MOBAirport DepAirport
        {
            get
            {
                return this.depAirport;
            }
            set
            {
                this.depAirport = value;
            }
        }

        public string AirlineCode
        {
            get
            {
                return this.airlineCode;
            }
            set
            {
                this.airlineCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScanOnInd
        {
            get
            {
                return this.scanOnInd;
            }
            set
            {
                this.scanOnInd = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FltLegLclDepDt
        {
            get
            {
                return this.fltLegLclDepDt;
            }
            set
            {
                this.fltLegLclDepDt = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FltLegSqnr
        {
            get
            {
                return this.fltLegSqnr;
            }
            set
            {
                this.fltLegSqnr = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
