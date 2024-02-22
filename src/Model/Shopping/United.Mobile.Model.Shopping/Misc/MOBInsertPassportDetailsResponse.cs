using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class InsertPassportDetailsResponse : MOBResponse
    {
        public InsertPassportDetailsResponse()
            : base()
        {
        }

        private string insertedRowID;

        public string InsertedRowID
        {
            get { return this.insertedRowID; }
            set
            {
                this.insertedRowID = value;
            }
        }
    }
    //[Serializable()]
    //public class JumioRequestType
    //{
    //    private string idExpiry = "";
    //    public string idType  { get; set; }
    //    public string idDob  { get; set; }
    //    public string idCheckSignature  { get; set; }
    //    public string idCheckDataPositions  { get; set; }
    //    public string idCheckHologram  { get; set; }
    //    public string idCheckMicroprint  { get; set; }
    //    public string idCheckDocumentValidation  { get; set; }
    //    public string idCountry  { get; set; }
    //    public string idScanSource  { get; set; }
    //    public string idFirstName  { get; set; }
    //    public string verificationStatus  { get; set; }
    //    public string jumioIdScanReference  { get; set; }
    //    public string personalNumber  { get; set; }
    //    public string merchantIdScanReference  { get; set; }
    //    public string idCheckSecurityFeatures  { get; set; }
    //    public string idCheckMRZcode  { get; set; }
    //    public string idScanImage  { get; set; }
    //    public string callBackType  { get; set; }
    //    public string clientIp  { get; set; }
    //    public string idLastName  { get; set; }
    //    public string idAddress  { get; set; }
    //    public string idScanStatus  { get; set; }
    //    public string idNumber  { get; set; }
    //    public string rejectReason { get; set; }

    //    [DefaultValue("")]
    //    public string IdExpiry
    //    {
    //        get
    //        {
    //            return idExpiry;
    //        }
    //        set
    //        {
    //            idExpiry = value;
    //        }
    //    }
    
    //}
}
