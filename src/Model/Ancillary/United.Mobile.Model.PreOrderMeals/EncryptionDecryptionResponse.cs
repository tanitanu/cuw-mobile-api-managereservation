using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Exception;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class EncryptionDecryptionResponse
    {
        private string confirmationNumber;

        public string ConfirmationNumber
        {
            get { return confirmationNumber; }
            set { confirmationNumber = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string encryptedConfirmationNumber;

        public string EncryptedConfirmationNumber
        {
            get { return encryptedConfirmationNumber; }
            set { encryptedConfirmationNumber = value; }
        }

        private string encryptedLastName;

        public string EncryptedLastName
        {
            get { return encryptedLastName; }
            set { encryptedLastName = value; }
        }

        private string deepLink;

        public string DeepLink
        {
            get { return deepLink; }
            set { deepLink = value; }
        }

        private MOBException exception;

        public MOBException Exception
        {
            get { return exception; }
            set { exception = value; }
        }

    }

    [Serializable]
    public class EncryptionDecryptionRequest
    {
        private string confirmationNumber;

        public string ConfirmationNumber
        {
            get { return confirmationNumber; }
            set { confirmationNumber = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private int mode;

        public int Mode
        {
            get { return mode; }
            set { mode = value; }
        }

    }

    public enum EncryptionDecryptionMode
    {
        Encryption = 1,
        Decryption = 2
    }
}
