using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace United.Mobile.Model
{
    public static class Constants
    {
        public const string HeaderAppIdText = "X_APP_ID";
        public const string HeaderDeviceIdText = "X_DEVICE_ID";
        public const string HeaderAppMinorText = "X_APP_MINOR";
        public const string HeaderAppMajorText = "X_APP_MAJOR";
        public const string HeaderLangCodeText = "X_LANG_CODE";
        public const string HeaderRequestTimeUtcText = "X_REQUEST_TIME_UTC";
        public const string HeaderTransactionIdText = "X_TRANSACTION_ID";
        public const string HeaderMileagePlusText = "X_MILEAGEPLUS_NUMBER";
        public const string HeaderhashPincode = "X_HASH_PIN_CODE";
        public const string HeaderSessionIdText = "X_SESSION_ID";
        public const string ErrorInvalidHeaderErrorDetail = "The {0} header is missing or invalid";
        public const string ErrorInvalidHeaderErrorMessage = "Please check the headers";

        public const string ContentTypeJsonText = "application/json";
        public const string ErrorUnauthorizedText = "User unauthorized";

        public const string DeviceIdText = "DeviceId";
        public const string LoggingContextText = "LoggingContext";
        public const string TransactionIdText = "TransactionId";
        public const string MessageText = "MessageText";
        public const string MessageTypeText = "MessageType";
        public const string ApplicationIdText = "ApplicationId";
        public const string ApplicationVersionText = "ApplicationVersion";
        public const string ServiceNameText= "ServiceName";
        public const string EnvironmentText= "Environment";
        public const string SessionId = "SessionId";
        public const string BagTagNumberText = "BagTagNumber";
        public const string RecordLocatorText = "RecordLocator";
        public const string LastNameText = "LastName";
        public const string AirportCode = "AirportCode";
        public const string ViewResFlow_ProductMapping = "PCU,PAS,PBS,APA";
        public const string ViewResFlow_VendorProductMapping = "TPI";
        public const string Credit_CardTypeDescription = "CardTypeDescription";
        public const string Credit_cCName = "cCName";
        public const string MileagePlusNumber = "mPNumber";
        public const string HashPinCode = "hashPinCode";
        public const string PostBookingFlow_ProductMapping = "PCU,PBS,TPI";
        public const string ErrorNotFound = "not_found";
        public static readonly string PublicBucketName = "dbpu";
        public static readonly string SemiPrivateBucketName = "dbsp";
        public static readonly string PrivateBucketName = "dbpi";
        public const string PublicChannelUserPrefix = "PU_PUBLICMOBILE";
        public const string SemiPrivateUserPrefix = "SP_SPMOBILE";
        //Security - Upload documents valid signature.
        public const string PNGDocumentSignature = "89-50-4E-47";
        public const string PDFDocumentSignature = "25-50-44-46";
        public const string JPG_JPEGDocumentSignature = "FF-D8";
        public const string HEICDocumentSignature = "00-00-00";
        public const string PNGDocumentExtension = "png";
        public const string PDFDocumentExtension = "pdf";
        public const string JPG_JPEGDocumentExtension = "jpeg";
        public const string HEICDocumentExtension = "heic";
        public const string LoggingContextSeparator = "::";
        public const string ServerName = "Server Name";
        public static readonly IList<string> HeaderTextList = new ReadOnlyCollection<string>(new List<string> {
            HeaderAppIdText,
            HeaderDeviceIdText,
            HeaderAppMajorText,
            HeaderAppMinorText,
            HeaderLangCodeText,
            HeaderRequestTimeUtcText,
            HeaderTransactionIdText

        });
    }
 }