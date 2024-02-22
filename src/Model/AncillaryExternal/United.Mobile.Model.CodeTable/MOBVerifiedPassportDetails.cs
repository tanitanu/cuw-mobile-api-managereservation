using System;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBVerifiedPassportDetails
    {
        public int ID { get; set; }

        public string IDExpiry { get; set; } 
        
        public string IDType { get; set; } = string.Empty;

        public string IDDob { get; set; } = string.Empty;

        public string IDCheckSignature { get; set; } = string.Empty;

        public string IDCheckDataPositions { get; set; } = string.Empty;

        public string IDCheckHologram { get; set; } = string.Empty;

        public string IDCheckMicroprint { get; set; } = string.Empty;

        public string IDCheckDocumentValidation { get; set; } = string.Empty;

        public string IDCountry { get; set; } = string.Empty;

        public string IDScanSource { get; set; } = string.Empty;

        public string IDFirstName { get; set; } = string.Empty;

        public string VerificationStatus { get; set; } = string.Empty;

        public string JumioIdScanReference { get; set; } = string.Empty;

        public string PersonalNumber { get; set; } = string.Empty;

        public string MerchantIdScanReference { get; set; } = string.Empty;

        public string IDCheckSecurityFeatures { get; set; } = string.Empty;

        public string IDCheckMRZcode { get; set; } = string.Empty;

        public string IDScanImage { get; set; } = string.Empty;

        public string CallBackType { get; set; } = string.Empty;

        public string ClientIp { get; set; } = string.Empty;

        public string IDLastName { get; set; } = string.Empty;

        public string IDAddress { get; set; } = string.Empty;

        public string IDScanStatus { get; set; } = string.Empty;

        public string IDNumber { get; set; } = string.Empty;

        public string RejectReason { get; set; } = string.Empty;

        public string IDGender { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string CountryName { get; set; } = string.Empty;

        public string InsertedDateTime { get; set; } = string.Empty;

        public string UpdatedDateTime { get; set; } = string.Empty;

        public string DurationOfStay { get; set; } = string.Empty;

        public string VisaCategory { get; set; } = string.Empty;

        public string NumberOfEntries { get; set; } = string.Empty;

        public string PassportNumber { get; set; } = string.Empty;

        public string IssuingDate { get; set; } = string.Empty;

        public string Nationality { get; set; } = string.Empty;
    }

    [Serializable()]
    public class MOBDeleteScanJumioResponse
    {
        public DateTime timestamp { get; set; }
        public string jumioIdScanReference { get; set; }
        public string status { get; set; }
    }

}
