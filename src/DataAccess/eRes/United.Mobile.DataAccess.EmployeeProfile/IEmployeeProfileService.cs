using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.EmployeeProfile
{
    public interface IEmployeeProfileService
    {
        Task<string> GetEmployeeProfile(string token, string requestPayload, string sessionId);
    }
}
