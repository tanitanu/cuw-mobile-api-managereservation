using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public interface IEResEmployeeProfileService
    {
        Task<string> GetEResEmployeeProfile(string token, string path, string requestPayload, string sessionId);
        Task<T> GetEResEmpProfile<T>(string token, string path, string sessionId);
    }
}
