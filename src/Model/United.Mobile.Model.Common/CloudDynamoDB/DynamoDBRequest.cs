namespace United.Mobile.Model.Common.CloudDynamoDB
{
    public class DynamoDBRequest
    {
        private string transactionId;
        private string tableName;
        private string key;
        private string absoluteExpiration;
        private string data;
        private string secondaryKey;
        public string AbsoluteExpiration
        {
            get { return absoluteExpiration; }
            set { absoluteExpiration = value; }
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }

        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        public string SecondaryKey
        {
            get { return secondaryKey; }
            set { secondaryKey = value; }
        }
    }

    public class MileagePlusDetails
    {
        private string customerID;
        private string isTokenAnonymous;
        private string mileagePlusNumber;
        private string mPUserName;
        private string hashPincode;
        private string pinCode;
        private string applicationID;
        private string appVersion;
        private string deviceID;
        private string dataPowerAccessToken;
        private string authenticatedToken;
        private string isTokenValid;
        private string tokenExpireDateTime;
        private string tokenExpiryInSeconds;
        private string isTouchIDSignIn;
        private string updateDateTime;
        private long callDuration;
        public string IsTouchIDSignIn
        {
            get { return isTouchIDSignIn; }
            set { isTouchIDSignIn = value; }
        }

        public string TokenExpiryInSeconds
        {
            get { return tokenExpiryInSeconds; }
            set { tokenExpiryInSeconds = value; }
        }


        public string TokenExpireDateTime
        {
            get { return tokenExpireDateTime; }
            set { tokenExpireDateTime = value; }
        }

        public string IsTokenValid
        {
            get { return isTokenValid; }
            set { isTokenValid = value; }
        }

        public string AuthenticatedToken
        {
            get { return authenticatedToken; }
            set { authenticatedToken = value; }
        }

        public string DataPowerAccessToken
        {
            get { return dataPowerAccessToken; }
            set { dataPowerAccessToken = value; }
        }

        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }

        public string ApplicationID
        {
            get { return applicationID; }
            set { applicationID = value; }
        }

        public string PinCode
        {
            get { return pinCode; }
            set { pinCode = value; }
        }

        public string HashPincode
        {
            get { return hashPincode; }
            set { hashPincode = value; }
        }

        public string MPUserName
        {
            get { return mPUserName; }
            set { mPUserName = value; }
        }

        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }

        public string IsTokenAnonymous
        {
            get { return isTokenAnonymous; }
            set { isTokenAnonymous = value; }
        }

        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public string UpdateDateTime
        {
            get { return updateDateTime; }
            set { updateDateTime = value; }
        }

        public long CallDuration
        {
            get { return callDuration; }
            set { callDuration = value; }
        }



    }
}
