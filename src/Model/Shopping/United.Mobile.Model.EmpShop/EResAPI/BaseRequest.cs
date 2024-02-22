using System.Text.RegularExpressions;

namespace United.Mobile.Model.Common
{
    public class BaseRequest
    {

        #region private property
        /// <summary>
        /// private property for search type
        /// </summary>
        private string searchtype { get; set; }

        /// <summary>
        /// private property for TransactionId
        /// </summary>
        private string transactionId { get; set; }

        /// <summary>
        /// private property for TransactionId
        /// </summary>
        private string bookingTravelType { get; set; }

        /// <summary>
        /// private property for TransactionId
        /// </summary>
        private string bookingsessionId { get; set; }


        /// <summary>
        /// private property for employeeID
        /// </summary>
        private string employeeID { get; set; } = string.Empty;

        #endregion

        /// <summary>
        /// Gets or sets TransactionId
        /// </summary>
        public string TransactionId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(transactionId))
                {
                    return transactionId.ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                transactionId = value;
            }
        }

        /// <summary>
        /// Gets or sets SecureToken
        /// </summary>
        public string SecureToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Booking TravelType
        /// </summary>
        public string BookingTravelType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(bookingTravelType))
                {
                    return bookingTravelType.ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                bookingTravelType = value;
            }
        }

        /// <summary>
        /// Gets or sets Serch Type
        /// </summary>
        public string SearchType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(searchtype))
                {
                    return searchtype.ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                searchtype = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is ChangeSegment 
        /// </summary>
        public bool IsChangeSegment { get; set; } = false;

        /// <summary>
        /// Gets or Sets EmployeeId
        /// </summary>
        public string EmployeeId { get; set; } = string.Empty;

        
        /// <summary>
        ///   Gets or sets a value indicating whether the user is login as Agent Admin Tool
        /// </summary>
        public bool IsAgentToolLogOn { get; set; } = false;

        /// <summary>
        /// Gets ot Sets Bookingsessionid
        /// </summary>
        public string BookingSessionId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(bookingsessionId))
                {
                    return bookingsessionId.ToUpper();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                bookingsessionId = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the log in is of pass rider or employee.
        /// </summary>
        public bool IsPassRiderLoggedIn { get; set; } = false;

        /// <summary>
        /// Gets ot Sets BoardDate
        /// </summary>
        public string BoardDate { get; set; } = string.Empty;

        /// <summary>
        ///  Gets ot Sets loadPassRider
        /// </summary>
        public bool loadPassRider { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating Pass Rider LoggedIn ID.
        /// </summary>
        public string PassRiderLoggedInID { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating Pass Rider LoggedIn User.
        /// </summary>
        public string PassRiderLoggedInUser { get; set; } = string.Empty;

        /// <summary>
        /// Validate employeeid
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static bool IsValidEmployeeId(string employeeId)
        {

            if (string.IsNullOrWhiteSpace(employeeId))
                return false;

            //All the request from UI will be encrypted. If plain value is sent return false since from fiddler, user can get anyemployee details, so do not accept plain employee id
            if (employeeId.Length == 7 || employeeId.Length == 6 || Regex.IsMatch(employeeId, @"^[A-Za-z]{1}[0-9]{6}\z") || Regex.IsMatch(employeeId, @"^[0-9]*$"))
                return false;

            return true;
        }
    }
}