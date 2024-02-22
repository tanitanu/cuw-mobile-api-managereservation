using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public interface IEmployeeProfileService
    {
        Task<string> GetEmployeeProfile(string token, string path, string sessionId);
    }
}
