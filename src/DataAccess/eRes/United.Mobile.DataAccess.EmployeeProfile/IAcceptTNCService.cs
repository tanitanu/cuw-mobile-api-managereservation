using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.EmployeeProfile
{
    public interface IAcceptTNCService
    {
        Task<string> AcceptTNC(string token, string encryptedEmployeeId, string sessionId);
    }
}
