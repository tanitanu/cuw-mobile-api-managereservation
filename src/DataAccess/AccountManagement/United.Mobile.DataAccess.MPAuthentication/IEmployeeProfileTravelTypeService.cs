using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public interface IEmployeeProfileTravelTypeService
    {
        Task<string> GetEmployeeProfileTravelType(string token, string path, string requestPayload, string sessionId);
    }
}
