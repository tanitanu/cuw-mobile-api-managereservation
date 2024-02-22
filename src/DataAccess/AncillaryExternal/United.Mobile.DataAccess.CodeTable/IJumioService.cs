using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CodeTable
{
    public interface IJumioService
    {
        Task<T> GetDeleteJumioScan<T>(string token, string passportScanReferenceID, string sessionId);
    }
}
