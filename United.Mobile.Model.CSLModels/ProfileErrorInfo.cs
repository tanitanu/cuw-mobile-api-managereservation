using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ProfileErrorInfo
    {
        #region Properties
        /// <summary>
        /// CSL Major code which is assigned by the CSL team. Each service has unique major code.
        /// </summary>

        public string MajorCode { get; set; }

        /// <summary>
        /// Major description is name of the service.
        /// </summary>

        public string MajorDescription { get; set; }

        /// <summary>
        /// CSL minor code which is assigned by the CSL team. Each error message has unique minor code shared by all services.
        /// </summary>

        public string MinorCode { get; set; }

        /// <summary>
        /// Minor description associated with the minor code.
        /// </summary>

        public string MinorDescription { get; set; }

        /// <summary>
        /// This field has either system generate message or custom message.
        /// </summary>

        public string Message { get; set; }

        /// <summary>
        /// Type of error.
        /// </summary>

        public string ErrorType { get; set; }


        public string UserFriendlyMessageType { get; set; }


        public string UserFriendlyMessageNumber { get; set; }


        public string UserFriendlyMessage { get; set; }

        /// <summary>
        /// Exception/Error thrown time.
        /// </summary>

        public DateTime CallTime { get; set; }

        #endregion

        #region Methods

        public ProfileErrorInfo()
        {
            CallTime = DateTime.Now;
            ErrorType = ProfileErrorCodes.ErrorType.Other.ToString();
        }

        #region Clone
        public ProfileErrorInfo Clone()
        {
            return new ProfileErrorInfo
            {
                MajorCode = this.MajorCode,
                MajorDescription = this.MajorDescription,
                MinorCode = this.MinorCode,
                MinorDescription = this.MinorDescription,
                Message = this.Message,
                UserFriendlyMessage = this.UserFriendlyMessage,
                CallTime = this.CallTime
            };
        }
        #endregion

        #endregion
    }
}
