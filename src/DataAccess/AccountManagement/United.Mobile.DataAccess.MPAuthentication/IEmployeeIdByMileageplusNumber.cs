using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public interface IEmployeeIdByMileageplusNumber
    {
        Task<string> GetEmployeeIdy(string mileageplusNumber, string transactionId, string sessionId);
    }
}
