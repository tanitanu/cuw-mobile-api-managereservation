using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBLMXRow
    {
        private string segmentText = string.Empty;
        private string tripText = string.Empty;
        private string awardMileText = string.Empty;
        private string pqmText = string.Empty;
        private string pqsText = string.Empty;
        private string pqdText = string.Empty;
        private string pqfText = string.Empty;
        private string pqpText = string.Empty;
        private bool isElligibleForEarnings; //drives operated by and showing inelligble
        private string ineligibleEarningsText = string.Empty;
        private string secondLineText = string.Empty;

        [JsonProperty(PropertyName = "pqfText")]
        [JsonPropertyName("pqfText")]
        public string PQF
        { get { return this.pqfText; } set { this.pqfText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        
        [JsonProperty(PropertyName = "pqpText")]
        [JsonPropertyName("pqpText")]
        public string PQP
        { get { return this.pqpText; } set { this.pqpText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        
        [JsonProperty(PropertyName = "tripText")]
        [JsonPropertyName("tripText")]
        public string Trip
        { get { return this.tripText; } set { this.tripText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }

        //[Newtonsoft.Json.JsonIgnore]
        //[JsonProperty(PropertyName = "tripText")]
        //[JsonPropertyName("tripText")]
        //public string TripNull
        //{ get { return this.tripText; } set { this.tripText = value; } }

        [JsonProperty(PropertyName = "segmentText")]
        [JsonPropertyName("segmentText")]
        public string Segment
        {
            get
            {
                return this.segmentText;
            }
            set
            {
                this.segmentText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }

        [JsonProperty(PropertyName = "awardMileText")]
        [JsonPropertyName("awardMileText")]
        public string AwardMiles
        {
            get
            {
                return this.awardMileText;
            }
            set
            {
                this.awardMileText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }

        [JsonProperty(PropertyName = "pqmText")]
        [JsonPropertyName("pqmText")]
        public string PQM
        {
            get
            {
                return this.pqmText;
            }
            set
            {
                this.pqmText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }

        [JsonProperty(PropertyName = "pqsText")]
        [JsonPropertyName("pqsText")]
        public string PQS
        {
            get
            {
                return this.pqsText;
            }
            set
            {
                this.pqsText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }

        [JsonProperty(PropertyName = "pqdText")]
        [JsonPropertyName("pqdText")]
        public string PQD
        {
            get
            {
                return this.pqdText;
            }
            set
            {
                this.pqdText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }

        [JsonProperty(PropertyName = "isElligibleForEarnings")]
        [JsonPropertyName("isElligibleForEarnings")]
        public bool IsEligibleSegment
        {
            get
            {
                return this.isElligibleForEarnings;
            }
            set
            {
                this.isElligibleForEarnings = value;
            }
        }

        [JsonProperty(PropertyName = "ineligibleEarningsText")]
        [JsonPropertyName("ineligibleEarningsText")]
        public string IneligibleSegmentMessage
        {
            get
            {
                return this.ineligibleEarningsText;
            }
            set
            {
                this.ineligibleEarningsText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }


        [JsonProperty(PropertyName = "secondLineText")]
        [JsonPropertyName("secondLineText")]
        public string OperatingCarrierDescription
        {
            get
            {
                return this.secondLineText;
            }
            set
            {
                this.secondLineText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }

        }
    }
}
