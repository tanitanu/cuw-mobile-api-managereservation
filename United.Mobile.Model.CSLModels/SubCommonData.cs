using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class SubCommonData : CommonData
    {
        /// <summary>
        /// It contains list of Error object. This will be always empty if status is 1(Success).
        /// </summary>

        public List<ProfileErrorInfo> Errors { get; set; } = new List<ProfileErrorInfo>();

        /// <summary>
        /// It has a request status. 0 for Failure and 1 for Success.
        /// </summary>

        public int Status { get; set; } = 1;

        /// <summary>
        /// This fields identify a source of data. DB, External Service or Cache
        /// </summary>

        public string Source { get; set; }

        /// <summary>
        /// The name of the server that served your request.
        /// </summary>

        public string ServerName
        {
            get
            {
                return Environment.MachineName;
            }
            protected set { } //Required for Serialization 
        }

        public void SetSourceData(string source)
        {
            this.Source = source;
        }

        public ProfileErrorCodes.ServiceMethodType ServiceMethod { get; set; }
    }
}
