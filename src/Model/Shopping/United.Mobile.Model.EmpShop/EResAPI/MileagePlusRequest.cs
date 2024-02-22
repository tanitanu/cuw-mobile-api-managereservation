using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    public class MileagePlusRequest
    {
        /// <summary>
        /// Get or  Sets DataSetting
        /// </summary>
        public string DataSetting { get; set; } = null;

        /// <summary>
        /// Get or  Sets DataToLoad
        /// </summary>
        public List<string> DataToLoad { get; set; }

        /// <summary>
        /// Get or  Sets LangCode
        /// </summary>
        public string LangCode { get; set; } = null;

        /// <summary>
        /// Get or  Sets MileagePlusId
        /// </summary>
        public string MileagePlusId { get; set; } = null;

        /// <summary>
        /// Get or  Sets MemberCustomerIdsToLoad
        /// </summary>
        public List<int> MemberCustomerIdsToLoad { get; set; }

        /// <summary>
        /// Get or  Sets MemberTravelerKeysToLoad
        /// </summary>
        public List<string> MemberTravelerKeysToLoad { get; set; }


        /// <summary>
        /// Get or Sets InsertId
        /// </summary>
        public string InsertId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Last name
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Mileageplus password
        /// </summary>
        public string MPPwd { get; set; } = string.Empty;

        /// <summary>
        /// Get or Sets UpdateId
        /// </summary>
        public string UpdateId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets SidaId
        /// </summary>
        public string SidaId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets LocationCode
        /// </summary>
        public string SidaLocationCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Sida Expiration Date
        /// </summary>
        public string SidaExpirationDate { get; set; } = string.Empty;
    }
}
